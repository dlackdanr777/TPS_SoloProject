using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image IconImage;
    public TextMeshProUGUI AmountText;

    [HideInInspector] public Item _item;
    [HideInInspector] public int SlotIndex = -1;

    private UIInventory _uiInventory;
    private RectTransform _rectTransform;

    public void Init(UIInventory uiInventory)
    {
        _uiInventory = uiInventory;
        _rectTransform = GetComponent<RectTransform>();
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
        if(eventData.button == PointerEventData.InputButton.Right) //���� �ش� ��ũ��Ʈ�� ����ִ� ������Ʈ�� ��Ŭ�� �������
        { 
            if(_item != null)
            {
                if(_item is IEquipmentItem) //�����������̶��
                {
                    //������Ű�� �ڵ带 �ۼ�
                }
                else if( _item is IUsableItem) // �Һ�������̶��
                {
                    //�Һ��Ű�� �ڵ带 �ۼ�
                    _uiInventory.UiDivItem.SetActive(_item);
                }
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData) //���콺 �巡�װ� ���� ���� �� ����Ǵ� �Լ�
    {
        if (eventData.button == PointerEventData.InputButton.Left) //���� �ش� ��ũ��Ʈ�� ����ִ� ������Ʈ�� ��Ŭ�� �������
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
        if (eventData.button == PointerEventData.InputButton.Left) //���� �ش� ��ũ��Ʈ�� ����ִ� ������Ʈ�� ��Ŭ�� �������
        {
            if (_item != null)
            {
                DragInventorySlot.Instance.transform.position = eventData.position;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //���� �ش� ��ũ��Ʈ�� ����ִ� ������Ʈ�� ��Ŭ�� �������
        {
            DragInventorySlot.Instance.SetColor(0);
            DragInventorySlot.Instance.DragSlot = null;
            DragInventorySlot.Instance.transform.position = new Vector3(1000, 1000, 1000);
        }

        if (!IsOverUI())
        {
            Debug.Log("������ ���");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //���� �ش� ��ũ��Ʈ�� ����ִ� ������Ʈ�� ��Ŭ�� �������
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_item != null)
        {
            _uiInventory.UiItemDescription.ChildSetActive(true);

            Vector3 slotHalfSize = _rectTransform.sizeDelta * 0.5f;
            Vector3 getUiSize = _uiInventory.UiItemDescription.GetUISize() * 0.5f;
            Vector3 uiPos = new Vector3(getUiSize.x, -getUiSize.y) + slotHalfSize;
            
            _uiInventory.UiItemDescription.transform.position = transform.position + uiPos;
            _uiInventory.UiItemDescription.UpdateUI(_item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _uiInventory.UiItemDescription.ChildSetActive(false);
    }
}
