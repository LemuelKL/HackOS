using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Job : MonoBehaviour
{
    public JobsManager JM;
    protected virtual void Awake()
    {
        JM = GameObject.Find("Jobs").GetComponent<JobsManager>();
    }
    public List<string> requiredHackTools;
    public int rewardMoney;
    public GameObject AnswerForm;
    public abstract void SetupEnvironment();
    public abstract void DeSetupEnvironment();
    public abstract void Play();
}
