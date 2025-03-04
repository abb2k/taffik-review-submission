using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public itemList ItemsList;
    public string choice;
    public int _choiceIndex;
    public float ZLayer;
    public static Transform ItemsCont;
    // Start is called before the first frame update
    void Start()
    {
        if (!ItemsCont)
        {
            GameObject ItemCont = Instantiate(GameManager.getShared().EmptyObj);
            ItemCont.transform.position = Vector3.zero;
            ItemCont.name = "Items";
            ItemsCont = ItemCont.transform;
        }

        GameObject item = Instantiate(ItemsList.Items[_choiceIndex], ItemsCont);
        item.transform.position = new Vector3(transform.position.x, transform.position.y, ZLayer);
        Destroy(gameObject);
    }
}
