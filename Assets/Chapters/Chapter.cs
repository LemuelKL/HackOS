using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Chapter : MonoBehaviour
{
    public List<string> requiredHackTools;
    public int rewardMoney;
    public int rewardExp;
    public GameObject AnswerForm;
    public abstract void SetupEnvironment();
    public abstract void DeSetupEnvironment();
    public abstract void Play();
    // public abstract bool Complete(); // decided to not do this becoz diff chapter could have very diff completion meachnaisms
    public abstract void Reward();
}
