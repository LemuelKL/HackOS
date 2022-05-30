using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DesktopGUIHandler : MonoBehaviour
{
    public GameObject canvas;
    GraphicRaycaster raycaster;
    PointerEventData clickData;
    List<RaycastResult> clickResults;


    // Start is called before the first frame update
    void Start()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            HandleUIElementsClicked();
        }
    }

    void HandleUIElementsClicked()
    {
        clickData.position = Mouse.current.position.ReadValue();
        clickResults.Clear();
        raycaster.Raycast(clickData, clickResults);

        foreach (RaycastResult result in clickResults)
        {
            GameObject element = result.gameObject;
            GameObject appWindow;
            GameObject app;
            // Debug.Log(element.name);

            appWindow = FindParentWithTag(element, "Window");

            if (appWindow == null)
                return;

            app = appWindow.transform.Find("Content").GetChild(0).gameObject;

            switch (app.name)
            {
                case "Terminal":
                    // Debug.Log("Focusing to Terminal");
                    InputField inputField = app.transform.Find("Line").Find("InputField").GetComponent<InputField>();
                    inputField.ActivateInputField();
                    inputField.Select();
                    break;
                default:
                    break;
            }
        }
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    public void showAlert(GameObject alertPrefab)
    {
        StartCoroutine(spawnAlert(alertPrefab));
    }

    private IEnumerator spawnAlert(GameObject alertPrefab)
    {
        Transform canvas = GameObject.Find("DesktopCanvas").transform;
        GameObject alert = Instantiate(alertPrefab, canvas.transform);
        alert.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
        yield return new WaitForSeconds(3);
        Destroy(alert);
    }
}
