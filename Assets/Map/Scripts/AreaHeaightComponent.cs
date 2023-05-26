using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AreaHeaightComponent : MonoBehaviour
{

    [SerializeField] float height = 1.0f;
    [SerializeField] float speed = 1.0f;

    float heightAcc = 1.0f;

    float scale = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.y; 
    }

    // Update is called once per frame
    void Update()
    {
        var move = speed * Time.deltaTime * math.sign(height- heightAcc);

        if(math.abs(height - heightAcc) > 0.01)
        {
            heightAcc += move;

            heightAcc = math.clamp(heightAcc, 0.0f, 1.0f);

            transform.localScale = new Vector3(transform.localScale.x, scale * heightAcc, transform.localScale.z);
        }
    }

    public void Raise()
    {
        height = 1f;
    }
    public void Lower()
    {
        height = 0f;
    }
    public void SetHeight(float newHeight) {
        height = newHeight;
    }
}
