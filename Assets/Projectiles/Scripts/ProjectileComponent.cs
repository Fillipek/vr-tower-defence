using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileComponent : MonoBehaviour
{
    [SerializeField] public GameObject Target;

    [SerializeField] public float speed = 1f;

    [SerializeField] public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 prevDist = new Vector3(1000000f, 100000f, 1000000f);
    // Update is called once per frame
    void Update()
    {
        if(Target == null)
        {
            Remove();
            return;
        }
        transform.Translate(Vector3.Normalize(Target.transform.localPosition - transform.localPosition) * speed * Time.deltaTime, MapSingleton.Instance.Map.transform);
        
        var dist = Target.transform.position - transform.position;
        Debug.Log(dist.sqrMagnitude);
        if(dist.sqrMagnitude >= prevDist.sqrMagnitude)
        {
            Target.GetComponent<HealthComponent>().DealDamage(damage);
            Remove();
        }
        prevDist = dist;
    }

    void Remove()
    {
        var tempList = transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {

            DestroyImmediate(child.gameObject);
        }
        DestroyImmediate(this.gameObject);
    }
}
