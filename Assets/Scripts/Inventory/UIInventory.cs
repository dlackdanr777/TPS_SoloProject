using UnityEngine;
using UnityEngine.UI;

public class UIInventory : PopupUI
{
    [SerializeField] private Transform _gridLayout; //슬롯들을 자식으로 가지고있는 오브젝트

    public UIDivItem UiDivItem;

    public UIItemDescription UiItemDescription;

    private UIInventorySlot[] _slots;

    private Inventory _inventory;

    [SerializeField] private Button _sortButton;

    public  void Start()
    {
        _inventory = GameManager.Instance.Player.Inventory;
        _sortButton.onClick.AddListener(_inventory.SortInventory);
        SetSlots();
        UpdateUI();
    }

    private void SetSlots()
    {
        _slots = _gridLayout.GetComponentsInChildren<UIInventorySlot>();
        for(int i = 0, count = _slots.Length; i < count; i++)
        {
            _slots[i].SlotIndex = i;
            _slots[i].Init(this);
        }
    }

    public UIInventorySlot GetSlotByIndex(int index)
    {
        if(index >= 0 && index < _slots.Length)
        {
            UIInventorySlot slot = _slots[index];
            return slot;
        }
        return null;
    }

    public void SlotSwap(UIInventorySlot slotA, UIInventorySlot slotB)
    {
        Item tempItem = slotA._item;
        slotA.UpdateUI(slotB._item);
        slotB.UpdateUI(tempItem);
    }

    public void ChangeSlotItem(UIInventorySlot slot, Item item)
    {
        slot._item = item;
        UpdateSlotUI(slot);
    }


    public int GetNullSlot()
    {
        for(int i = 0, count = _slots.Length; i < count; i++)
        {
            if (_slots[i].GetisNull())
            {
                return i;
            }
        }
        return -1;
    }

    public override void ChildSetActive(bool value)
    {
        UiDivItem.ChildSetActive(false);
        UiItemDescription.ChildSetActive(false);
        base.ChildSetActive(value);
    }

    public void UpdateUI()
    {
        for (int i = 0, count = _slots.Length; i < count; i++)
        {
            _slots[i].UpdateUI(null);
        }
        for (int i = 0, count = _inventory.GetItemCount(); i < count; i++)
        {
            _slots[_inventory.GetItemByIndex(i).SlotIndex].UpdateUI(_inventory.GetItemByIndex(i));
        }
    }

    public void UpdateSlotUI(UIInventorySlot slot)
    {
        slot.UpdateUI(slot._item);
    }

}
