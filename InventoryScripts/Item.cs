using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Coin,
        HealthItem,
        ManaItem,
    }

    public ItemType itemType;
    public int amount;
    public string desc;
    public string itemName;
    public string itemGrade;
    public int itemID;
}
