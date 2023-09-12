using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieType
{
    Basic,
    Women,
    Heavy,
    Count
}

public class ObjectPoolManager : SingletonHandler<ObjectPoolManager>
{
    [Serializable]
    private struct ZombieStruct
    {
        public ZombieType Type;
        public int PoolCount;
        public GameObject Prefab;
        [HideInInspector] public Queue<GameObject> Pool;
        [HideInInspector] public GameObject Parent;
    }

    [SerializeField] private GameObject BulletHolePrefab;
    [SerializeField] private int _bulletHoleCount;
    private GameObject _bulletHoleParent;
    private Queue<GameObject> _bulletHolePool;

    [SerializeField] private ZombieStruct[] _zombieStruct;


    private void Start()
    {
        BulletHoleObjectPooling();
        ZombieObjectPooling(ZombieType.Basic);
        ZombieObjectPooling(ZombieType.Women);
    }

    private void BulletHoleObjectPooling()
    {
        _bulletHolePool = new Queue<GameObject>();
       _bulletHoleParent = new GameObject("BulletHoleParent");

        for(int i = 0, count = _bulletHoleCount; i < count; i++)
        {
            GameObject bulletHole = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            bulletHole.transform.parent = _bulletHoleParent.transform;
            _bulletHolePool.Enqueue(bulletHole);
            bulletHole.SetActive(false);
        }
    }

    private void ZombieObjectPooling(ZombieType zombieType)
    {
        _zombieStruct[(int)zombieType].Pool = new Queue<GameObject>();
        _zombieStruct[(int)zombieType].Parent = new GameObject(Enum.GetName(typeof(ZombieType), (int)zombieType) + " Zombie Parent======");

        for (int i = 0, count = _zombieStruct[(int)zombieType].PoolCount; i < count; i++)
        {
            GameObject zombie = Instantiate(_zombieStruct[(int)zombieType].Prefab, Vector3.zero, Quaternion.identity);
            zombie.transform.parent = _zombieStruct[(int)zombieType].Parent.transform;
            _zombieStruct[(int)zombieType].Pool.Enqueue(zombie);
            zombie.SetActive(false);
        }
    }

    public GameObject SpawnBulletHole( Vector3 pos, Quaternion rot)
    {
        GameObject bulletHole = _bulletHolePool.Dequeue();
        if(bulletHole == null)
        {
            bulletHole = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("½ºÆùÇÔ");
        }
        bulletHole.SetActive(false);
        bulletHole.SetActive(true);
        bulletHole.transform.position = pos;
        bulletHole.transform.rotation = rot;
        _bulletHolePool.Enqueue(bulletHole);
        return bulletHole;
    }

    public void SpawnZombie(ZombieType zombieType, Vector3 pos, Quaternion rot)
    {
        if(_zombieStruct[(int)zombieType].Pool.Count == 0)
        {
            GameObject zombiePool = Instantiate(_zombieStruct[(int)zombieType].Prefab, pos, rot);
            _zombieStruct[(int)zombieType].Pool.Enqueue(zombiePool);
            zombiePool.transform.parent = _zombieStruct[(int)zombieType].Parent.transform;
            zombiePool.SetActive(false);
        }
        GameObject zombie = _zombieStruct[(int)zombieType].Pool.Dequeue();
        zombie.SetActive(true);
        zombie.transform.position = pos;
        zombie.transform.rotation = rot;
    }

    public IEnumerator ZombleDisable(Enemy enemy)
    {
        yield return YieldCache.WaitForSeconds(40);
        int zombieType = (int)enemy.ZombieType;
        _zombieStruct[zombieType].Pool.Enqueue(enemy.gameObject);
        enemy.gameObject.SetActive(false);
    }

}
