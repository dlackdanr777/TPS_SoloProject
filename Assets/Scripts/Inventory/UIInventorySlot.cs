using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image IconImage;
    public TextMeshProUGUI AmountText;

    [HideInInspector] public Item _item;
    [HideInInspector] public int SlotIndex = -1;

    private UIInventory _uiInventory;

    public void Init(UIInventory uiInventory)
    {
        _uiInventory = uiInventory;
    }
    public bool GetisNull()
    {
        return _item == null;
    }


    public void UpdateUI(Item item)
    {
        if(item == null)
        {
            IconImage.enabled = false;
            AmountText.enabled = false;
            _item = null;
        }
        else
        {
            _item = item;
            IconImage.enabled = true;
            IconImage.sprite = _item.Data.Icon;
            _item.SlotIndex = SlotIndex;
            if (_item is CountableItem)
            {
                AmountText.enabled = true;
                AmountText.text = _item.Amount.ToString();
            }
            else
            {
                AmountText.enabled = false;
            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right) //만약 해당 스크립트가 들어있는 오브젝트를 우클릭 했을경우
        { 
            if(_item != null)
            {
                if(_item is IEquipmentItem) //장착아이템이라면
                {
                    //장착시키는 코드를 작성
                }
                else if( _item is IUsableItem) // 소비아이템이라면
                {
                    //소비시키는 코드를 작성
                    _uiInventory.UIDivItem.SetActive(_item);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) //마우스 드래그가 시작 됬을 때 실행되는 함수
    {
        if (eventData.button == PointerEventData.InputButton.Left) //만약 해당 스크립트가 들어있는 오브젝트를 좌클릭 했을경우
        {
            if (_item != null)
            {
                DragInventorySlot.Instance.DragSlot = this;
                DragInventorySlot.Instance.DragSetIcon(_item.Data.Icon);
                DragInventorySlot.Instance.transform.position = eventData.position;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //만약 해당 스크립트가 들어있는 오브젝트를 좌클릭 했을경우
        {
            if (_item != null)
            {
                DragInventorySlot.Instance.transform.position = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //만약 해당 스크립트가 들어있는 오브젝트를 좌클릭 했을경우
        {
            DragInventorySlot.Instance.SetColor(0);
            DragInventorySlot.Instance.DragSlot = null;
            DragInventorySlot.Instance.transform.position = new Vector3(1000, 1000, 1000);
        }

        if (!IsOverUI())
        {
            Debug.Log("아이템 드랍");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //만약 해당 스크립트가 들어있는 오브젝트를 좌클릭 했을경우
        {
            if (DragInventorySlot.Instance.DragSlot != null)
            {
                if(DragInventorySlot.Instance.DragSlot != this)
                {
                    if (_item != null)
                    {
                        if (_item.Data.ID == DragInventorySlot.Instance.DragSlot._item.Data.ID)
                        {
                            GameManager.Instance.Player.Inventory.MergeItem(_item, DragInventorySlot.Instance.DragSlot._item);
                            return;
                        }
                    }
                    _uiInventory.SlotSwap(this, DragInventorySlot.Instance.DragSlot);
                }
            }
        }
    }

    private bool IsOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }  
}
