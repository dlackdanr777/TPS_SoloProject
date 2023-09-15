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

    ObstacleBuild _obstacleBuild;
    private void Update()
    {
        _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(_ray, out _hit, _rayDistance, _layerMask))
        {
            if (_obstacleBuild == null)
            {
                if (_hit.transform.GetComponent<ObstacleBuild>() != null)
                {
                    _obstacleBuild = _hit.transform.GetComponent<ObstacleBuild>();
                    _obstacleBuild.ShowObstacle();
                }
            }
        }
        else
        {
            if(_obstacleBuild != null)
            {
                _obstacleBuild.HiddenObstacle();
                _obstacleBuild = null;
            }
        }
    }
}
