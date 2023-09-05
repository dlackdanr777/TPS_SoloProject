using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectPoolType
{
    Bullet,
    size
}

public class ObjectPoolManager : SingletonHandler<ObjectPoolManager>
{
    public GameObject BulletHolePrefab;
    [SerializeField] private int _bulletHoleCount;
    private GameObject _bulletHoleParent;
    private Queue<GameObject> _bulletHolePool;


    private void Start()
    {
        BulletHoleObjectPooling();
    }

    private void BulletHoleObjectPooling()
    {
        _bulletHolePool = new Queue<GameObject>();
       _bulletHoleParent = new GameObject("BulletHoleParent");

        for(int i =0, count = _bulletHoleCount; i < count; i++)
        {
            GameObject bulletHole = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            _bulletHolePool.Enqueue(bulletHole);
            bulletHole.transform.parent = _bulletHoleParent.transform;
            bulletHole.SetActive(false);
        }
    }

    public void SpawnBulletHole(Vector3 pos, Quaternion rot)
    {
        GameObject bulletHole = _bulletHolePool.Dequeue();
        bulletHole.SetActive(false);
        bulletHole.SetActive(true);
        bulletHole.transform.position = pos;
        bulletHole.transform.rotation = rot;
        _bulletHolePool.Enqueue(bulletHole);
    } 
}
