using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class SpawnEnemySystem : MonoBehaviour
{
    [SerializeField] private float SpawnAreaStateChangeTime = 2f;
    [SerializeField] private float WallAreaStateChangeTime = 5f;
    [SerializeField] private GameObject Map;
    [SerializeField] private Wave[] Waves;


    AreaHeightComponent wallHeight = null;
    AreaHeightComponent spawnHeight = null;
    // Start is called before the first frame update
    void Start()
    {
  
    }


    float timerSpawn = 0f;
    float timerWall = 0f;
    // Update is called once per frame
    void Update()
    {
        setAreaComponentWhenReady();
        foreach (var wave in Waves)
        {
            if (wave.state == Wave.State.NotSpawn && Time.realtimeSinceStartup > wave.timer)
            {
                PrepareForSpawn(wave);
                timerSpawn = SpawnAreaStateChangeTime;

            }
            if (wave.state == Wave.State.SpawnInProgress)
            {
                timerSpawn -= Time.deltaTime;
                if (timerSpawn < 0)
                {
                    Spawn(wave);
                    timerWall = WallAreaStateChangeTime;

                }
            }
            if (wave.state == Wave.State.WallInProgress)
            {
                timerWall -= Time.deltaTime;
                if (timerWall < 0)
                {
                    ActivateWave(wave);

                }
            }
        }

    }

    private void ActivateWave(Wave wave)
    {
        wave.state = Wave.State.Ended;
        wallHeight.Lower(SpawnAreaStateChangeTime);
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

    private void setAreaComponentWhenReady()
    {
        if (wallHeight == null || spawnHeight == null)
        {
           
            wallHeight = Map.GetNamedChild("wall-area").GetComponent<AreaHeightComponent>();
            spawnHeight = Map.GetNamedChild("spawn-area").GetComponent<AreaHeightComponent>();
        }
    }

    void SpawnEnemy(Wave wave)
    {
        GameObject EnemyGameObject = Map.GetNamedChild("Enemy");
        foreach (var enemy in wave.enemies) {
            for (var i = 0; i < enemy.number; i++)
            {

                Vector3 pos = Helper.SphericalToCartesian(UnityEngine.Random.Range(.85f, .95f), enemy.angle+i, 0);
                GameObject newObject = Instantiate(enemy.enemy, EnemyGameObject.transform ,false);
                newObject.transform.localPosition = pos;
            }
        }
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
