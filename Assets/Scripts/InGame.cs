using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InGame;

public class InGame : MonoBehaviour
{
    public struct Rounds
    {
        public SpawnZombie[] SpawnZombies;
        public float LimitTime;
        public float WaitingTime;
    }

    public struct SpawnZombie
    {
        public ZombieType Type;
        public int SpawnCount;
    }

    [SerializeField] private Rounds[] _rounds;

    private int _enemyCount;
    private int _currentRound;
    

    private float _currentTime;


    private void Update()
    {
        _currentTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _enemyCount = ObjectPoolManager.Instance.ZombieCounting();
        Debug.Log(_enemyCount);
    }

    private void RoundStart(int round)
    {
        _currentTime = _rounds[round - 1].LimitTime;

        for(int i = 0; i < _rounds[round - 1].SpawnZombies.Length; i++)
        {
            for(int j = 0; j < _rounds[round - 1].SpawnZombies[i].SpawnCount; j++)
                ObjectPoolManager.Instance.SpawnZombie(_rounds[round - 1].SpawnZombies[i].Type, Vector3.zero, Quaternion.identity);
        }
    }

    private void RoundWait(int round)
    {
        _currentTime = _rounds[round - 1].WaitingTime;
    }

    private void RoundProceeding(int round)
    {

    }

}
