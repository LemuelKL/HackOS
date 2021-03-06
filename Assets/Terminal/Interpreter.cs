using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    TerminalManager TM;
    GameManager GM;
    static List<string> hackosCmds = new List<string> { "voler", "bleed", "nmap", "nslookup", "hydra", "ssh", "about" };
    void Awake()
    {
        TM = this.GetComponent<TerminalManager>(); // using `this` ensures the correct TM in multi-terminal scenarios
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public void Interpret(string userInput)
    {
        if (userInput == "cheat")
        {
            Cheat.Execute(TM);
            return;
        }

        Argument args = ParseArguments(userInput);
        string command;
        Hacker hacker = GM.hacker;

        if (!hacker.hackTools.Contains(args.command))
            command = "----";
        else command = args.command;

        bool localSession = TM.sessions.Last().hostname == "hack0s";
        if (!localSession && hackosCmds.Contains(command))
            command = "----";

        switch (command)
        {
            case "voler":
                Voler.Execute(TM, args.flagArgs, args.cmdArgs);
                break;
            case "bleed":
                Bleed.Execute(TM, args.flagArgs, args.cmdArgs);
                break;
            case "nmap":
                Nmap.Execute(TM, args.flagArgs, args.cmdArgs);
                break;
            case "nslookup":
                Nslookup.Execute(TM, args.flagArgs, args.cmdArgs);
                break;
            case "hydra":
                Hydra.Execute(TM, args.flagArgs, args.cmdArgs);
                break;
            case "ssh":
                Ssh.Execute(TM, args.flagArgs, args.cmdArgs);
                break;
            case "exit":
                TerminalCommand.Exit.Execute(TM);
                break;
            case "help":
                TerminalCommand.Help.Execute(TM);
                break;
            case "clear":
                Clear.Execute(TM);
                break;
            case "whoami":
                WhoAmI.Execute(TM);
                break;
            case "ls":
                Ls.Execute(TM);
                break;
            case "pwd":
                Pwd.Execute(TM);
                break;
            case "cd":
                switch (args.cmdArgs.Count())
                {
                    case 0:
                        Cd.Execute(TM);
                        break;
                    case 1:
                        Cd.Execute(TM, args.cmdArgs.ElementAt(0));
                        break;
                    default:
                        break;
                }
                break;
            case "about":
                TM.PrintResponseLine("");
                TM.PrintResponseLine("HackOS");
                TM.PrintResponseLine("an educational sandbox offensive cyber-security game");
                TM.PrintResponseLine("produced by Lemuel Lee");
                TM.PrintResponseLine("30/4/2022");
                TM.PrintResponseLine("");
                TM.FinishCommand();
                break;
            default:
                TM.PrintResponseLine("Command not recognized.");
                TM.FinishCommand();
                break;
        }
    }

    public struct Argument
    {
        public string command;
        public List<string> rawArgs;
        public Dictionary<string, string> flagArgs;
        public List<string> cmdArgs;

        public void Log()
        {
            Debug.Log("CMD: " + command);
            Debug.Log("Flags:");
            foreach (KeyValuePair<string, string> kvp in flagArgs)
            {
                Debug.Log(kvp.Key + " " + kvp.Value);
            }
            Debug.Log("Args:");
            foreach (string cmdArg in cmdArgs)
            {
                Debug.Log(cmdArg);
            }
        }
    }

    public static Argument ParseArguments(string argsStr)
    {
        Argument ret;
        ret.flagArgs = new Dictionary<string, string>();
        ret.cmdArgs = new List<string>();
        List<string> rawArgs = argsStr.Split().ToList<string>();
        ret.command = rawArgs.ElementAtOrDefault(0);
        rawArgs.RemoveAt(0);
        ret.rawArgs = rawArgs;

        // process flags
        for (int i = 0; i < rawArgs.Count(); i++)
        {
            if (rawArgs[i].Contains("--"))
            {
                ret.flagArgs.Add(rawArgs[i], "");
            }
            else if (rawArgs[i].Contains("-"))
            {
                ret.flagArgs.Add(rawArgs[i], rawArgs.ElementAtOrDefault(i + 1));
                // rawArgs.RemoveAt(0);
                // rawArgs.RemoveAt(0);
                // i--;
                i++;
            }
            else
            {
                ret.cmdArgs.Add(rawArgs[i]);
            }
        }
        // ret.Log();
        return ret;
    }
}
