using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTrigger : MonoBehaviour
{
    [SerializeField] private InGame _ingame;
    void Start()
    {
        _ingame.enabled = false;
        ObjectPoolManager.Instance.BulletHoleObjectPooling();
        ObjectPoolManager.Instance.ZombieObjectPooling(ZombieType.Basic);
        ObjectPoolManager.Instance.ZombieObjectPooling(ZombieType.Women);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _ingame.enabled = true;
            this.enabled = false;
        }
    }
}
