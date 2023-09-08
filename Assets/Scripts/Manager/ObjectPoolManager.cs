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
    [SerializeField] private GameObject BulletHolePrefab;
    [SerializeField] private int _bulletHoleCount;
    private GameObject _bulletHoleParent;
    private Queue<GameObject> _bulletHolePool;

    [SerializeField] private GameObject _basicZombiePrefab;
    [SerializeField] private int _zombieCount;
    private GameObject _zombieParent;
    private Queue<GameObject> _zombiePool;

    private void Start()
    {
        BulletHoleObjectPooling();
        BasicZombieObjectPooling();
    }

    private void BulletHoleObjectPooling()
    {
        _bulletHolePool = new Queue<GameObject>();
       _bulletHoleParent = new GameObject("BulletHoleParent");

        for(int i = 0, count = _bulletHoleCount; i < count; i++)
        {
            GameObject bulletHole = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            _bulletHolePool.Enqueue(bulletHole);
            bulletHole.transform.parent = _bulletHoleParent.transform;
            bulletHole.SetActive(false);
        }
    }

    private void BasicZombieObjectPooling()
    {
        _zombiePool = new Queue<GameObject>();
        _zombieParent = new GameObject("ZombieParent");

        for (int i = 0, count = _zombieCount; i < count; i++)
        {
            GameObject zombie = Instantiate(_basicZombiePrefab, Vector3.zero, Quaternion.identity);
            _zombiePool.Enqueue(zombie);
            zombie.transform.parent = _zombieParent.transform;
            zombie.SetActive(false);
        }
    }

    public GameObject SpawnBulletHole( Vector3 pos, Quaternion rot)
    {
        GameObject bulletHole = _bulletHolePool.Dequeue();
        if(bulletHole == null)
        {
            bulletHole = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("������");
        }
        bulletHole.SetActive(false);
        bulletHole.SetActive(true);
        bulletHole.transform.position = pos;
        bulletHole.transform.rotation = rot;
        _bulletHolePool.Enqueue(bulletHole);
        return bulletHole;
    }


    public void SpawnZombie(Vector3 pos, Quaternion rot)
    {
        GameObject zombie = _zombiePool.Dequeue();
        zombie.SetActive(false);
        zombie.SetActive(true);
        zombie.transform.position = pos;
        zombie.transform.rotation = rot;
        _zombiePool.Enqueue(zombie);
    }

}
