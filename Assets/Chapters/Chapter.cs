using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Chapter : MonoBehaviour
{
    public List<string> requiredHackTools;
    public int rewardMoney;
    public int rewardExp;

    public abstract void SetupEnvironment();
    public abstract void DeSetupEnvironment();
    public abstract void Play();
    public abstract bool HasCompleted();
    public abstract void ClaimReward();
}
