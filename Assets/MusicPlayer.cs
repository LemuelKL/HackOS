using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    void Awake()
    {
        TogglePlayer();
    }
    public void TogglePlayer()
    {
        bool play = this.GetComponent<Toggle>().isOn;
        if (play)
        {
            this.GetComponent<AudioSource>().UnPause();
            this.GetComponent<Image>().color = new Color(0f, 0.85f, 1f, 1f);
        }
        else
        {
            this.GetComponent<AudioSource>().Pause();
            this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
