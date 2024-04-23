using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary> �κ��丮 ���� ����� ���� Ŭ���� </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private UIInventory UIInventory;
    [SerializeField] private List<Item> _inventoryItems = new List<Item>();


    /// <summary> _inventoryItems.Count�� ��ȯ�ϴ� �Լ� </summary>
    public int GetItemCount()
    {
        return _inventoryItems.Count;
    }


    /// <summary> ���� ������ ����Ʈ[index]�� ��ȯ�ϴ� �Լ� </summary>
    public Item GetItemByIndex(int index)
    {
        return _inventoryItems[index];
    }


    /// <summary> ������ id������ �κ��丮�� �������� �߰��ϴ� �Լ� </summary>
    public bool AddItemByID(int ID, int amount = 1)
    {
        Item item = ItemManager.Instance.GetItemByID(ID);
        if (item == null)
        {
            Debug.Log("[Item Databse] �������� �ʴ� ItemID �Դϴ�.");
            return false;
        }

        if (item is CountableItem)
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


    /// <summary> �������� ����ϴ� �Լ� </summary>
    public bool UseItemByID(int ID, int amount = 1)
    {
        int itemCount = FindItemCountByID(ID);
        if (itemCount <= -1 && itemCount < amount)
        {
            Debug.Log("�����ϰ� �ִ� �������� ������ �����մϴ�.");
            return false;
        }


        List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == ID);
        foreach (Item item in items)
        {
            if (item.Amount < amount)
                continue;

            item.Amount -= amount;
            UIInventory.UpdateSlotUI(UIInventory.GetSlotByIndex(item.SlotIndex));

            if (item.Amount <= 0)
                RemoveItem(item);

            return true;
        }

        return false;
    }


    /// <summary> ������ Ŭ������ �������� �߰���Ű�� �Լ� </summary>
    private bool AddItemAmount(CountableItem item, int amount = 1)
    {

        UIInventorySlot updateSlot = UIInventory.GetSlotByIndex(item.SlotIndex);
        if (item.Amount + amount <= item.CountableData.MaxAmount)
        {
            item.Amount += amount;
            UIInventory.UpdateSlotUI(updateSlot);
            return true;
        }

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


    /// <summary> �������� ���ο� ���Կ� �߰���Ű�� �Լ� </summary>
    private bool AddItemToNewSlot(Item item, int amount = 1)
    {
        if (item == null)
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
            return false;
        }

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


    /// <summary> �ִ� ������ �ƴ� �������� ã�� �Լ� </summary>
    public Item FindExtraAountItem(Item item)
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


    /// <summary> List�� �ش� id�� ���� �������� ��ִ����� ��ȯ�ϴ� �Լ� </summary>
    public int FindExtraAountCountByID(int ID)
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


    /// <summary> itemList�� �ش�ID�� �ִ������ �ƴ� �������� ã�� list�� ��ȯ�ϴ� �Լ� </summary>
    public List<Item> FindExtraAountListByID(int ID)
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


    /// <summary> ������ ID�� ���� �κ��丮�� �ش� �������� � �ִ��� Ȯ���ϴ� �Լ� </summary>
    public int FindItemCountByID(int ID)
    {
        List<Item> items = _inventoryItems.FindAll(x => x.Data.ID == ID);
        int totalAmount = 0;
        if (items.Count <= 0)
            return -1;

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


    /// <summary> �ΰ��� �������� �����ִ��Լ� </summary>
    public void MergeItem(Item mainItem, Item subItem)
    {
        if (!(mainItem is CountableItem))
        {
            Debug.Log("��� �������� ��ĥ �� �����ϴ�.");
            int mainItemIndex = mainItem.SlotIndex;
            int subItemIndex = subItem.SlotIndex;
            UIInventory.SlotSwap(UIInventory.GetSlotByIndex(mainItemIndex), UIInventory.GetSlotByIndex(subItemIndex));
        }

        if (mainItem.Data.ID != subItem.Data.ID)
        {
            Debug.Log("�� �������� ID�� ���� �ٸ��ϴ�.");
            return;
        }


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


    /// <summary> �������� �����ϴ� �Լ� </summary>
    public void RemoveItem(Item item)
    {
        if (!_inventoryItems.Contains(item))
            return;

        UIInventory.GetSlotByIndex(item.SlotIndex).UpdateUI(null);
        _inventoryItems.Remove(item);
    }


    /// <summary> �������� �����ϴ� �Լ� </summary>
    public void RemoveItem(int ID)
    {
        Item item = ItemManager.Instance.GetItemByID(ID);
        if (item == null)
            return;

        if (!_inventoryItems.Contains(item))
            return;

        UIInventory.GetSlotByIndex(item.SlotIndex).UpdateUI(null);
        _inventoryItems.Remove(item);
    }


    /// <summary> �������� ������ ���ҽ�Ű�� �Լ� </summary>
    public void SubItem(Item item, int amount = 1)
    {
        if (!_inventoryItems.Contains(item))
            return;

        item.Amount -= amount;

        if (item.Amount <= 0)
            RemoveItem(item);
        else
            UIInventory.GetSlotByIndex(item.SlotIndex).UpdateUI(item);
    }


    /// <summary> �������� ������ ���ҽ�Ű�� �Լ� </summary>
    public void SubItemByID(int ID, int amount = 1)
    {
        Item item = _inventoryItems.Find(x => x.Data.ID == ID);
        if (!_inventoryItems.Contains(item))
            return;

        item.Amount -= amount;
        if (item.Amount <= 0)
            RemoveItem(item);
        else
            UIInventory.GetSlotByIndex(item.SlotIndex).UpdateUI(item);
    }


    /// <summary> �������� �ѷ� ������ �Լ� </summary>
    public void DivItem(Item item, int Amount)
    {
        if (item != null)
        {
            Debug.Log("�������� �����ϴ�.");
            return;
        }

        if (IsInventoryFull())
        {
            Debug.Log("�κ��丮�� �� á���ϴ�.");
            return;
        }


        if (item.Amount <= Amount)
        {
            Debug.Log("������ �������� ���� ������ Ů�ϴ�.");
            return;
        }

        Item newItem = item.Data.CreateItem();
        item.Amount -= Amount;
        AddItemToNewSlot(newItem, Amount);
        UIInventory.UpdateSlotUI(UIInventory.GetSlotByIndex(item.SlotIndex));
    }


    /// <summary> �κ��丮�� �����ϴ� �Լ� </summary>
    public void SortInventory()
    {
        var itemIDs = _inventoryItems.Select(x => x.Data.ID).Distinct().ToArray(); //�κ��丮�� �ִ� Item ID�� �ߺ��� �����ϰ� �̾Ƴ���.
        foreach (var ID in itemIDs) //ID�� ����ŭ �ݺ��Ѵ�.
        {
            if (!(ItemManager.Instance.GetItemByID(ID) is CountableItem)) //���� ������ �� �� �ִ� �������� �ƴ϶��
                continue;

            while (1 < FindExtraAountCountByID(ID)) //�ִ������ �ƴ� �ش�ID�� ������ �� ������ 1�̻��϶� �ݺ� 
            {
                List<Item> items = FindExtraAountListByID(ID); //�ִ������ �ƴ� �����۸���Ʈ�� �̾Ƴ���
                MergeItem(items[0], items[1]); //���ʺ��� ���ս�Ų��.
            }
        }

        _inventoryItems = _inventoryItems.OrderBy(x => x.Data.Name).ThenByDescending(x => x.Amount).ToList(); //�̸����� ��������, ������ �������� ����
        for (int i = 0, count =  _inventoryItems.Count; i < count; i++)
        {
            _inventoryItems[i].SlotIndex = i; //UI���� ��� �ε����� ���ʺ��� ������� �Է½�Ų��.
        }
        UIInventory.UpdateUI();
    }


    /// <summary> �κ��丮�� á���� ��, �ƴϸ� ������ ��ȯ </summary>
    public bool IsInventoryFull()
    {
        if(UIInventory.GetNullSlot() == -1)
            return true;

        return false;
    }

    /// <summary> UI�� �����ϴ� �Լ� </summary>
    public void OpenUI()
    {
        UIInventory.ChildSetActive();
        UIInventory.UpdateUI();
    }

}
