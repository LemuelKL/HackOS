using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OSManager : MonoBehaviour
{
    [SerializeField]
    public GameObject desktopCanvas;

    public void LaunchAppWindow(GameObject appWindowPrefab)
    {
        if (appWindowPrefab == null) return;
        if (appWindowPrefab.name == "ZignalWindow")
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Zignal");
            if (gos.Length != 0) return;
        };
        GameObject appWindow = Instantiate<GameObject>(appWindowPrefab, new Vector3(0, 0, 0), Quaternion.identity, desktopCanvas.transform);
        appWindow.SetActive(true);
        appWindow.transform.localPosition = new Vector3(0, 0, 0); // relative coord to parent which is the desktop canvas
    }
}
