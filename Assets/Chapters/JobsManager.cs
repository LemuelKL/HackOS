using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobsManager : MonoBehaviour
{
    public GameObject jobCompletedAlertPrefab;

    public void OnJobCompletion(string jobName, int rewardMoney)
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().hacker.completedJobs.Add(jobName);

        GameManager GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        GM.hacker.money += rewardMoney;
        GM.SaveGame();
        GM.LoadGame();

        GameObject.Find("DesktopCanvas").GetComponent<DesktopGUIHandler>().showAlert(jobCompletedAlertPrefab);
    }
}
