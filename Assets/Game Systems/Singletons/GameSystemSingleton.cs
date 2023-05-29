using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameSystemSingleton : MonoBehaviour
{
    public static GameSystemSingleton Instance { get; private set; }
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

    public GameObject GameSystem { get; private set; }

    private void Start()
    {
        GameSystem = GameObject.Find("Game System");
    }
}
