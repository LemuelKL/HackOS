using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZignalController : MonoBehaviour
{
    public GameObject chatList;
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

    // public GameObject crackAddictAnswerPrefab;
    // public GameObject heartSurgeonAnswerPrefab;
    // public GameObject taylorMadeAnswerPrefab;

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

        // Debug.Log(crackAddictAnswerPrefab);
        // Debug.Log(heartSurgeonAnswerPrefab);
        // Debug.Log(taylorMadeAnswerPrefab);
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

    /// <summary>
    /// Pre-existing chat history for each game chapter
    /// </summary>
    private Dictionary<string, List<chatRecord>> chatHistory = new Dictionary<string, List<chatRecord>>()
    {
        {"Crack Addict", new List<chatRecord>(){
            new chatRecord("CA", "Hey, need ur help..."),
            new chatRecord("Me", "Wts up"),
            new chatRecord("Me", "I dun work for free tho"),
            new chatRecord("CA", "right, get me a pdf "),
            new chatRecord("CA", "final exam of COMP3329"),
            new chatRecord("CA", "on hcku.hk"),
        }},
        {"Heart Surgeon", new List<chatRecord>(){
            new chatRecord("HS", "Yo, heard u short on $"),
            new chatRecord("Me", "yea..."),
            new chatRecord("HS", "Get me 3 accounts of this site:"),
            new chatRecord("HS", "172.245.143.198"),
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
                chatLine.GetComponent<HorizonclLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
        }

        // Transform zignal = GameObject.FindGameObjectWithTag("Zignal").transform;

        // foreach (Transform child in zignal)
        // {
        //     if (child.name.Contains("(Clone)"))
        //         Destroy(child.gameObject);
        // }
        // GameObject answerArea = null;
        // if (chatter == "Crack Addict")
        // {
        //     answerArea = Instantiate(this.crackAddictAnswerPrefab, zignal);
        // }
        // if (chatter == "Heart Surgeon")
        // {
        //     Debug.Log(heartSurgeonAnswerPrefab);
        //     answerArea = Instantiate(this.heartSurgeonAnswerPrefab, zignal);
        // }
        // if (chatter == "Taylor Made")
        // {
        //     answerArea = Instantiate(this.taylorMadeAnswerPrefab, zignal);
        // }
        // if (answerArea != null)
        // {
        //     answerArea.transform.SetAsLastSibling();
        // }

        // need this
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatListTransform as RectTransform);
    }
    public void HandleChatSelectionChange(Toggle changedContact)
    {
        if (changedContact.isOn == true)
        {
            string chatter = changedContact.GetComponentInChildren<Text>().text;
            this.ChangeChat(chatter);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GoChapter(chatter);
        }
    }
}
