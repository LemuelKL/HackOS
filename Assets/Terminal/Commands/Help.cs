using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerminalCommand
{
    public class Help : MonoBehaviour
    {
        public static void Execute(TerminalManager TM)
        {
            TM.PrintResponseLine("HackOS besh, version 0.0.1");
            TM.PrintResponseLine("Type  <command> --help  to display help manual of command.");
            TM.PrintResponseLine("LIST OF COMMANDS");
            TM.PrintResponseLine("   help     - print this manual");
            TM.PrintResponseLine("   ls       - list files and directories");
            TM.PrintResponseLine("   pwd      - print working directory");
            TM.PrintResponseLine("   cd       - change working directory");
            TM.PrintResponseLine("   clear    - clear terminal output");
            TM.PrintResponseLine("   nslookup - IP lookup");
            TM.PrintResponseLine("   ssh      - make a SSH connection");
            TM.PrintResponseLine("   hydra    - password cracker");
            TM.PrintResponseLine("   voler    - steal file from remote machine");
            TM.PrintResponseLine("");
            TM.FinishCommand();
        }
    }
}
