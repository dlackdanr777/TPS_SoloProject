using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    private List<Collider> _colliders = new List<Collider>();
    private const int IGNORE_RAYCAST_LAYER = 2;

    private void Update()
    {
        ChangeColor();

    }

    private void ChangeColor()
    {
        if (_colliders.Count > 0)
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
            Debug.Log(other.gameObject.layer);
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
