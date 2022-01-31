using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _chunksToInstantiate;
    private Transform _carTransform;
    
    private const float ChunkLength = 50f;
    private float _spawnZ = -50f;
    private float _safeZone = 100f;
    private int _amountOfChunksVisible = 15;
    private int _lastChunkIndex = 0;

    private List<GameObject> _activeChunks;
    
    private void Start()
    {
        SetVarieblesOnStart();
    }
    
    private void Update()
    {
        if (_carTransform.position.z - _safeZone > (_spawnZ - _amountOfChunksVisible * ChunkLength))
        {
            SpawnChunk();
            DeleteTile();
        }
    }
    
    private void SpawnChunk(int chunkIndex = -1)
    {
        GameObject chunkToSpawn;
        if (chunkIndex == -1)
        {
            chunkToSpawn = Instantiate(_chunksToInstantiate[RandomChunkIndex()]);
        }
        else
        {
            chunkToSpawn = Instantiate(_chunksToInstantiate[chunkIndex]);
        }
        chunkToSpawn.transform.SetParent(transform);
        chunkToSpawn.transform.position = Vector3.forward * _spawnZ;
        _spawnZ += ChunkLength;
        _activeChunks.Add(chunkToSpawn);
    }

    private void DeleteTile()
    {
        Destroy(_activeChunks[0]);
        _activeChunks.RemoveAt(0);
    }

    private int RandomChunkIndex()
    {
        if (_chunksToInstantiate.Length <= 1)
            return 0;

        int randomIndex = _lastChunkIndex;
        while (randomIndex == _lastChunkIndex)
        {
            randomIndex = Random.Range(0, _chunksToInstantiate.Length);
        }

        _lastChunkIndex = randomIndex;
        return randomIndex;
    }

    private void SetVarieblesOnStart()
    {
        _activeChunks = new List<GameObject>();
        _carTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < _amountOfChunksVisible; i++)
        {
            if (i < 1)
            {
                SpawnChunk(0);
            }
            SpawnChunk();
        }
    }
}
