using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrackAddictAnswerHandler : MonoBehaviour
{
    CrackAddict CA;
    ZignalController ZC;
    TMPro.TMP_Text msg;
    Button sendBtn;
    bool completed;
    void Awake()
    {
        GameObject Chapters = GameObject.FindGameObjectWithTag("Chapters");
        CA = Chapters.GetComponentInChildren<CrackAddict>();
        ZC = GameObject.FindGameObjectWithTag("Zignal").GetComponent<ZignalController>();
        msg = transform.GetChild(2).GetComponentInChildren<TMPro.TMP_Text>();
        sendBtn = transform.GetChild(3).GetComponentInChildren<Button>();
        completed = false;
    }
    void Update()
    {
        completed = CA.CanComplete();
        sendBtn.interactable = completed;
        if (!completed)
            msg.text = "File does not exist!";
        else
            msg.text = "";
    }
    public void SubmitAnswer()
    {
        Debug.Log("Submitting answer for Crack Addict.");
        if (completed)
        {
            CA.Complete();
            CloseForm();
            return;
        }
        Debug.LogAssertion("Branch should never be reached.");
    }

    public void CloseForm()
    {
        ZC.ToggleAnswerForm(false);
    }
}
