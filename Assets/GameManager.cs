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
        LoadGame();
    }
    public Chapter activeChapter;
    public CrackAddict crackAddict;
    public HeartSurgeon heartSurgeon;

    public void GoChapter(string chapterName)
    {
        switch (chapterName)
        {
            case "Crack Addict":
                activeChapter = crackAddict;
                break;
            case "Heart Surgeon":
                activeChapter = heartSurgeon;
                break;
            default:
                break;
        }

        bool haveHackToolsRequired = true;
        foreach (string requiredhackTool in activeChapter.requiredHackTools)
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
            Debug.Log("Does not have required Hack Tools!");
            return;
        }
        activeChapter.SetupEnvironment();
        activeChapter.Play();
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

    public void CompleteChapter(string chapterName) {
        FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();
        switch(chapterName) {
            case "Crack Addict":
                if (FSM.GetFileNode("localhost", "/home/root/downloads/exam.pdf") != null)
                {
                    hacker.money += 100;
                    SavePlayer();
                    LoadPlayer();
                }
                break;
            default:
                break;
        }
    }
}
