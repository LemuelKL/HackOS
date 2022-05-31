using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    void Update()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("BTC:<color=green>{0}</color>", GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().hacker.money.ToString());
    }
}
