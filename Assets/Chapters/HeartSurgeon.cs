using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeartSurgeon : Chapter
{
    public Dictionary<string, string> ans;
    string targetIp = "172.245.143.198";

    void Awake()
    {
        this.requiredHackTools = new List<string>()
        {
            "ssh",
            "bleed",
        };
        this.rewardMoney = 15;
        this.rewardExp = 1500;
        ans = new Dictionary<string, string>();
        ans.Add("root", "asd123");
        ans.Add("alant", "guildford");
        ans.Add("deckard", "ford");
    }

    override public void SetupEnvironment()
    {
        Debug.Log("Setting up environment for HeartSurgeon");

        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();
        Internet.Server targetServer = internet.StartServer(targetIp);
        Internet.Server.Service targetService = new Internet.Server.HTTPS("https", "1.0.0", "1.0.1f");
        targetServer.StartService(443, targetService);
    }

    public override void DeSetupEnvironment()
    {
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();
        internet.KillServer(targetIp);
    }

    override public void Play()
    {
        Debug.Log("Now playing: HeartSurgeon");
    }

    public override bool HasCompleted()
    {
        return false;
    }
    public override void ClaimReward()
    {
        return;
    }
}