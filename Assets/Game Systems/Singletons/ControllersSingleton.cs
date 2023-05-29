using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersSingleton : MonoBehaviour
{
    public static ControllersSingleton Instance { get; private set; }
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

    public GameObject LeftController { get; private set; }
    public GameObject RightController { get; private set; }

    private void Start()
    {
        LeftController = GameObject.Find("LeftHand Controller");
        RightController = GameObject.Find("RightHand Controller");
    }
}
