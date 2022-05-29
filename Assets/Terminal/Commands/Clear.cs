using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Clear : MonoBehaviour
{
    public static void Execute(TerminalManager TM)
    {
        TM.ClearTerminalOutput();
        TM.FinishCommand();
    }
}