using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZignalController : MonoBehaviour
{
    public static string chatter;
    public GameObject chatLinePrefab;
    public GameObject contactList;
    public GameObject contactPrefab;
    public Sprite crack_addict;
    public Sprite heart_surgeon;
    public Sprite taylor_made;
    public Sprite stasi;
    public Sprite bombshell;
    public Sprite human_sea;
    public Sprite old_fashioned;

    private List<string> mainUIs;
    GameManager GM;
    private Button doJobBtn;
    private Button replyBtn;

    void Start()
    {
        List<Contact> contacts = new List<Contact>();
        contacts.Add(new Contact { name = "Crack Addict", avatar = crack_addict });
        contacts.Add(new Contact { name = "Heart Surgeon", avatar = heart_surgeon });
        contacts.Add(new Contact { name = "Taylor Made", avatar = taylor_made });
        // contacts.Add(new Contact { name = "Stasi", avatar = stasi });
        // contacts.Add(new Contact { name = "Bombshell", avatar = bombshell });
        // contacts.Add(new Contact { name = "Human Sea", avatar = human_sea });
        // contacts.Add(new Contact { name = "Old-fashioned", avatar = old_fashioned });
        foreach (Contact contact in contacts)
        {
            GameObject newContact = Instantiate(contactPrefab, contactList.transform);
            newContact.GetComponent<ContactUIController>().ChangeName(contact.name);
            newContact.GetComponent<ContactUIController>().ChangeAvatar(contact.avatar);
            // Need to set Toggle Group programmatically coz Unity would not respect what's set in the Inspector. Might be a Unity bug actually.
            newContact.GetComponent<Toggle>().group = this.contactList.GetComponent<ToggleGroup>();
        }
        mainUIs = new List<string>();
        mainUIs.Add("ContactScroll");
        mainUIs.Add("ChatScroll");
        mainUIs.Add("ActionArea");
    }

    void Awake()
    {
        Transform actionArea = GameObject.FindGameObjectWithTag("Zignal").transform.Find("ActionArea").transform;
        doJobBtn = actionArea.Find("BeginBtn").GetComponent<Button>();
        replyBtn = actionArea.Find("ReplyBtn").GetComponent<Button>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        List<string> completedChatters = GM.hacker.completedJobs;
        if (completedChatters.Contains(chatter))
        {
            doJobBtn.interactable = false;
            replyBtn.interactable = false;
            doJobBtn.GetComponentInChildren<TMPro.TMP_Text>().text = "Completed";
            replyBtn.GetComponentInChildren<TMPro.TMP_Text>().text = "Replied";
        }
        else
        {
            if (chatter == GM.activeChapterName)
            {
                doJobBtn.interactable = false;
                doJobBtn.GetComponentInChildren<TMPro.TMP_Text>().text = "In Progress";
                replyBtn.interactable = true;
            }
            else
            {
                doJobBtn.interactable = true;
                doJobBtn.GetComponentInChildren<TMPro.TMP_Text>().text = "Do Job";
                replyBtn.interactable = false;
            }
            replyBtn.GetComponentInChildren<TMPro.TMP_Text>().text = "Reply";
        }
    }

    private class chatRecord
    {
        public string sender;
        public string msg;
        public chatRecord(string sender, string msg)
        {
            this.sender = sender;
            this.msg = msg;
        }
    }
    private Dictionary<string, List<chatRecord>> chatHistory = new Dictionary<string, List<chatRecord>>()
    {
        {"Crack Addict", new List<chatRecord>(){
            new chatRecord("CA", "Hey, need ur help..."),
            new chatRecord("Me", "Wts up"),
            new chatRecord("Me", "I dun work for free tho"),
            new chatRecord("CA", "right, get me a pdf "),
            new chatRecord("CA", "final exam of COMP3329"),
            new chatRecord("CA", "on hcku.hk"),
            new chatRecord("Me", "that'll be 15 BTC"),
            new chatRecord("CA", "right."),
        }},
        {"Heart Surgeon", new List<chatRecord>(){
            new chatRecord("HS", "Yo, heard u short on $"),
            new chatRecord("Me", "yea..."),
            new chatRecord("HS", "Get me 3 accounts of this site:"),
            new chatRecord("HS", "172.245.143.198"),
            new chatRecord("HS", "I need `root`, `staff` and `admin`"),
            new chatRecord("Me", "ok"),
            new chatRecord("Me", "that'll be 15 BTC"),
            new chatRecord("CA", "right."),
        }},
        {"Taylor Made", new List<chatRecord>(){
            new chatRecord("TM", "I really like this girl..."),
            new chatRecord("TM", "Can u get her FB acc?"),
            new chatRecord("Me", "Sure, her name?"),
            new chatRecord("TM", "Taylor"),
        }}

    };

    private void ChangeChat(string chatter)
    {
        List<chatRecord> chatRecords;
        chatHistory.TryGetValue(chatter, out chatRecords);
        Transform chatListTransform = GameObject.Find("ChatList").transform;
        // remove old chat UIs
        foreach (Transform child in chatListTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
        // populate the new ones
        foreach (var chatRecord in chatRecords)
        {
            GameObject chatLine = Instantiate(chatLinePrefab, chatListTransform);
            chatLine.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = chatRecord.msg;
            if (chatRecord.sender == "Me")
                chatLine.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleRight;
            else
                chatLine.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
        }
        // need this
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatListTransform as RectTransform);
    }
    public void HandleChatSelectionChange(Toggle changedContact)
    {
        if (changedContact.isOn == true)
        {
            chatter = changedContact.GetComponentInChildren<Text>().text;
            this.ChangeChat(chatter);
        }
    }

    public void DoJob()
    {
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        // could be null if just started game
        if (GM.activeChapter != null)
            PromptSwitchJob();
        else
            ConfirmSwitchJob();
    }

    private void PromptSwitchJob()
    {
        ToggleConfirmScreen(true);
    }

    public void ConfirmSwitchJob()
    {
        ToggleConfirmScreen(false);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GoChapter(chatter);
    }

    public void ToggleConfirmScreen(bool togg)
    {
        Transform zignal = GameObject.FindGameObjectWithTag("Zignal").transform;
        foreach (Transform child in zignal)
        {
            if (child.name == "ConfirmScreen")
            {
                child.gameObject.SetActive(togg);
            }
            if (mainUIs.Contains(child.name))
            {
                child.gameObject.SetActive(!togg);
            }
        }
    }

    public void ToggleAnswerForm(bool togg)
    {
        Transform zignal = GameObject.FindGameObjectWithTag("Zignal").transform;
        foreach (Transform child in zignal)
        {
            if (child.name == "AnswerFormArea")
            {
                child.gameObject.SetActive(togg);
            }
            if (mainUIs.Contains(child.name))
            {
                child.gameObject.SetActive(!togg);
            }
        }
    }

    public void DoReply()
    {
        Chapter activeChapter = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().activeChapter;
        if (activeChapter == null)
            return;
        GameObject answerFormPrefab = activeChapter.AnswerForm;
        ToggleAnswerForm(true);
        GameObject answerFormArea = GameObject.Find("AnswerFormArea");
        Instantiate(answerFormPrefab, answerFormArea.transform);
    }
}
