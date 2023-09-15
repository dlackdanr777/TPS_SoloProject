using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ObstacleBuild : MonoBehaviour
{
    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private GameObject[] _obstacleBluePrint;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private int _necessaryItemId;
    [SerializeField] private int _necessaryitemAmount;

    private GameObject[] _obstacle;
    private Inventory _inventory;

    private bool _buildEnable;
    public void Start()
    {
        _obstacle = new GameObject[_obstacleBluePrint.Length];
        _inventory = GameManager.Instance.Player.Inventory;
        _text.gameObject.SetActive(false);

        for (int i = 0; i < _obstacle.Length; i++)
        {
            Vector3 pos = _obstacleBluePrint[i].transform.position;
            Quaternion rot = _obstacleBluePrint[i].transform.rotation;
            _obstacle[i] = Instantiate(_obstaclePrefab, pos, rot);
            _obstacle[i].SetActive(false);
            _obstacleBluePrint[i].SetActive(false);
        }
    }

    public void Update()
    {
        if (_buildEnable)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                BuildObstacle();
            }
        }
    }

    public bool CheckBuildEnable()
    {
        int inventoryItemCount = _inventory.FindItemCountByID(_necessaryItemId);

        if (inventoryItemCount > 0 && inventoryItemCount >= _necessaryitemAmount)
        {
            for (int i = 0; i < _obstacle.Length; i++)
            {
                if (!_obstacle[i].activeSelf)
                    return true;
            }
        }

        return false;
    }

    public void BuildObstacle()
    {
        for (int i = 0; i < _obstacle.Length; i++)
        {
            if (!_obstacle[i].activeSelf)
            {
                if (_inventory.UseItemByID(_necessaryItemId, _necessaryitemAmount))
                {
                    _obstacle[i].SetActive(true);
                    _obstacleBluePrint[i].SetActive(false);
                    ShowObstacle();

                    return;
                }
            }
        }
    }

    public void ShowObstacle()
    {
        _buildEnable = true;
        for (int i = 0; i < _obstacleBluePrint.Length; i++)
        {
            if (!_obstacle[i].activeSelf)
                _obstacleBluePrint[i].SetActive(true);
        }

        string itemName = ItemManager.Instance.GetItemByID(_necessaryItemId).Data.Name;
        _text.gameObject.SetActive(true);
        _text.text = itemName + "[" + _necessaryitemAmount + "개 필요]";
        if (CheckBuildEnable())
            _text.text += "\n[B]입력";

        else
            _text.text += "\n설치불가";
    }

    public void HiddenObstacle()
    {
        _buildEnable = false;

        for (int i = 0; i < _obstacleBluePrint.Length; i++)
        {
            if (!_obstacle[i].activeSelf)
                _obstacleBluePrint[i].SetActive(false);
        }
        _text.gameObject.SetActive(false);
    }
}
