using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class EndGameSystem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(MapSingleton.Instance.MainCastle == null)
        {
            Defeat();
            Time.timeScale = 0f;
        }
        if(GetComponent<SpawnEnemySystem>().AllWaveEnded() && MapSingleton.Instance.Enemy.transform.childCount == 0)
        {
            Victory();
            Time.timeScale = 0f;
        }
       
    }

    public void Defeat() {
        Debug.Log("Defeat");
        MapSingleton.Instance.Message.SetActive(true);
        MapSingleton.Instance.Message.GetComponent<MessageComponent>().setMessage("Defeat");
    }
    public void Victory()
    {
        Debug.Log("Victory");
        MapSingleton.Instance.Message.SetActive(true);
        MapSingleton.Instance.Message.GetComponent<MessageComponent>().setMessage("Victory");
    }
}
