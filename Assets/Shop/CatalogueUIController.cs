using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalogueUIController : MonoBehaviour
{
    public GameObject shopItemPrefab;
    private ShopManager SM;

    public GameObject hackToolTabPrefab;
    public GameObject wordlistTabPrefab;
    public GameObject gpuTabPrefab;
    private GameObject activeTab;

    void Awake()
    {
        this.activeTab = hackToolTabPrefab;
    }
    public void RerenderGoods()
    {
        SM = GameObject.FindGameObjectWithTag("ShopManager").GetComponent<ShopManager>();
        foreach (Transform child in this.transform.GetChild(1).transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (var ht in SM.GetHackTools())
        {
            GameObject newHT = Instantiate(shopItemPrefab, this.transform.GetChild(1).transform);
            newHT.name = ht.name;
            newHT.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = ht.name;
            newHT.transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = ht.descripton;
            newHT.transform.Find("Price").GetComponent<TMPro.TextMeshProUGUI>().text = ht.price + " BTC";
        }
    }
}
