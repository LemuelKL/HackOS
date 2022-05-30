using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
public class HeartSurgeon : Chapter
{
    public Dictionary<string, string> answer;
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
        answer = new Dictionary<string, string>();
        answer.Add("root", "asd123");
        answer.Add("staff", "20202024");
        answer.Add("admin", "secure");
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
    public void Complete()
    {
        Reward();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().hacker.completedJobs.Add("Heart Surgeon");
        return;
    }

    public bool CheckAnswer(string p1, string p2, string p3)
    {
        return p1.CompareTo("asd123") == 0 && p2.CompareTo("20202024") == 0 && p3.CompareTo("secure") == 0;
    }

    override public void Reward()
    {
        GameManager GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GM.hacker.money += this.rewardMoney;
        GM.SaveGame();
        GM.LoadGame();
    }
}