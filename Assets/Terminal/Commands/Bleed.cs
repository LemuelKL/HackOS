using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class Bleed : MonoBehaviour
{
    public static Bleed instance;
    void Awake()
    {
        instance = this;
    }
    public static void Execute(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        if (flagArgs.Count != 0)
        {
            TM.PrintResponseLine("Unknown flag/s in arugment!");
            TM.FinishCommand("Bleed", 1);
            return;
        }
        if (cmdArgs.Count != 1)
        {
            TM.PrintResponseLine("Bleed takes exactly one arugment!");
            TM.FinishCommand("Bleed", 2);
            return;
        }

        instance.StartCoroutine(instance.Hack(TM, flagArgs, cmdArgs));
    }

    private static string RandomString(int length)
    {
        const string pool = "...................................................................................................................................................................................................................................................................................abcdefghijklmnopqrstuvwsyzABCDEFGHIJKLMNOPQRSTUVWSYZ0123456789)!@#$%^&*(";
        var chars = Enumerable.Range(0, length)
            .Select(x => pool[Random.Range(0, pool.Length)]);
        return new string(chars.ToArray());
    }

    public IEnumerator Hack(TerminalManager TM, Dictionary<string, string> flagArgs, List<string> cmdArgs)
    {
        string ip = cmdArgs[0];
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();

        Internet.Server targetServer = internet.GetServer(ip);
        TM.PrintResponseLine("");
        yield return new WaitForSeconds(1);
        TM.PrintResponseLine("Starting Bleed");
        TM.PrintResponseLine("Connecting...");
        yield return new WaitForSeconds(1);
        if (targetServer == null)
        {
            TM.PrintResponseLine("<color=red>[ERROR]</color> no respsone from server " + ip);
            TM.FinishCommand("Bleed", 0);
            yield break;
        }

        bool hasHttps = targetServer.HasService("https");
        if (!hasHttps)
        {
            TM.PrintResponseLine("<color=red>[ERROR]</color> cannot find any HTTPS services!");
            TM.FinishCommand("Bleed", 0);
            yield break;
        }
        else
        {
            TM.PrintResponseLine("Sending Client Hello...");
            yield return new WaitForSeconds(0.5f);
            TM.PrintResponseLine("Waiting for Server Hello...");
            yield return new WaitForSeconds(0.5f);
            TM.PrintResponseLine("Received Server Hello...");
            TM.PrintResponseLine("Sending heartbeat request...");
            yield return new WaitForSeconds(1f);
            TM.PrintResponseLine("...received message: type = 24, ver = 0302, length = " + ((uint)Random.Range(14000, 21000)));

            List<Text> texts = new List<Text>();
            for (int i = 0; i < 24; i++)
            {
                texts.Add(TM.PrintResponseLine(RandomString(80)));
            }

            InputField contInput;
            Beat(texts);
            contInput = TM.PrintInputPrompt("Beat more [y/N]? ");


            contInput.enabled = true;
            contInput.ActivateInputField();
            contInput.interactable = true;
            contInput.Select();
            contInput.onSubmit.RemoveAllListeners();
            contInput.onSubmit.AddListener((contText) =>
            {
                bool doBeat = true;
                switch (contText)
                {
                    case "":
                        break;
                    case "Y":
                        break;
                    case "y":
                        break;
                    case "N":
                        doBeat = false;
                        break;
                    case "n":
                        doBeat = false;
                        break;
                    default:
                        doBeat = false;
                        break;
                }

                Destroy(contInput.GetComponent<DynamicInputHandler>());
                contInput.textComponent.color = Color.white;
                contInput.interactable = false;

                if (!doBeat)
                {
                    TM.FinishCommand("Bleed", 0);
                    return;
                }

                Beat(texts);
                contInput.enabled = true;
                contInput.ActivateInputField();
                contInput.interactable = true;
                contInput.Select();
            });
        }
        yield break;
    }


    // texts is the workable UI Text s
    public void Beat(List<Text> texts)
    {
        KeyValuePair<string, string> credCombo;
        string answer = "";
        GameManager GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (GM.activeChapter.name == "HeartSurgeon")
        {
            HeartSurgeon hs = GameObject.FindGameObjectWithTag("Chapters").transform.Find("HeartSurgeon").GetComponent<HeartSurgeon>();
            credCombo = hs.answer.ElementAt(Random.Range(0, hs.answer.Count - 1));
            answer = string.Format("{0}::{1}", credCombo.Key, credCombo.Value);
        }
        int l = answer.Length;
        int pos = Random.Range(0, 80 - l);

        for (int i = 0; i < 24; i++)
        {
            texts[i].text = RandomString(80);
        }
        texts[Random.Range(0, 23)].text = RandomString(pos) + answer + RandomString(80 - l - pos);
    }
}