using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonHandler<ItemManager>
{
    public ItemDatabase ItemDatabase;

    public Item GetItemByID(int ID) 
    { 
        if(Array.Find(ItemDatabase.Items, x => x.ID == ID) != null) //���� ���� ID�� ���� ID�� ������
        {
            return Array.Find(ItemDatabase.Items, x => x.ID == ID).CreateItem();
        }
        else
        {
            Debug.Log("[Item Database] �� ��ġ�ϴ� ID�� �����ϴ�.");
            return null;
        }
    }
}
