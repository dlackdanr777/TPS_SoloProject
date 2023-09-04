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
    private GameObject[] _bulletHolePool;


    private void Start()
    {
        ObjectPooling();
    }

    private void ObjectPooling()
    {
        _bulletHolePool = new GameObject[_bulletHoleCount];
        _bulletHoleParent = new GameObject("BulletHoleParent");

        for (int i = 0, count = _bulletHoleCount; i < count; i++) 
        {
            _bulletHolePool[i] = Instantiate(BulletHolePrefab, Vector3.zero, Quaternion.identity);
            _bulletHolePool[i].transform.parent = _bulletHoleParent.transform;
            _bulletHolePool[i].SetActive(false);
        }
    }

    public void UseObjectPool(ObjectPoolType type, Vector3 pos, Quaternion rot)
    {
        if (type == ObjectPoolType.Bullet)
        {
            UseObjectArray(ref _bulletHolePool, pos, rot);
        }
    }

    private void UseObjectArray(ref GameObject[] objArray, Vector3 pos, Quaternion rot)
    {
        for (int i = 0, count = objArray.Length; i < count; i++)
        {
            if (!objArray[i].activeSelf)
            {
                objArray[i].SetActive(true);
                objArray[i].transform.position = pos;
                objArray[i].transform.rotation = rot;
                break;
            }
        }
    }
}
