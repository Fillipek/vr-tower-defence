using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class SpawnEnemySystem : MonoBehaviour
{
    [SerializeField] private float SpawnAreaStateChangeTime = 2f;
    [SerializeField] private float WallAreaStateChangeTime = 5f;
    [SerializeField] private Vector2 spawnRangeMinMax = Vector2.one;
    [SerializeField] private Wave[] Waves;


    AreaHeightComponent wallHeight = null;
    AreaHeightComponent spawnHeight = null;

    List<GameObject> spawned = new List<GameObject>();
    // Start is called before the first frame update
    private void Init()
    {
        if (wallHeight == null || spawnHeight == null)
        {
            wallHeight = MapSingleton.Instance.Wall.GetComponent<AreaHeightComponent>();
            spawnHeight = MapSingleton.Instance.Spawn.GetComponent<AreaHeightComponent>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        Init();
        foreach (var wave in Waves)
        {
            if (wave.state == Wave.State.NotSpawn && Time.realtimeSinceStartup > wave.timer)
            {
                wave.state = Wave.State.SpawnInProgress;
                StartCoroutine(StartSpawnCycle(wave));

            }
        }

    }

    private IEnumerator StartSpawnCycle(Wave wave)
    {
        PrepareForSpawn(wave);
        yield return new WaitForSeconds(SpawnAreaStateChangeTime);
        Spawn(wave);
        yield return new WaitForSeconds(SpawnAreaStateChangeTime);
        ActivateWave(wave);
        yield return new WaitForSeconds(WallAreaStateChangeTime);
        ActivateEnemies(wave);
        yield return null;
    }

    private void ActivateEnemies(Wave wave)
    {
        wave.state = Wave.State.Ended;
        foreach (var obj in spawned)
        {
            obj.GetComponent<EnemyBaseComponent>().Activate();
        }
        spawned.Clear();
    }

    private void ActivateWave(Wave wave)
    {
        wallHeight.Lower(WallAreaStateChangeTime);
    }

    private void Spawn(Wave wave)
    {
        wave.state = Wave.State.WallInProgress;
        SpawnEnemy(wave);
        spawnHeight.Lower(SpawnAreaStateChangeTime);
    }

    private void PrepareForSpawn(Wave wave)
    {
        wave.state = Wave.State.SpawnInProgress;
        wallHeight.Raise(SpawnAreaStateChangeTime);
        spawnHeight.Raise(SpawnAreaStateChangeTime);
    }


    void SpawnEnemy(Wave wave)
    {
        foreach (var enemy in wave.enemies) {
            for (var i = 0; i < enemy.number; i++)
            {

                Vector3 pos = Helper.SphericalToCartesian(UnityEngine.Random.Range(spawnRangeMinMax.y, spawnRangeMinMax.x), enemy.angle+i, 0);
                GameObject newObject = Instantiate(enemy.enemy, MapSingleton.Instance.Enemy.transform ,false);
                newObject.transform.localPosition = pos;
                spawned.Add(newObject);
            }
        }
    }

    public bool AllWaveEnded()
    {
        foreach(var wave in Waves)
        {
            if(wave.state != Wave.State.Ended) return false;
        }
        return true;
    }

    [System.Serializable]
    class Wave
    {
        public enum State
        {
            NotSpawn,
            SpawnInProgress,
            WallInProgress,
            Ended
        }

        public State state = State.NotSpawn;

        [SerializeField] public float timer = 0f;
        [SerializeField] public Enemies[] enemies;


        [System.Serializable]
        public class Enemies
        {
            [SerializeField] public float angle = 0f;
            [SerializeField] public int number = 0;
            [SerializeField] public GameObject enemy = null;

            
        }

    }
}
