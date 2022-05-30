using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSurgeonAnswerHandler : MonoBehaviour
{
    HeartSurgeon HS;
    ZignalController ZC;
    TMPro.TMP_Text msg;
    Button sendBtn;

    void Awake()
    {
        GameObject Chapters = GameObject.FindGameObjectWithTag("Chapters");
        HS = Chapters.GetComponentInChildren<HeartSurgeon>();
        ZC = GameObject.FindGameObjectWithTag("Zignal").GetComponent<ZignalController>();
        msg = transform.GetChild(2).GetComponentInChildren<TMPro.TMP_Text>();
        sendBtn = transform.GetChild(3).GetComponentInChildren<Button>();
    }
    void Update()
    {
        Transform form = transform.Find("InputArea").Find("Form");
        foreach (Transform field in form.transform)
        {
            if (field.GetComponent<TMPro.TMP_InputField>().text.Length == 0)
            {
                sendBtn.interactable = false;
                return;
            }
        }
        sendBtn.interactable = true;
    }

    private string GetFormValue(string fieldName)
    {
        Transform form = transform.Find("InputArea").Find("Form");
        return form.Find(fieldName).GetComponent<TMPro.TMP_InputField>().text;
    }
    public void SubmitAnswer()
    {
        bool correct = HS.CheckAnswer(GetFormValue("pwd1"), GetFormValue("pwd2"), GetFormValue("pwd3"));
        if (correct)
        {
            HS.Complete();
            CloseForm();
            return;
        }
        StartCoroutine(DisplayMessage("Incorrect!"));
    }

    private IEnumerator DisplayMessage(string m)
    {
        msg.text = m;
        yield return new WaitForSeconds(2);
        msg.text = "";
    }

    public void CloseForm()
    {
        ZC.ToggleAnswerForm(false);
    }
}
