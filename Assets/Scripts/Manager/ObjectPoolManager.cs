using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPoolManager : SingletonHandler<ObjectPoolManager>
{
    [Serializable]
    private struct ZombieStruct
    {
        public EnemyType Type;
        public int PoolCount;
        public GameObject Prefab;
        [HideInInspector] public Queue<GameObject> Pool;
        [HideInInspector] public Transform Parent;
    }

    [SerializeField] private GameObject BulletHolePrefab;
    [SerializeField] private int _bulletHoleCount;
    private GameObject _bulletHoleParent;
    private Queue<GameObject> _bulletHolePool;

    [SerializeField] private ZombieStruct[] _zombieStruct;


    public override void Awake()
    {
        base.Awake();
        BulletHoleObjectPooling();
        ZombieObjectPooling(EnemyType.Basic);
    }


    public void BulletHoleObjectPooling()
    {
        _bulletHolePool = new Queue<GameObject>();
        _bulletHoleParent = new GameObject("BulletHoleParent");
        _bulletHoleParent.transform.parent = transform;

        for(int i = 0, count = _bulletHoleCount; i < count; i++)
        {
            GameObject bulletHole = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            bulletHole.transform.parent = _bulletHoleParent.transform;
            _bulletHolePool.Enqueue(bulletHole);
            bulletHole.SetActive(false);
        }
    }

    private void ZombieObjectPooling(EnemyType zombieType)
    {
        string parentName = Enum.GetName(typeof(EnemyType), (int)zombieType) + " Zombie Parent======";

        _zombieStruct[(int)zombieType].Pool = new Queue<GameObject>();
        _zombieStruct[(int)zombieType].Parent = new GameObject(parentName).transform;
        _zombieStruct[(int)zombieType].Parent.transform.parent = transform;

        for (int i = 0, count = _zombieStruct[(int)zombieType].PoolCount; i < count; i++)
        {
            GameObject zombie = Instantiate(_zombieStruct[(int)zombieType].Prefab, _zombieStruct[(int)zombieType].Parent);
            _zombieStruct[(int)zombieType].Pool.Enqueue(zombie);
            zombie.gameObject.SetActive(false);
        }
    }


    public GameObject SpawnBulletHole( Vector3 pos, Quaternion rot)
    {
        GameObject bulletHole = _bulletHolePool.Dequeue();

        bulletHole.SetActive(false);
        bulletHole.SetActive(true);
        bulletHole.transform.position = pos;
        bulletHole.transform.rotation = rot;
        _bulletHolePool.Enqueue(bulletHole);
        return bulletHole;
    }



    public void SpawnZombie(EnemyType zombieType, Vector3 pos, Quaternion rot)
    {
        GameObject zombie;
        if(_zombieStruct[(int)zombieType].Pool.Count == 0)
        {
            zombie = Instantiate(_zombieStruct[(int)zombieType].Prefab, _zombieStruct[(int)zombieType].Parent);
            _zombieStruct[(int)zombieType].Pool.Enqueue(zombie);
            zombie.gameObject.SetActive(false);
            return;
        }

        zombie = _zombieStruct[(int)zombieType].Pool.Dequeue();
        zombie.gameObject.SetActive(true);
        Enemy enemy = zombie.GetComponent<Enemy>();
        enemy.Navmesh.NaveMeshEnabled(false);
        zombie.transform.position = pos;
        zombie.transform.rotation = rot;
        enemy.Navmesh.NaveMeshEnabled(true);
        enemy.Target = GameManager.Instance.Player.transform;
    }

    public IEnumerator DisableZombie(Enemy enemy) 
    {
        yield return YieldCache.WaitForSeconds(40);
        int zombieType = (int)enemy.EnemyType;
        _zombieStruct[zombieType].Pool.Enqueue(enemy.gameObject);
        enemy.gameObject.SetActive(false);
    }

    public int ZombieCounting()
    {
        int zombieCount = 0;
        for(int i = 0, count = (int)EnemyType.Count - 1; i <  count; i++)
        {
            Enemy[] objs = _zombieStruct[i].Parent.GetComponentsInChildren<Enemy>();
            zombieCount += objs.Length;

            foreach(var obj in objs)
            {
                if (obj.IsDead)
                    zombieCount--;
            }
        }
        return zombieCount;
    }

}
