using System;
using UnityEngine;

public enum ItemType
{
    none, //��Ÿ������
    Countable, //�Ҹ� ������
    Equipment, // ��� ������
}


[Serializable]
public class InventoryItem
{
    public int ID;
    public int Amount;
    public string Name;
    public string Explanation;
    public Sprite Icon;
    public ItemType ItemType;

    public int SlotIndex = -1;

    public InventoryItem Clone()
    {
        InventoryItem item = new InventoryItem();
        item.ID = ID;
        item.Amount = Amount;
        item.Name = Name;
        item.Explanation = Explanation;
        item.Icon = Icon;
        item.ItemType = ItemType;
        return item;
    }
}
