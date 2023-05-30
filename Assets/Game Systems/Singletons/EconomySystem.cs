using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomySystem: MonoBehaviour
{
    public static EconomySystem Instance { get; private set; }
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

    [SerializeField] private int numberOfCoins = 500;

    public int getNumberOfCoins()
    {
        return numberOfCoins;
    }

    public bool Buy(int amount)
    {
        if(numberOfCoins >= amount)
        {
            numberOfCoins -= amount;
            return true;
        }
        return false;
    }
    public bool Sell(int amount)
    {
        numberOfCoins += amount;
        return true;
    }

    public bool Return(int amount)
    {
        numberOfCoins += amount;
        return true;
    }

    public bool Reward(int amount)
    {
        numberOfCoins += amount;
        return true;
    }
}
