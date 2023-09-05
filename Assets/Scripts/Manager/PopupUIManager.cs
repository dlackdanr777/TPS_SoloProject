using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : SingletonHandler<PopupUIManager>
{
    public PopupUI UIInventory;

    public bool PopupEnable;
    //Ű�Է�
    private KeyCode EscapeKey = KeyCode.Escape;
    private KeyCode InventoryKey = KeyCode.I;

    private LinkedList<PopupUI> _activePopupList;
    private List<PopupUI> _allPopupList;

    private void Start()
    {
        _activePopupList = new LinkedList<PopupUI>();

        Init();
        InitCloseAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(EscapeKey))
        {
            if(_activePopupList.Count > 0)
            {
                ClosePopup(_activePopupList.First.Value);
            }
        }

        TogglekeyDownAction(InventoryKey, UIInventory);
    }

    private void Init()
    {
        _allPopupList = new List<PopupUI>()
        { UIInventory };

        foreach (var popup in _allPopupList)
        {
            popup.OnFocus += () =>
            {
                _activePopupList.Remove(popup);
                _activePopupList.AddFirst(popup);
                RefreshAllPopupDepth();
            };
            popup.CloseButton.onClick.AddListener(() => ClosePopup(popup));
        }
    }

    private void InitCloseAll()
    {

        foreach(var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }

    private void TogglekeyDownAction(in KeyCode key, PopupUI popup)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleOpenClosePopup(popup);
        }
    }

    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if (!popup.UIEnable) OpenPopup(popup);
        else ClosePopup(popup);
        CheckPopupEnabled();
    }

    private void OpenPopup(PopupUI popup)
    {
        _activePopupList.AddFirst(popup);
        popup.ChildSetActive(true);

        CheckPopupEnabled();
        RefreshAllPopupDepth();
    }

    private void ClosePopup(PopupUI popup)
    {
        _activePopupList.Remove(popup);
        popup.ChildSetActive(false);
        CheckPopupEnabled();
        RefreshAllPopupDepth();
    }

    private void CheckPopupEnabled()
    {
        if (_activePopupList.Count <= 0)
        {
            GameManager.Instance.CursorHidden();
            PopupEnable = false;
        }
        else
        {
            GameManager.Instance.CursorVisible();
            PopupEnable = true;
        }
    }

    private void RefreshAllPopupDepth() //��ũ�� ����Ʈ ���� ��� �˾��� ������ ���ġ�ϴ� �Լ�
    {
        foreach(var popup in _allPopupList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }
}
