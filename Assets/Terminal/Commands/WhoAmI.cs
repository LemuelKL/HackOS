using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class WhoAmI : MonoBehaviour
{
    public static void Execute(TerminalManager TM)
    {
        TM.PrintResponseLine(TM.sessions.Last().user);
        TM.FinishCommand();
    }
}