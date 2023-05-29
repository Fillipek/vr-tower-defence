using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MapSingleton : MonoBehaviour
{
    public static MapSingleton Instance { get; private set; }
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

    public GameObject Map { get; private set; }
    public GameObject Enemy { get; private set; }
    public GameObject Towers { get; private set; }
    public GameObject Spawn { get; private set; }
    public GameObject Wall { get; private set; }

    private void Start()
    {
        Map = GameObject.Find("Map");
        Enemy = Map.GetNamedChild("Enemy");
        Towers = Map.GetNamedChild("Towers");
        Wall = Map.GetNamedChild("wall-area");
        Spawn = Map.GetNamedChild("spawn-area");
    }
 
}
