using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cheat : MonoBehaviour
{
    public static void Execute(TerminalManager TM)
    {
        TM.PrintResponseLine("----[ HackOS Cheat Menu ]----");
        TM.PrintResponseLine(" [ 1 ] ++money");
        TM.PrintResponseLine(" [ 2 ] --money");
        TM.PrintResponseLine(" [ 3 ] reset Jobs progress");
        TM.PrintResponseLine(" [ 0 ] Exit");
        InputField prompt = TM.PrintInputPrompt("Enter code: ");
        prompt.enabled = true;
        prompt.ActivateInputField();
        prompt.interactable = true;
        prompt.Select();
        prompt.onSubmit.RemoveAllListeners();
        prompt.onSubmit.AddListener((code) =>
        {
            GameManager GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            switch (code)
            {
                case "1":
                    GM.hacker.money += 100;
                    break;
                case "2":
                    GM.hacker.money -= 100;
                    break;
                case "3":
                    GM.hacker.completedJobs = new List<string>();
                    GM.SaveGame();
                    GM.LoadGame();
                    break;
                case "0":
                    prompt.interactable = false;
                    TM.FinishCommand();
                    return;
                default:
                    break;
            }
        });

    }
}