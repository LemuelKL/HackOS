using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Pwd : MonoBehaviour
{
    public static void Execute(TerminalManager TM)
    {
        if (TM.IsAtRoot())
            TM.PrintResponseLine("/");
        else
            TM.PrintResponseLine(TM.sessions.Last().workingDirectory);
        TM.FinishCommand();
    }
}