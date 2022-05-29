using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContactUIController : MonoBehaviour
{
    public void ChangeName(string newName)
    {
        Text name = this.gameObject.GetComponentInChildren<Text>();
        name.text = newName;
    }

    public void ChangeAvatar(Sprite newAvatar)
    {
        Image avatar = this.gameObject.transform.Find("Avatar").gameObject.GetComponent<Image>();
        avatar.sprite = newAvatar;
    }

    public void InformParent() {
        
    }
}
