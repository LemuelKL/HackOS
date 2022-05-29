using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerminalCommand
{
    public class Exit : MonoBehaviour
    {
        public static void Execute(TerminalManager TM)
        {
            TM.PopMachineSession();
            TM.FinishCommand();
        }
    }
}
