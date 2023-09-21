using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainCameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _layerMask;
    
    Ray _ray;
    RaycastHit _hit;

    Iinteractive _interactive;

    private void Update()
    {
        _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(_ray, out _hit, _rayDistance, _layerMask))
        {
            if (_interactive == null)
            {
                if (_hit.transform.GetComponent<Iinteractive>() != null)
                {
                    _interactive = _hit.transform.GetComponent<Iinteractive>();
                    _interactive.EnableInteraction();
                }
            }
        }
        else
        {
            if(_interactive != null)
            {
                _interactive.DisableInteraction();
                _interactive = null;
            }
        }

        if(_interactive != null)
        {

            if (Input.GetKeyDown(_interactive.InputKey))
            {
                _interactive.Interact();
                _interactive.DisableInteraction();
                _interactive = null;

            }
        }

    }
}
