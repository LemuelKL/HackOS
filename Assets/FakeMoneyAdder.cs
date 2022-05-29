using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMoneyAdder : MonoBehaviour
{
    // Start is called before the first frame update
    int money;
    void Start()
    {
        money = 200;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney() {
        this.transform.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("BTC:<color=green>{0}</color>", money);
        money++;
    }
}
