using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public itemList itemList;

    public int Items;

    public int[] DropChances;
    public string[] items;
    public int[] SelectedItems;

    public void RollItemDrop()
    {
        int roll = Random.Range(0, 100);

        for (int i = 0; i < DropChances.Length; i++)
        {
            if (DropChances[i] > roll)
            {
                GameObject item = null;

                foreach (GameObject ite in itemList.Items)
                {
                    if (ite.name == items[i])
                    {
                        item = ite;
                    }
                }
                GameObject itemObj = null;
                if (item != null) itemObj = Instantiate(item);
                if (itemObj != null)
                {
                    var pos = itemObj.transform.position;
                    pos.x = transform.position.x;
                    pos.y = transform.position.y;
                    itemObj.transform.position = pos;
                }
            }
        }
    }
}
