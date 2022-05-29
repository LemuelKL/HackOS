using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Nmap : MonoBehaviour
{
    public static Nmap instance;
    void Awake()
    {
        instance = this;
    }
    public static void Execute(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        if (flagArgs.Count != 0)
        {
            TM.PrintResponseLine("Unknown flag/s in arugment!");
            TM.FinishCommand("Nmap", 1);
            return;
        }
        if (cmdArgs.Count != 1)
        {
            TM.PrintResponseLine("Nmap takes exactly one arugment!");
            TM.FinishCommand("Nmap", 2);
            return;
        }

        instance.StartCoroutine(instance.Hack(TM, flagArgs, cmdArgs));
    }

    public IEnumerator Hack(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        string ip = cmdArgs[0];
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();

        Internet.Server targetServer = internet.GetServer(ip);
        TM.PrintResponseLine("");
        yield return new WaitForSeconds(1);
        TM.PrintResponseLine("Starting Nmap 1.0.0");
        yield return new WaitForSeconds(1);
        if (targetServer == null)
        {
            TM.PrintResponseLine("<color=red>[ERROR]</color> no respsone from server " + ip);
            TM.FinishCommand("Nmap", 0);
            yield break;
        }

        Dictionary<int, Internet.Server.Service> targetServices = targetServer.GetServices();

        string spinners = "◢◣◤◥";
        Text scanningLine = TM.PrintResponseLine("");
        for (int i = 0; i < 7; i++)
        {
            scanningLine.text = spinners[i % 4].ToString() + " scanning for services...";
            yield return new WaitForSeconds(1);
        }

        if (targetServices.Count == 0)
        {
            scanningLine.text = "Nmap cannot find any running service!";
        }
        else
        {
            scanningLine.text = "PORT\tSTATE\tSERVICE\tVERSION";
            foreach (var ts in targetServices)
            {
                int port = ts.Key;
                Internet.Server.Service service = ts.Value;
                TM.PrintResponseLine(string.Format("{0}/tcp\t{1}\t{2}\t\t{3}", port, "open", service.name, service.version));
            }
        }
        TM.PrintResponseLine("Device type: general purpose");
        TM.PrintResponseLine("Running Linux 1.6.X");
        TM.PrintResponseLine("OS details: 1.6.0 - 1.6.11");
        TM.PrintResponseLine("Uptime: unknown");
        TM.PrintResponseLine("");
        TM.FinishCommand("Nmap", 0);
        yield break;
    }
}