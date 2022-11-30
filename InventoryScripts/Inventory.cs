using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;
    private Item itemToAdd;

    public Inventory()
    {
        itemList = new List<Item>();
    }
    public void FindItem(int id, int _amount)
    {
        switch (id)
        {
            case 100:
                itemToAdd = new Item { itemType = Item.ItemType.HealthItem, amount = _amount, itemID = id };
                AddItem(itemToAdd);
                break;
            case 101:
                itemToAdd = new Item { itemType = Item.ItemType.ManaItem, amount = _amount, itemID = id };
                AddItem(itemToAdd);
                break;
            case 102:
                itemToAdd = new Item { itemType = Item.ItemType.ManaItem, amount = _amount, itemID = id };
                AddItem(itemToAdd);
                break;
        }
    }

    public void AddItem(Item itemToAdd)
    {
        if (itemList.Count == 0)
        {
            itemList.Add(itemToAdd);
            Debug.Log("Added a new item to the list due to it being empty");
        }
        else
        {
            int i = 0;
            foreach (Item item in itemList)
            {
                i++;
                if (item.itemID == itemToAdd.itemID)
                {
                    item.amount++;
                    Debug.Log("Increased an existing items amount");
                    break;
                }
                else if (i >= itemList.Count)
                {
                    itemList.Add(itemToAdd);
                    Debug.Log("Item not found. New item added to the inventory");
                    break;
                }
                else
                {
                    Debug.Log("Error: Inventory's AddItem");
                }
            }
        }
    }

    public Item GetItemInfo(int item)
    {
        switch (item)
        {
            case 100:
                return new Item { itemName = "Health potion", desc = "A potion used to regain lost health", itemGrade = "Medium" };
            case 101:
                return new Item { itemName = "Mana potion", desc = "A potion used to regain lost mana", itemGrade = "High-tier" };
            default:
                return new Item { itemName = "error", desc = "error" };
        }
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}