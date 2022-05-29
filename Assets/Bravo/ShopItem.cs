using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyItemFromShop()
    {
        ShopManager SM = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();
        SM.BuyHackTool(this.name);
    }
}
