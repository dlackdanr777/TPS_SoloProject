using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    private List<Collider> _colliders = new List<Collider>();
    private const int IGNORE_RAYCAST_LAYER = 2;

    private int _itemID;
    private int _itemAmount;


    private void FixedUpdate()
    {
        ChangeColor();
    }

    public void SetItem(int id, int Amount)
    {
        _itemID = id;
        _itemAmount = Amount;
    }

    private void ChangeColor()
    {
        int itemCount = GameManager.Instance.Player.Inventory.FindItemCountByID(_itemID);

        if (_colliders.Count > 0 || itemCount < _itemAmount)
            SetColor(_redMaterial);
        else
            SetColor(_greenMaterial);
    }

    private void SetColor(Material mat)
    {
        transform.GetComponent<Renderer>().material = mat;
        foreach (Transform tfChild in transform)
        {
            Material[] newMaterials = new Material[tfChild.GetComponent<Renderer>().materials.Length];

            for (int i = 0, count = newMaterials.Length; i < count; i++)
                newMaterials[i] = mat;

            tfChild.GetComponent<Renderer>().materials = newMaterials;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 9 && other.gameObject.layer != 17 && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            _colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 9 && other.gameObject.layer != 17 &&  other.gameObject.layer != IGNORE_RAYCAST_LAYER)
        {
            _colliders.Remove(other);
        }
    }

    public bool isBuildable()
    {
        return _colliders.Count == 0;
    }
}
