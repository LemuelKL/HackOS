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
        activeChapter = null;
        activeChapterName = null;
        LoadGame();
    }
    public Chapter activeChapter;
    public string activeChapterName;
    public CrackAddict crackAddict;
    public HeartSurgeon heartSurgeon;

    public void GoChapter(string chapterName)
    {
        Chapter prevChapter = activeChapter;
        switch (chapterName)
        {
            case "Crack Addict":
                activeChapter = crackAddict;
                break;
            case "Heart Surgeon":
                activeChapter = heartSurgeon;
                break;
            // TODO
            case "Taylor Made":
                activeChapter = heartSurgeon;
                break;
            default:
                Debug.LogAssertion("Invalid chapter name.");
                return;
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
        if (prevChapter != null)
            prevChapter.DeSetupEnvironment();
        activeChapter.SetupEnvironment();
        activeChapter.Play();
        activeChapterName = chapterName;
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
