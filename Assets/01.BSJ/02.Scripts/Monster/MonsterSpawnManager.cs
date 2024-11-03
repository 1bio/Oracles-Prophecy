using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    // Monster Wave
    [SerializeField] private int _currentWave;
    [SerializeField] private GameObject[] _firstWaveMonsters;
    [SerializeField] private GameObject[] _secondWaveMonsters;
    [SerializeField] private GameObject[] _thirdWaveMonsters;
    private GameObject[][] _monsterWaves;

    // Spawn Point
    [SerializeField] private GameObject _player;
    private PointGrid _pointGrid;
    private HashSet<PointNode> _neighborNodes = new HashSet<PointNode>();

    private bool _isFirstSpawn = true;


    private void Awake()
    {
        _currentWave = 0;

        _monsterWaves = new GameObject[][] { _firstWaveMonsters, _secondWaveMonsters, _thirdWaveMonsters };

        _pointGrid = FindObjectOfType<PointGrid>();
    }

    private void Start()
    {
        foreach (GameObject[] wave in _monsterWaves)
        {
            foreach (GameObject monster in wave)
            {
                if (monster != null)
                    monster.SetActive(false);
            }
        }
    }


    private void Update()
    {
        if (!_isFirstSpawn && _currentWave < _monsterWaves.Length && IsWaveCleared(_monsterWaves[_currentWave]))
            _currentWave++;

        SpawnMonsterWave();

        _isFirstSpawn = false;
    }

    private void SpawnMonsterWave()
    {
        if (_currentWave < _monsterWaves.Length && _monsterWaves[_currentWave].Length > 0 && IsWaveCleared(_monsterWaves[_currentWave]))
        {
            OnSetActiveMonsters();
        }
    }

    private void OnSetActiveMonsters()
    {
        foreach (GameObject monster in _monsterWaves[_currentWave])
        {
            if (monster != null)
            {
                monster.SetActive(true);
                monster.transform.position = FindRandomSpawnPoint();
            }
        }
    }

    private bool IsWaveCleared(GameObject[] monsterWave)
    {
        foreach (GameObject monster in monsterWave)
        {
            if (monster != null && monster.activeSelf)
                return false;
        }
        return true;
    }

    private Vector3 FindRandomSpawnPoint()
    {
        if (_player == null)
            return Vector3.zero;

        _neighborNodes.Clear();

        List<PointNode> nodes = new List<PointNode>();

        PointNode currentNode = _pointGrid.GetPointNodeFromGridByPosition(_player.transform.position);

        nodes.AddRange(_pointGrid.GetNeighborNodes(currentNode));

        Queue<PointNode> nodesToCheck = new Queue<PointNode>();
        nodesToCheck.Enqueue(currentNode);

        int depth = 3;
        while (nodesToCheck.Count > 0 && depth > 0)
        {
            int count = nodesToCheck.Count;
            depth--;

            for (int i = 0; i < count; i++)
            {
                PointNode node = nodesToCheck.Dequeue();
                if (_neighborNodes.Add(node))
                {
                    foreach (PointNode neighbor in _pointGrid.GetNeighborNodes(node))
                    {
                        nodesToCheck.Enqueue(neighbor);
                    }
                }
            }
        }

        int randNum = Random.Range(0, _neighborNodes.Count);

        PointNode[] neighborArray = new PointNode[_neighborNodes.Count];
        _neighborNodes.CopyTo(neighborArray);

        return neighborArray[randNum].Position;
    }
}
