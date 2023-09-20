using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    [Serializable]
    public struct Rounds
    {
        public SpawnZombie[] SpawnZombies;
        public float LimitTime;
        public float WaitingTime;
    }

    [Serializable]
    public struct SpawnZombie
    {
        public ZombieType Type;
        public int SpawnCount;
    }

    public enum RoundType { Wait, Start, Proceeding, End }


    [SerializeField] private UIGame _uiGame;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private Rounds[] _rounds;
    
    private RoundType _roundType;
    private int _enemyCount;
    private int _currentRound;
    

    private float _currentTime;

    private bool _roundClear;

    private void Awake()
    {
        GameManager.Instance.IsGameEnd = false;
    }

    public void Start()
    {
        _currentRound = 1;
        _roundType = RoundType.Wait;
    }


    private void OnDisable()
    {
        _uiGame.SetZombieCountText("�б��� �����Ͻʽÿ�");
    }


    private void Update()
    {
        if (!_roundClear)
        {
            RoundTimer();
            RoundUpdate();
        }
        else
        {
            RoundClear();
        }
    }

    private void FixedUpdate()
    {
        if(_roundType == RoundType.Proceeding)
        {
            _enemyCount = ObjectPoolManager.Instance.ZombieCounting();
            _uiGame.SetZombieCountText("���� ���� ��: " + _enemyCount);

            if (_enemyCount <= 0)
            {
                _roundClear = true;
            }
                
        }
    }

    private void RoundTimer()
    {
        
        _currentTime -= Time.deltaTime;
        _uiGame.SetGameTimerText(Enum.GetName(typeof(RoundType), _roundType) +": " + Mathf.Floor(_currentTime * 100f) / 100f);
        if (_currentTime <= 0)
            _isEndTime = true;
    }

    private bool _isEndTime;
    private void RoundUpdate()
    {
        if (_isEndTime)
        {
            switch (_roundType)
            {
                case RoundType.Wait:
                    RoundWait(_currentRound);
                    break;
                case RoundType.Start:
                    RoundStart(_currentRound);
                    break;
                case RoundType.Proceeding:
                    RoundProceeding(_currentRound);
                    break;
            }
            _roundType++;
            _roundType = (RoundType)((int)_roundType % 3);

            _isEndTime = false;
        }
    }


    private void RoundStart(int round)
    {
        _currentTime = _rounds[round - 1].LimitTime;

        for(int i = 0; i < _rounds[round - 1].SpawnZombies.Length; i++)
        {
            for(int j = 0; j < _rounds[round - 1].SpawnZombies[i].SpawnCount; j++)
                ObjectPoolManager.Instance.SpawnZombie(_rounds[round - 1].SpawnZombies[i].Type, _spawnPos.position, Quaternion.Euler(0,180,0));

        }
    }

    private void RoundWait(int round)
    {
        _uiGame.SetZombieCountText("�� ���� �����ɴϴ�...");
        _currentTime = _rounds[round - 1].WaitingTime;
    }

    private void RoundProceeding(int round)
    {
        if(_enemyCount >= 0)
        {
            GameManager.Instance.GameEnd();
            //���ӿ���
        }
    }

    private float _clearWaitTime;
    private void RoundClear()
    {
        _clearWaitTime += Time.deltaTime;
        if (_clearWaitTime > 5)
        {
            
            if(_rounds.Length > _currentRound)
            {
                _clearWaitTime = 0;
                _roundType = RoundType.Wait;
                _currentRound++;
                _isEndTime = true;
                _roundClear = false;
            }
            else
            {
                GameManager.Instance.GameEnd();
                _uiGame.ShowUIWin();
            }

        }
    }
}
