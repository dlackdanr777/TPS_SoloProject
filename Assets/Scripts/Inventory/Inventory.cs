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
            Debug.Log("[Item Databse] �������� �ʴ� ItemID �Դϴ�.");
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
            Debug.Log("�����ϰ� �ִ� �������� ������ �����մϴ�.");
        }

        return false;
    }

    private bool AddItemAmount(CountableItem item, int amount = 1) //������ ī��Ʈ�� ������Ű�� �Լ�
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
                Debug.Log("�κ��丮�� �����ֽ��ϴ�.");
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
                Debug.Log("�κ��丮 ������ ���ֽ��ϴ�.");
                return false;
            }
        }
        else
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
            return false;
        }
    }

    public Item FindExtraAountItem(Item item) //�ִ������ �ƴ� �������� ã�� �Լ�
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

    public int FindExtraAountCountByID(int ID) //�ش� ID�� list�� ��ִ����� ��ȯ�ϴ� �Լ�
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

    public List<Item> FindExtraAountListByID(int ID) //itemList�� �ش�ID�� �ִ������ �ƴ� �������� ã�� list�� ��ȯ�ϴ� �Լ�
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
    /// ������ ID�� ���� �κ��丮�� �ش� �������� � �ִ��� Ȯ���ϴ� �Լ�
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

    public void MergeItem(Item mainItem, Item subItem) //�ΰ��� �������� �����ִ��Լ�
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
                Debug.Log("�� �������� ID�� ���� �ٸ��ϴ�.");
            }
        }
        else
        {
            Debug.Log("��� �������� ��ĥ �� �����ϴ�.");
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

    public void DivItem(Item item, int Amount) //�ϳ��� �������� �ѷ� �����ִ� �Լ�
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
                    Debug.Log("������ �������� ���� ������ Ů�ϴ�.");
                }
            }
            else
            {
                Debug.Log("�κ��丮�� �� á���ϴ�.");
            }       
        }
        else
        {
            Debug.Log("�������� �����ϴ�.");
        }
    }

    public void SortInventory() //�κ��丮�� �����ϴ� �Լ�
    {
        var itemIDs = _inventoryItems.Select(x => x.Data.ID).Distinct().ToArray(); //�κ��丮�� �ִ� Item ID�� �ߺ��� �����ϰ� �̾Ƴ���.
        foreach(var ID in itemIDs) //ID�� ����ŭ �ݺ��Ѵ�.
        {
            if(ItemManager.Instance.GetItemByID(ID) is CountableItem) //���� �ش� �������̸�
            {
                while (FindExtraAountCountByID(ID) > 1) //�ִ������ �ƴ� �ش�ID�� ������ �� ������ 1�̻��϶� �ݺ� 
                {
                    List<Item> items = FindExtraAountListByID(ID); //�ִ������ �ƴ� �����۸���Ʈ�� �̾Ƴ���
                    MergeItem(items[0], items[1]); //���ʺ��� ���ս�Ų��.
                }
            }
        }
        _inventoryItems = _inventoryItems.OrderBy(x => x.Data.Name).ThenByDescending(x => x.Amount).ToList(); //�̸����� ��������, ������ �������� ����
        for (int i = 0, count =  _inventoryItems.Count; i < count; i++)
        {
            _inventoryItems[i].SlotIndex = i; //UI���� ��� �ε����� ���ʺ��� ������� �Է½�Ų��.
        }
        UIInventory.UpdateUI();
    }

    
    public bool IsInventoryFull() //�κ��丮UI�� ��ã������ �˻��ϴ� �׸�
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
