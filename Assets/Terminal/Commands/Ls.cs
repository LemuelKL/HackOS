using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Ls : MonoBehaviour
{
    public static void Execute(TerminalManager TM)
    {
        foreach (string fileRecord in TM.sessions.Last().workingDirectoryFileNode.GetChildrenNames())
        {
            TM.PrintResponseLine(fileRecord);
        }
        TM.FinishCommand();
    }
}