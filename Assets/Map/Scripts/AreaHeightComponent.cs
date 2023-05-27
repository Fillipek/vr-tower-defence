using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AreaHeightComponent : MonoBehaviour
{

    float height = 1.0f;
    float speed = 1.0f;

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

    public float getHeightAcc()
    {
        return heightAcc;
    }

    public void Raise(float time)
    {
        height = 1f;
        speed = 1 / time;
    }
    public void Lower(float time)
    {
        height = 0f;
        speed = 1 / time;
    }
    public void SetHeight(float newHeight, float time) {
        height = newHeight;
        speed = 1 / time;
    }
}
