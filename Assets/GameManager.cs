using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Hacker hacker;
    public GameObject moneyDisplay;
    private string savePath;
    private string presistentPath;
    void Awake()
    {
        savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        presistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        hacker = new Hacker();
        activeJob = null;
        activeJobName = null;
        LoadGame();
    }
    public Job activeJob;
    public string activeJobName;
    public CrackAddict crackAddict;
    public HeartSurgeon heartSurgeon;

    public void GoJob(string jobName)
    {
        Job prevJob = activeJob;
        switch (jobName)
        {
            case "Crack Addict":
                activeJob = crackAddict;
                break;
            case "Heart Surgeon":
                activeJob = heartSurgeon;
                break;
            // TODO
            case "Taylor Made":
                activeJob = heartSurgeon;
                break;
            default:
                Debug.LogAssertion("Invalid job name: " + jobName);
                return;
        }

        bool haveHackToolsRequired = true;
        foreach (string requiredhackTool in activeJob.requiredHackTools)
        {
            if (!hacker.hackTools.Contains(requiredhackTool))
            {
                haveHackToolsRequired = false;
                break;
            }
        }
        if (!haveHackToolsRequired)
        {
            // TODO: alert player
            // or maybe not?
            Debug.Log("Does not have required Hack Tools!");
            return;
        }
        if (prevJob != null)
            prevJob.DeSetupEnvironment();
        activeJob.SetupEnvironment();
        activeJob.Play();
        activeJobName = jobName;
    }

    public void LoadGame()
    {
        LoadPlayer();
    }
    public void SaveGame()
    {
        SavePlayer();
    }
    public void SaveGameAndQuit()
    {
        SaveGame();
        Application.Quit();
    }
    private void LoadPlayer()
    {
        Debug.Log("Loading Player");
        using StreamReader reader = new StreamReader(savePath);
        string json = reader.ReadToEnd();

        hacker = JsonUtility.FromJson<Hacker>(json);
        moneyDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("BTC:<color=green>{0}</color>", hacker.money.ToString());
        GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>().UpdateCatalog();
    }
    private void SavePlayer()
    {
        string json = JsonUtility.ToJson(hacker);
        Debug.Log(json);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }
}
