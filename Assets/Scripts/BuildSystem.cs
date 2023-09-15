using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] Material _material;
    [SerializeField] Camera _camera;

    [SerializeField] private float _ratetePerSecond;
    RaycastHit _hit;
    Ray _ray;
    LayerMask _layerMask;

    public void Start()
    {
        _layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    public void Update()
    {
        Building();
    }

    public void Building()
    {
        Ray _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if(Physics.Raycast(_ray, out _hit, 10, _layerMask))
        {
            _prefab.SetActive(true);
            Vector3 location = _hit.point;
            location.Set(Mathf.Round(location.x * 10f) * 0.1f, location.y, Mathf.Round(location.z * 10f) * 0.1f);
            _prefab.transform.position = location;

            if (Input.GetKey(KeyCode.Q))
                _prefab.transform.eulerAngles -= Vector3.up * _ratetePerSecond * Time.deltaTime;

            else if (Input.GetKey(KeyCode.E))
                _prefab.transform.eulerAngles += Vector3.up * _ratetePerSecond * Time.deltaTime;
        }
        else
        {
            _prefab.SetActive(false);
        }
    }
}
