using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TerminalManager : MonoBehaviour
{
    public GameObject prompt;
    public InputField terminalInput;
    public GameObject terminalContainer;
    public ScrollRect sr;
    public GameObject interactableLine;

    Interpreter interpreter;
    FileSystemManager fsm;

    public class Session
    {
        public string user;
        public string hostname;
        public string workingDirectory;
        public FileSystem.FileNode workingDirectoryFileNode;

        public Session(string user, string hostname, string workingDirectory)
        {
            this.user = user;
            this.hostname = hostname;
            this.workingDirectory = workingDirectory;
            FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();
            this.workingDirectoryFileNode = FSM.GetFileNode(hostname, workingDirectory);
        }
    }

    public List<Session> sessions;
    private bool executing;
    public Dictionary<int, string> commandHistory;
    int cursor;

    private string GetHomePath()
    {
        return "/home/" + sessions.Last().user;
    }
    private bool IsUnderHomeDir()
    {
        try
        {
            return (sessions.Last().workingDirectory.Substring(0, 6 + sessions.Last().user.Length) == "/home/" + sessions.Last().user);
        }
        catch (System.ArgumentOutOfRangeException)
        {
            return false;
        }
    }
    public bool IsAtRoot()
    {
        return (sessions.Last().workingDirectory == "" && sessions.Last().workingDirectoryFileNode.name == "");
    }

    private string ComputedPrompt()
    {
        Session curS = sessions.Last();
        if (IsAtRoot())
            return System.String.Format("{0}@{1}:{2}$ ", curS.user, curS.hostname, "/");
        if (IsUnderHomeDir())
            return System.String.Format("{0}@{1}:{2}$ ", curS.user, curS.hostname, "~" + curS.workingDirectory.Substring(6 + curS.user.Length));
        return System.String.Format("{0}@{1}:{2}$ ", curS.user, curS.hostname, sessions.Last().workingDirectory);
    }

    private void Awake()
    {
        interpreter = GetComponent<Interpreter>();
        fsm = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>(); // legal since only one FSM through-out
    }
    void Start()
    {
        sessions = new List<Session>();
        Session session = new Session("root", "hack0s", "/home/root");
        sessions.Add(session);
        commandHistory = new Dictionary<int, string>();
    }

    public void PushMachineSession(string user, string hostname)
    {
        Session session = new Session(user, hostname, "/home/" + user);
        sessions.Add(session);
    }

    public void PopMachineSession()
    {
        if (sessions.Count > 1)
            sessions.RemoveAt(sessions.Count - 1);
    }

    // Should be safe
    public bool ChangeWorkingDirectory(string fullPath, FileSystem.FileNode fileNode = null)
    {
        FileSystem.FileNode destFileNode = null;
        if (fileNode == null)
        {
            destFileNode = fsm.GetFileNode(sessions.Last().hostname, fullPath);
        }
        else
        {
            destFileNode = fileNode;
        }
        if (destFileNode != null)
        {
            sessions.Last().workingDirectoryFileNode = destFileNode;
            sessions.Last().workingDirectory = fullPath;
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        if (!executing)
            prompt.GetComponent<Text>().text = ComputedPrompt();
        RectTransform promptRT = (RectTransform)prompt.transform;
        RectTransform terminalRT = terminalInput.GetComponent<RectTransform>();
        terminalRT.anchoredPosition = new Vector2(promptRT.rect.width, promptRT.anchoredPosition.y);
    }

    public void EnterLine()
    {
        if (terminalInput.text != "")
        {
            string userInput = terminalInput.text;

            terminalInput.interactable = false;
            terminalInput.DeactivateInputField();
            ClearInputField();

            PrintLine(userInput);
            commandHistory.Add(commandHistory.Count, userInput);
            cursor = commandHistory.Count; // future
            executing = true;
            interpreter.Interpret(userInput);

            interactableLine.transform.SetAsLastSibling();
            interactableLine.GetComponentInChildren<Text>().text = "";
        }
    }

    // let other components inform this TM that their command has finished and so we proceed printing the next prompt
    public void FinishCommand()
    {
        executing = false;
        interactableLine.transform.SetAsLastSibling();
        terminalInput.interactable = true;
        terminalInput.ActivateInputField();
        terminalInput.Select();
    }

    public void FinishCommand(string command, int exitCode)
    {
        switch (exitCode)
        {
            case 0:
                PrintResponseLine(string.Format("<color=green>[SUCCESS]</color> {0} exitted with code {1}", command, exitCode));
                break;
            default:
                PrintResponseLine(string.Format("<color=red>[ERROR]</color> {0} exitted with code {1}", command, exitCode));
                break;
        }
        FinishCommand();
    }
    void ClearInputField()
    {
        terminalInput.text = "";
    }

    /// <summary>
    /// Destorys all children of terminal container except the last one (the prompt line)
    /// Also resets the size of terminal container to default
    /// </summary>
    public void ClearTerminalOutput()
    {
        int n = terminalContainer.transform.childCount;
        List<GameObject> toDestroy = new List<GameObject>();
        int numDestoyed = 0;
        for (int i = 0; i < n; i++)
        {
            GameObject line = terminalContainer.transform.GetChild(i).gameObject;
            if (line.name == "Line(Clone)")
            {
                toDestroy.Add(line);
                numDestoyed++;
            }
        }
        foreach (GameObject go in toDestroy)
        {
            Destroy(go);
        }
        Vector2 msgListSize = terminalContainer.GetComponent<RectTransform>().sizeDelta;
        terminalContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y - 21.0f * numDestoyed);
    }

    void PrintLine(string userInput)
    {
        Vector2 msgListSize = terminalContainer.GetComponent<RectTransform>().sizeDelta;
        terminalContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 21.0f);
        GameObject writtenLine = Instantiate(interactableLine, terminalContainer.transform);
        writtenLine.transform.SetSiblingIndex(terminalContainer.transform.childCount - 1);
        InputField writtenInput = writtenLine.GetComponentInChildren<InputField>();
        writtenInput.text = userInput;
        writtenInput.interactable = false;
        writtenInput.textComponent.color = Color.white;
        Destroy(writtenInput.GetComponent<DynamicInputHandler>());
    }

    public void PrintResponseLines(List<string> responseLines)
    {
        for (int i = 0; i < responseLines.Count; i++)
        {
            PrintResponseLine(responseLines[i]);
        }
    }

    // exposed to other components
    // return the Text obj so that caller could have further access
    public Text PrintResponseLine(string response)
    {
        GameObject resLine = Instantiate(interactableLine, terminalContainer.transform);
        Destroy(resLine.transform.GetChild(1).gameObject); // delete InputField
        resLine.transform.SetAsLastSibling();
        Vector2 msgListSize = terminalContainer.GetComponent<RectTransform>().sizeDelta;
        terminalContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 21.0f);
        Text resText = resLine.GetComponentInChildren<Text>();
        resText.text = response;
        interactableLine.transform.SetAsLastSibling();
        return resText;
    }

    public InputField PrintInputPrompt(string prompt)
    {
        GameObject resLine = Instantiate(interactableLine, terminalContainer.transform);
        resLine.transform.SetAsLastSibling();
        Vector2 msgListSize = terminalContainer.GetComponent<RectTransform>().sizeDelta;
        terminalContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 21.0f);
        Text resText = resLine.GetComponentInChildren<Text>();
        resText.text = prompt;
        interactableLine.transform.SetAsLastSibling();
        return resLine.transform.GetChild(1).gameObject.GetComponent<InputField>();
    }
    public string QPreviousCommand()
    {
        cursor--;
        if (commandHistory.ContainsKey(cursor))
        {
            string command = commandHistory[cursor];
            return command;
        }
        cursor = 0;
        if (commandHistory.ContainsKey(cursor))
            return commandHistory[cursor];
        else
            return "";
    }

    public string QNextCommand()
    {
        cursor++;
        if (commandHistory.ContainsKey(cursor))
        {
            string command = commandHistory[cursor];
            return command;
        }
        cursor = commandHistory.Count;
        if (commandHistory.ContainsKey(cursor - 1))
            return commandHistory[cursor - 1];
        else
            return "";
    }
}
