using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Hydra : MonoBehaviour
{
    public static List<string> manual = new List<string>() {
        "EXAMPLE USAGE",
        "    [*] Guess SSH credentials using a given username and a list of passwords:",
        "        hydra -l username -P path/to/wordlist.txt host_ip ssh"
    };
    public static Hydra instance;
    void Awake()
    {
        instance = this;
    }
    public static void Execute(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        if (flagArgs.ContainsKey("--help"))
        {
            TM.PrintResponseLines(manual);
            TM.FinishCommand();
            return;
        }

        bool validArgs = true;
        if (!flagArgs.ContainsKey("-l") || !flagArgs.ContainsKey("-P"))
            validArgs = false;
        if (flagArgs.GetValueOrDefault("-l") == null || flagArgs.GetValueOrDefault("-P") == null)
            validArgs = false;
        if (cmdArgs.Count != 2)
            validArgs = false;
        // if (cmdArgs.ElementAtOrDefault(1) != "ssh")
        //     validArgs = false;

        if (!validArgs)
        {
            TM.PrintResponseLine("unknown arguments...");
            TM.FinishCommand();
            return;
        }
        FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();
        if (flagArgs["-P"] != "wordlists/rockyou.txt")
        {
            TM.PrintResponseLine("<color=red>[ERROR]</color> could not locate " + flagArgs["-P"]);
            TM.FinishCommand("Hydra", 1);
            return;
        }

        instance.StartCoroutine(instance.Hack(TM, flagArgs, cmdArgs));
    }

    public IEnumerator Hack(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        string ip = cmdArgs[0];
        string serviceName = cmdArgs[1];
        TM.PrintResponseLine("Hydra starting at " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        yield return new WaitForSeconds(1);

        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();

        // check if server exists on Internet
        if (!internet.PingServer(ip))
        {
            TM.PrintResponseLine(string.Format("[ERROR] failed to establish connection to {0}", ip));
            TM.PrintResponseLine("Exiting...");
            TM.FinishCommand();
            yield break;
        }

        if (!internet.GetServer(ip).HasService(cmdArgs.ElementAt(1)))
        {
            TM.PrintResponseLine(string.Format("[ERROR] {0} has no protocol/service {1}", ip, cmdArgs.ElementAt(1)));
            TM.PrintResponseLine("Exiting...");
            TM.FinishCommand();
            yield break;
        }

        TM.PrintResponseLine(string.Format("[DATA] attacking ssh://{0}:22/", cmdArgs[0]));
        yield return new WaitForSeconds(1);

        Text progressBar = TM.PrintResponseLine("[>...................]");
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.1f);
            progressBar.text = string.Format("[{0}>{1}]", new string('=', i), new string('.', 19 - i));
        }

        string login = flagArgs["-l"];
        string password = "toor";
        TM.PrintResponseLine(string.Format("[<color=lime>22</color>][<color=lime>ssh</color>] host: {0}    login: <color=lime>{1}</color>    password: <color=lime>{2}</color>", cmdArgs[0], login, password));

        TM.PrintResponseLine("Attack successffuly completed, 1 valid password found");
        TM.PrintResponseLine("Hydra finished at " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        TM.FinishCommand();
    }
}


