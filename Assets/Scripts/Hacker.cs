using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Hacker
{
    public double money;
    public int exp;

    public List<string> hackTools;

    public Hacker()
    {
        // this.money = 5;
        // this.exp = 0;
        // this.hackTools = new List<string>(){
        //     "cd","clear","help","ls","nslookup","pwd","ssh","whoami"
        // };
    }
}
