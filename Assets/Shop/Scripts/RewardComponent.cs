using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardComponent : MonoBehaviour
{
    [SerializeField] int amount = 10;


    private void OnDestroy()
    {
        EconomySystem.Instance.Reward(amount);
    }

    
}
