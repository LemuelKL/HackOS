using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Nslookup : MonoBehaviour
{
    public static void Execute(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        if (flagArgs.Count != 0)
        {
            TM.PrintResponseLine("unknown flag/s in arugment!");
            TM.FinishCommand();
            return;
        }
        if (cmdArgs.Count != 1)
        {
            TM.PrintResponseLine("nslookup takes exactly one arugment!");
            TM.FinishCommand();
            return;
        }

        string name = cmdArgs[0];
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();

        if (!internet.DNSRecords.ContainsKey(name))
        {
            TM.PrintResponseLine("Answer from deckdns.org:");
            TM.PrintResponseLine("No record of " + name);
            TM.FinishCommand();
            return;
        }

        TM.PrintResponseLine("Answer from deckdns.org:");
        TM.PrintResponseLine("Name:    " + name);
        TM.PrintResponseLine("Address: " + internet.DNSLookUp(name));
        TM.PrintResponseLine("Name:    " + name);
        TM.PrintResponseLine("Address: " + "N/A");
        TM.FinishCommand();
    }
}