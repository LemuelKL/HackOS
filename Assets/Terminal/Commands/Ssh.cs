using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ssh : MonoBehaviour
{
    public static Ssh instance;
    string username;
    void Awake()
    {
        instance = this;
    }
    public static void Execute(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();

        bool validArgs = true;
        if (flagArgs.Count != 0)
            validArgs = false;
        if (cmdArgs.Count != 1)
            validArgs = false;
        if (!validArgs)
        {
            TM.PrintResponseLine("unknown arguments...");
            TM.FinishCommand();
            return;
        }

        string[] creds = cmdArgs[0].Split("@");

        if (creds.Length != 2)
        {
            TM.PrintResponseLine("unkown arguments...");
            TM.FinishCommand();
            return;
        }

        string username = creds[0];
        string ip = creds[1];

        if (!internet.PingServer(ip))
        {
            TM.PrintResponseLine(string.Format("[ERROR] failed to establish connection to {0}", ip));
            TM.PrintResponseLine("Exiting...");
            TM.FinishCommand();
            return;
        }

        Internet.Server remoteServer = internet.GetServer(ip);

        if (!remoteServer.HasService("ssh"))
        {
            TM.PrintResponseLine(string.Format("[ERROR] {0} has no SSH service", ip));
            TM.PrintResponseLine("Exiting...");
            TM.FinishCommand();
            return;
        }

        Internet.Server.SSH sshService = remoteServer.GetService("ssh") as Internet.Server.SSH;
        InputField passwordInput = TM.PrintInputPrompt("Enter password: ");
        passwordInput.enabled = true;
        passwordInput.ActivateInputField();
        passwordInput.interactable = true;
        passwordInput.Select();
        passwordInput.onSubmit.RemoveAllListeners();
        passwordInput.onSubmit.AddListener((password) =>
        {
            Destroy(passwordInput.GetComponent<DynamicInputHandler>());
            passwordInput.textComponent.color = Color.white;
            passwordInput.interactable = false;
            if (sshService.Login(username, password))
            {
                TM.PrintResponseLine("Established SSH session with " + ip);
                TM.FinishCommand();
                TM.PushMachineSession(username, ip);
            }
            else
            {
                {
                    TM.PrintResponseLine("Incorrect password!");
                    TM.FinishCommand();
                }
            }
        });
    }
}
