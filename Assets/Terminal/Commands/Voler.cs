using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Voler : MonoBehaviour
{
    public static Voler instance;
    void Awake()
    {
        instance = this;
    }
    // voler -l root -p toor ip /home/root/comp3329/exam.pdf
    public static void Execute(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        if (flagArgs.Count != 2)
        {
            TM.PrintResponseLine("Unknown flag/s in arugment!");
            TM.FinishCommand("Voler", 1);
            return;
        }
        if (cmdArgs.Count != 2)
        {
            TM.PrintResponseLine("Voler takes exactly two arugments IP and PATH!");
            TM.FinishCommand("Voler", 2);
            return;
        }

        instance.StartCoroutine(instance.Hack(TM, flagArgs, cmdArgs));
    }

    public IEnumerator Hack(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        string ip = cmdArgs[0];
        string filePath = cmdArgs[1];
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();

        Internet.Server targetServer = internet.GetServer(ip);
        TM.PrintResponseLine("");
        yield return new WaitForSeconds(1);
        TM.PrintResponseLine("Starting Voler 1.0.0");
        yield return new WaitForSeconds(1);
        if (targetServer == null)
        {
            TM.PrintResponseLine("<color=red>[ERROR]</color> no respsone from server " + ip);
            TM.FinishCommand("Voler", 0);
            yield break;
        }

        Internet.Server.SSH targetSSH = targetServer.GetService("ssh") as Internet.Server.SSH;

        string spinners = "◢◣◤◥";
        Text scanningLine = TM.PrintResponseLine("");
        for (int i = 0; i < 3; i++)
        {
            scanningLine.text = spinners[i % 4].ToString() + " scanning for SSH service...";
            yield return new WaitForSeconds(1);
        }

        if (targetSSH == null)
        {
            scanningLine.text = "<color=red>[ERROR]</color> Voler cannot find any running SSH service!";
            TM.FinishCommand("Voler", 11);
            yield break;
        }
        else
        {
            TM.PrintResponseLine("Talking to remote...");

            bool correctLogin = targetSSH.Login(flagArgs["-l"], flagArgs["-p"]);
            // bool correctLogin = targetSSH.Login("root", "toor");
            if (!correctLogin)
            {
                TM.PrintResponseLine("<color=red>[ERROR]</color> Incorrect SSH credentials!");
                TM.FinishCommand("Voler", 12);
                yield break;
            }

            TM.PrintResponseLine("Got SSH session...");
            yield return new WaitForSeconds(1);
            TM.PrintResponseLine("Requesting file " + filePath);

            FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();
            FileSystem.FileNode fn = FSM.GetFileNode(ip, filePath);

            if (fn == null || fn.isDirectory)
            {
                TM.PrintResponseLine("<color=red>[ERROR]</color> File does not exist!");
                TM.FinishCommand("Voler", 20);
                yield break;
            }

            Text downloadProgress = TM.PrintResponseLine("");
            for (int i = 0; i <= 10; i++)
            {
                downloadProgress.text = string.Format("[{0}{1}] Downloading...", new string('-', i), new string('_', 10 - i));
                yield return new WaitForSeconds(1);
            }

            FSM.localhost.Add("/home/root/downloads", false, fn.name);
            TM.PrintResponseLine("<color=green>[1958KB]</color> Successfully downloaded file " + fn.name + " to localhost");

        }
        TM.PrintResponseLine("");
        TM.FinishCommand("Voler", 0);
        yield break;
    }
}