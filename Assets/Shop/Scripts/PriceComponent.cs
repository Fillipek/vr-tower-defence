using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceComponent : MonoBehaviour
{
    [SerializeField] int price = 100;


    bool Solded = false;
    // Start is called before the first frame update

    public bool Buy()
    {
        return EconomySystem.Instance.Buy(price);
    }

    public bool Sell()
    {
        if (!Solded) { 
            Solded = true;
            return EconomySystem.Instance.Sell(price);
        }
        else
        {
            return false;
        }
    }

    public bool Return()
    {
        if (!Solded)
        {
            Solded = true;
            return EconomySystem.Instance.Return(price);
        }
        else
        {
            return false;
        }
    }
}
