using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] private GameObject _previewPrefab;
    [SerializeField] private GameObject _buildPrefab;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            _previewPrefab.tr
        }
    }
}
