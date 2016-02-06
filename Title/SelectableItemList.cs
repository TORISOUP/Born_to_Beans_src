using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectableItemList : MonoBehaviour
{
    public int id;
    public List<SelectableItem> selectableItems = new List<SelectableItem>();

    // Use this for initialization
    public void Initialize () {
        for (int i = 0; i < selectableItems.Count; i++)
        {
            selectableItems[i].id = i;
        }
    }
}
