using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameManager GM;
    private List<HackTool> hackTools;

    void Awake()
    {
        hackTools = new List<HackTool>();
    }
    void Start()
    {
        UpdateCatalog();
    }
    void ResetCatelogue()
    {
        hackTools = new List<HackTool>();
        hackTools.Add(new HackTool("Hydra", "hydra", "Login password cracker", 5.0, 0));
        hackTools.Add(new HackTool("Voler", "voler", "File downloader", 7.0, 0));
        hackTools.Add(new HackTool("Nmap", "nmap", "Server scanner", 10.0, 0));
        hackTools.Add(new HackTool("Bleed", "bleed", "Heartbleed attack", 20.0, 0));
    }
    public void UpdateCatalog()
    {
        ResetCatelogue();
        if (hackTools == null) return;
        hackTools.RemoveAll(ht => GM.hacker.hackTools.Contains(ht.commandName));
        GameObject shop = GameObject.FindGameObjectWithTag("Shop");
        if (shop)
            shop.GetComponentInChildren<CatalogueUIController>().RerenderGoods();
    }
    public void BuyHackTool(string name)
    {
        Debug.Log("SM: Got buy request for " + name);
        foreach (var ht in this.hackTools)
        {
            if (ht.name == name)
            {
                if (GM.hacker.hackTools.Contains(ht.commandName)) return;
                if (GM.hacker.money < ht.price) return;
                // commit
                GM.hacker.hackTools.Add(ht.commandName);
                GM.hacker.money -= ht.price;
                GM.SaveGame();
                GM.LoadGame();
            }
        }
    }

    public List<HackTool> GetHackTools()
    {
        return this.hackTools;
    }
}