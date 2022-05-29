using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DynamicInputHandler : MonoBehaviour
{

    TerminalManager TM;
    InputField inputField;
    void Awake()
    {
        TM = DesktopGUIHandler.FindParentWithTag(this.gameObject, "Terminal").GetComponent<TerminalManager>();
        inputField = transform.GetComponent<InputField>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // TODO: kinda repeats that in Terminal Manager
    // fix later...
    void OnGUI()
    {
        RectTransform promptRT = (RectTransform)this.transform.parent.GetChild(0).transform;
        RectTransform terminalRT = this.GetComponent<RectTransform>();
        terminalRT.anchoredPosition = new Vector2(promptRT.rect.width, promptRT.anchoredPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputField.isActiveAndEnabled)
            return;
        if (!inputField.isFocused)
            return;
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            inputField.text = TM.QPreviousCommand();
        }
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            inputField.text = TM.QNextCommand();
        }
    }
}
