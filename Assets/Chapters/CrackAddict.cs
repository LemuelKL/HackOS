using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CrackAddict : Chapter
{
    string targetIp = "147.8.2.58";
    string targetDomain = "hcku.hk";
    void Awake()
    {
        this.requiredHackTools = new List<string>()
        {
            "ssh",
            "hydra",
            "voler"
        };
        this.rewardMoney = 15;
        this.rewardExp = 1500;
    }

    override public void SetupEnvironment()
    {
        Debug.Log("Setting up environment for CrackAddict");

        // TODO: randomize targetIp

        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();
        Internet.Server targetServer = internet.StartServer(targetIp);
        Internet.Server.Service targetService = new Internet.Server.SSH("ssh", "1.0.0");
        targetServer.StartService(22, targetService);

        internet.RegisterDomain(targetIp, targetDomain);

        FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();

        FileSystem fs = new FileSystem();
        fs.NewFolder("home", "");
        fs.NewFolder("root", "/home");
        fs.NewFolder("comp1853", "/home/root");
        fs.NewFile("midterm.tex", "/home/root/comp1853");
        fs.NewFolder("comp2120", "/home/root");
        fs.NewFile("assignment1_ans.pdf", "/home/root/comp2120");
        fs.NewFolder("comp3322", "/home/root");
        fs.NewFile("exam.pdf", "/home/root/comp3322");
        fs.NewFolder("comp3329", "/home/root");
        fs.NewFile("exam.pdf", "/home/root/comp3329");
        fs.NewFile("deadlines.xlsx", "/home/root/comp3329");
        FSM.AddFileSystem(targetIp, fs);
    }

    public override void DeSetupEnvironment()
    {
        Internet internet = GameObject.FindGameObjectWithTag("Internet").GetComponent<Internet>();
        internet.KillServer(targetIp);
        internet.RemoveDomain(targetDomain);
        FileSystemManager FSM = GameObject.FindGameObjectWithTag("FileSystemManager").GetComponent<FileSystemManager>();
        FSM.RemoveFileSystem(targetIp);
    }

    override public void Play()
    {
        Debug.Log("Now playing: CrackAddct");
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