using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MapSingleton : MonoBehaviour
{
    public static MapSingleton Instance;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] public GameObject Map;
    [SerializeField] public GameObject Enemy;
    [SerializeField] public GameObject Towers;
    [SerializeField] public GameObject Spawn;
    [SerializeField] public GameObject Wall;
    [SerializeField] public GameObject MainCastle;
    [SerializeField] public GameObject Message;
    private void Start()
    {
        //Map = GameObject.Find("Map");
        //Enemy = Map.GetNamedChild("Enemy");
        //Towers = Map.GetNamedChild("Towers");
        //Wall = Map.GetNamedChild("wall-area");
        //Spawn = Map.GetNamedChild("spawn-area");
        //MainCastle = Map.GetNamedChild("MainCastle");
        //Message = Map.GetNamedChild("Message");
    }
 
}
