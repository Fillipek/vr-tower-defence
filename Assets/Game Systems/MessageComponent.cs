using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    bool dis = false;
    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled && !dis)
        {
            MapSingleton.Instance.Message.SetActive(false);
            dis= true;
        }
    }

    public void setMessage(string message)
    {
        foreach (var comp in GetComponentsInChildren<TextMeshProUGUI>())
        {
            comp.text = message;
        }
    }
}
