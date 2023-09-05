using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonHandler<ItemManager>
{
    public ItemDatabase ItemDatabase;

    public Item GetItemByID(int ID) 
    { 
        if(Array.Find(ItemDatabase.Items, x => x.ID == ID) != null) //만약 받은 ID와 같은 ID가 있으면
        {
            return Array.Find(ItemDatabase.Items, x => x.ID == ID).CreateItem();
        }
        else
        {
            Debug.Log("[Item Database] 상에 일치하는 ID가 없습니다.");
            return null;
        }
    }
}
