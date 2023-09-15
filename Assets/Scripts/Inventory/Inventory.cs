using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] private UIInventory UIInventory;
    [SerializeField] private List<Item> _inventoryItems = new List<Item>();


    public int GetItemCount()
    {
        return _inventoryItems.Count;
    }

    public Item GetItemByIndex(int index)
    {
        return _inventoryItems[index];
    }

    public bool AddItemByID(int ID, int amount = 1)
    {
        Item item = ItemManager.Instance.GetItemByID(ID);
        if(item != null)
        {
            if(item is CountableItem)
            {
                if (_inventoryItems.Exists(x => x.Data.ID == item.Data.ID))
                {
                    Item countableItem = FindExtraAountItem(item);
                    if (countableItem != null)
                    {
                        return AddItemAmount(countableItem as CountableItem, amount);
                    }   
                }
                return AddItemToNewSlot(item, amount);
            }
            return AddItemToNewSlot(item);
        }
        else
        {
            Debug.Log("[Item Databse] 존재하지 않는 ItemID 입니다.");
            return false;
        }
    }

    public bool UseItemByID(int ID, int amount = 1)
    {
        int itemCount = FindItemCountByID(ID);
        if (itemCount > -1 && itemCount >= amount)
        {
            List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == ID);
            foreach(Item item in items)
            {
                if(item.Amount > amount)
                {
                    item.Amount -= amount;
                    UIInventory.UpdateSlotUI(UIInventory.GetSlotByIndex(item.SlotIndex));
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("소지하고 있는 아이템의 갯수가 부족합니다.");
        }

        return false;
    }

    private bool AddItemAmount(CountableItem item, int amount = 1) //아이템 카운트를 증가시키는 함수
    {

        UIInventorySlot updateSlot = UIInventory.GetSlotByIndex(item.SlotIndex);
        if (item.Amount + amount > item.CountableData.MaxAmount)
        {
            if (!IsInventoryFull())
            {
                Item newItem = ItemManager.Instance.GetItemByID(item.Data.ID);
                int restAmount = item.Amount + amount - item.CountableData.MaxAmount;
                item.Amount = item.CountableData.MaxAmount;
                UIInventory.UpdateSlotUI(updateSlot);
                return AddItemToNewSlot(newItem, restAmount);
            }
            else
            {
                Debug.Log("인벤토리가 꽉차있습니다.");
                return false;
            }
        }
        else
        {
            item.Amount += amount;
            UIInventory.UpdateSlotUI(updateSlot);
            return true;
        }
    }


    private bool AddItemToNewSlot(Item item, int amount = 1)
    {
        if (item != null)
        {
            if (!IsInventoryFull())
            {
                item.Amount = amount;
                _inventoryItems.Add(item);
                int slotIndex = UIInventory.GetNullSlot();
                UIInventorySlot nullSlot = UIInventory.GetSlotByIndex(slotIndex);

                UIInventory.ChangeSlotItem(nullSlot, item);
                return true;
            }
            else
            {
                Debug.Log("인벤토리 슬롯이 차있습니다.");
                return false;
            }
        }
        else
        {
            Debug.Log("아이템이 존재하지 않습니다.");
            return false;
        }
    }

    public Item FindExtraAountItem(Item item) //최대수량이 아닌 아이템을 찾는 함수
    {
        if(item is CountableItem)
        {
            List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == item.Data.ID);
            for (int i = 0, count = items.Count; i < count; i++)
            {
                CountableItem CountableItem = items[i] as CountableItem;
                if (items[i].Amount < CountableItem.CountableData.MaxAmount)
                {
                    return items[i];
                }
            }
        }
        return null;
    }

    public int FindExtraAountCountByID(int ID) //해당 ID가 list에 몇개있는지를 반환하는 함수
    {
        List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == ID);
        int itemCount = 0;
        for (int i = 0, count = items.Count; i < count; i++)
        {
            CountableItem CountableItem = items[i] as CountableItem;
            if (items[i].Amount < CountableItem.CountableData.MaxAmount)
            {
                itemCount++;
            }
        }
        return itemCount;
    }

    public List<Item> FindExtraAountListByID(int ID) //itemList에 해당ID의 최대수량이 아닌 아이템을 찾아 list로 반환하는 함수
    {
        List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == ID);
        List<Item> itemBox = new List<Item>();
        for (int i = 0, count = items.Count; i < count; i++)
        {
            CountableItem CountableItem = items[i] as CountableItem;
            if (items[i].Amount < CountableItem.CountableData.MaxAmount)
            {
                itemBox.Add(items[i]);
            }
        }
        return itemBox;
    }

    /// <summary>
    /// 아이템 ID를 통해 인벤토리에 해당 아이템이 몇개 있는지 확인하는 함수
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public int FindItemCountByID(int ID)
    {
        List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == ID);
        int totalAmount = 0;
        if(items.Count > 0)
        {
            if (items[0] is CountableItem)
            {
                for (int i = 0, count = items.Count; i < count; i++)
                {
                    CountableItem CountableItem = items[i] as CountableItem;
                    totalAmount += CountableItem.Amount;
                }
            }
            else if (items[0] is EquipmentItem)
            {
                totalAmount = items.Count;
            }

            return totalAmount;
        }
        else
        {
            return -1;
        }
    }

    public void MergeItem(Item mainItem, Item subItem) //두개의 아이템을 합쳐주는함수
    {
        if(mainItem is CountableItem)
        {
            if (mainItem.Data.ID == subItem.Data.ID)
            {
                CountableItem CountableItem = mainItem as CountableItem;
                if (mainItem.Amount + subItem.Amount <= CountableItem.CountableData.MaxAmount)
                {
                    mainItem.Amount += subItem.Amount;
                    RemoveItem(subItem);
                }
                else
                {
                    if (mainItem.Amount == CountableItem.CountableData.MaxAmount)
                    {
                        int mainItemIndex = mainItem.SlotIndex;
                        int subItemIndex = subItem.SlotIndex;
                        UIInventory.SlotSwap(UIInventory.GetSlotByIndex(mainItemIndex), UIInventory.GetSlotByIndex(subItemIndex));
                    }
                    else
                    {
                        subItem.Amount -= CountableItem.CountableData.MaxAmount - mainItem.Amount;
                        mainItem.Amount = CountableItem.CountableData.MaxAmount;
                    }
                }
                UIInventory.UpdateSlotUI(UIInventory.GetSlotByIndex(mainItem.SlotIndex));
                UIInventory.UpdateSlotUI(UIInventory.GetSlotByIndex(subItem.SlotIndex));
            }
            else
            {
                Debug.Log("두 아이템의 ID가 서로 다릅니다.");
            }
        }
        else
        {
            Debug.Log("장비 아이템은 합칠 수 없습니다.");
            int mainItemIndex = mainItem.SlotIndex;
            int subItemIndex = subItem.SlotIndex;
            UIInventory.SlotSwap(UIInventory.GetSlotByIndex(mainItemIndex), UIInventory.GetSlotByIndex(subItemIndex));
        }   
    }

    public void RemoveItem(Item item)
    {
        if (_inventoryItems.Contains(item))
        {
            UIInventory.GetSlotByIndex(item.SlotIndex).UpdateUI(null);
            _inventoryItems.Remove(item);
        }
    }

    public void RemoveItem(int ID)
    {
        Item item = ItemManager.Instance.GetItemByID(ID);
        if(item != null)
        {
            if (_inventoryItems.Contains(item))
            {
                UIInventory.GetSlotByIndex(item.SlotIndex).UpdateUI(null);
                _inventoryItems.Remove(item);
            }
        }
    }

    public void DivItem(Item item, int Amount) //하나의 아이템을 둘로 나눠주는 함수
    {
        if (item != null)
        {
            if (!IsInventoryFull())
            {
                if (item.Amount > Amount)
                {
                    Item newItem = item.Data.CreateItem();
                    item.Amount -= Amount;
                    AddItemToNewSlot(newItem, Amount);
                    UIInventory.UpdateSlotUI(UIInventory.GetSlotByIndex(item.SlotIndex));
                }
                else
                {
                    Debug.Log("아이템 수량보다 나눌 갯수가 큽니다.");
                }
            }
            else
            {
                Debug.Log("인벤토리가 꽉 찼습니다.");
            }       
        }
        else
        {
            Debug.Log("아이템이 없습니다.");
        }
    }

    public void SortInventory() //인벤토리를 정렬하는 함수
    {
        var itemIDs = _inventoryItems.Select(x => x.Data.ID).Distinct().ToArray(); //인벤토리에 있는 Item ID를 중복을 제거하고 뽑아낸다.
        foreach(var ID in itemIDs) //ID의 수만큼 반복한다.
        {
            if(ItemManager.Instance.GetItemByID(ID) is CountableItem) //만약 해당 아이템이면
            {
                while (FindExtraAountCountByID(ID) > 1) //최대수량이 아닌 해당ID의 아이템 의 갯수가 1이상일때 반복 
                {
                    List<Item> items = FindExtraAountListByID(ID); //최대수량이 아닌 아이템리스트를 뽑아내서
                    MergeItem(items[0], items[1]); //앞쪽부터 병합시킨다.
                }
            }
        }
        _inventoryItems = _inventoryItems.OrderBy(x => x.Data.Name).ThenByDescending(x => x.Amount).ToList(); //이름으로 오름차순, 갯수로 내림차순 정렬
        for (int i = 0, count =  _inventoryItems.Count; i < count; i++)
        {
            _inventoryItems[i].SlotIndex = i; //UI상의 장소 인덱스를 앞쪽부터 순서대로 입력시킨다.
        }
        UIInventory.UpdateUI();
    }

    
    public bool IsInventoryFull() //인벤토리UI가 꽉찾는지를 검사하는 항목
    {
        if(UIInventory.GetNullSlot() == -1)
        {
            return true;
        }
        return false;
    }


    public void OpenUI()
    {
        UIInventory.ChildSetActive();
        UIInventory.UpdateUI();
    }

}
