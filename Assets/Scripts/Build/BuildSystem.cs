using System;
using UnityEngine;

[Serializable]
public struct Craft
{
    public Sprite Sprite;
    public string Name;
    public int NecessaryItemId;
    public int NecessaryItemAmount;
    public GameObject CraftPrefab;
    public PreviewObject PreviewPrefab;
}


public class BuildSystem : MonoBehaviour
{
    [SerializeField] private Craft[] _craftItem;
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioClip _buildSuccessClip;
    private int _craftItemIndex = -1;
    private PreviewObject _PreviewObj;
    private RaycastHit _hit;
    private LayerMask _layerMask;
    private Ray _ray;

    [SerializeField] private bool _buildingEnable;

    public void Start()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    public void Update()
    {

        if (_buildingEnable)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                BuildDisable();
        }

        BuildEnable();
    }

    public int GetCraftItemLength()
    {
        return _craftItem.Length;
    }

    public Craft GetCraftItem(int index)
    {
        return _craftItem[index];
    }

    public void SelectCraftItem(int index)
    {
        if (index < 0 && index >= _craftItem.Length) { Debug.Log("�ε������� ���"); return; }

        if (_PreviewObj != null)
            Destroy(_PreviewObj.gameObject);


        _craftItemIndex = index;
        _PreviewObj = Instantiate(_craftItem[_craftItemIndex].PreviewPrefab.gameObject, Vector3.zero, _craftItem[_craftItemIndex].PreviewPrefab.transform.rotation).
            GetComponent<PreviewObject>();
        _buildingEnable = true;
    }

    public void BuildDisable()
    {
        if (_PreviewObj != null)
            Destroy(_PreviewObj.gameObject);

        _craftItemIndex = -1;
        _buildingEnable = false;
    }


    /// <summary>���๰ ��ġ ���������� �ֱ������� �ҷ����� �Լ�</summary>
    public void BuildEnable()
    {
        if (!_buildingEnable || _craftItemIndex == -1)
            return;

        //ī�޶󿡼� ���̸� �� ���� Ư�� ���̾ ���� ������Ʈ�� ���� ��� return
        _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (!Physics.Raycast(_ray, out _hit, 10, _layerMask))
            return;

        //ray�� ���� ��ġ�� �̸����� ������Ʈ ����
        Vector3 location = _hit.point;
        location.Set(Mathf.Round(location.x * 10f) * 0.1f, location.y, Mathf.Round(location.z * 10f) * 0.1f);
        _PreviewObj.transform.position = location;

        int itemId = _craftItem[_craftItemIndex].NecessaryItemId;
        int amount = _craftItem[_craftItemIndex].NecessaryItemAmount;
        _PreviewObj.SetItem(itemId, amount);

        //Q, E Ű�� �Է¹޾� ������Ʈ ȸ��
        if (Input.GetKeyDown(KeyCode.Q))
            _PreviewObj.transform.eulerAngles -= Vector3.up * 30f;

        else if (Input.GetKeyDown(KeyCode.E))
            _PreviewObj.transform.eulerAngles += Vector3.up * 30f;

        //���� ���콺 ��Ŭ���� ������ ���� ��� return
        if (!Input.GetMouseButtonDown(0))
            return;

        //������ �������� Ȯ�� �� �ش� ��ġ�� ���๰ ����
        if (!_PreviewObj.isBuildable())
        {
            UIManager.Instance.ShowCenterText("�װ��� �Ǽ��� �� �����ϴ�.");
            return;
        }

        if (GameManager.Instance.Player.Inventory.UseItemByID(itemId, amount))
        {
            Instantiate(_craftItem[_craftItemIndex].CraftPrefab, _PreviewObj.transform.position, _PreviewObj.transform.rotation);
            SoundManager.Instance.PlayAudio(AudioType.Effect, _buildSuccessClip);
        }
        else
        {
            UIManager.Instance.ShowCenterText("�Ǽ� ��ᰡ �����մϴ�.");
        }
    }
}
