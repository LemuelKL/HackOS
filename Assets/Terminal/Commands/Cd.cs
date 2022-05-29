using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Cd : MonoBehaviour
{
    static TerminalManager TM = GameObject.FindGameObjectWithTag("Terminal").GetComponent<TerminalManager>();
    static FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();
    public static void Execute(TerminalManager TM)
    {
        TM.FinishCommand();
    }
    public static void Execute(TerminalManager TM, string destPath)
    {
        if (!(destPath.Length >= 1))
        {
            Cd.Execute(TM);
            return;
        }
        // string destPath = args[1];
        string fullPath = "";
        FileSystem.FileNode destFN = null;
        FileSystem.FileNode wdfn;
        if (destPath[0] == '/')
        {
            wdfn = FSM.GetRootNode(TM.sessions.Last().hostname);
            destPath = destPath.Substring(1);
        }
        else
        {
            wdfn = FSM.GetFileNode(TM.sessions.Last().hostname, TM.sessions.Last().workingDirectory);
        }
        bool allValid = true;
        foreach (string pathComponent in destPath.Split('/'))
        {
            Debug.Log("Resolving PC: " + pathComponent);
            if (pathComponent == "")
            {
                // do nth
            }
            else if (pathComponent == "..")
            {
                if (wdfn.name == "")
                {
                    allValid = false;
                    break;
                }
                wdfn = wdfn.parent;
            }
            else
            { // advance
                FileSystem.FileNode childFN = wdfn.GetChild(pathComponent);
                if (childFN == null)
                {
                    allValid = false;
                    break;
                }
                wdfn = childFN;
            }
        }
        // Commit changes if all path components are valid
        if (!allValid)
        {
            TM.PrintResponseLine(destPath + " does not exist!");
            TM.FinishCommand();
            return;
        }
        destFN = wdfn;
        fullPath = destFN.GetPath();
        bool successful = TM.ChangeWorkingDirectory(fullPath, destFN);
        if (successful)
        {
            TM.FinishCommand();
            return;
        }
        TM.PrintResponseLine(destPath + " does not exist!");
        TM.FinishCommand();
        return;
    }
}