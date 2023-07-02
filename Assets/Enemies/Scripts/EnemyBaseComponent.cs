using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBaseComponent : MonoBehaviour
{
    public enum State
    {
        Spawned,
        Active
    }
    public State state = State.Spawned;
    [SerializeField] float speed = 1f;
    [SerializeField] int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive())
        {
            transform.Translate(
                Vector3.Normalize(MapSingleton.Instance.Enemy.transform.localPosition - transform.localPosition) * speed * Time.deltaTime, 
                MapSingleton.Instance.Map.transform
            );
            Vector2 lookTarget = MapSingleton.Instance.Map.transform.position;
            lookTarget.y = 0f;
            transform.LookAt(lookTarget);
            if (transform.localPosition.magnitude < 0.12)
            {
                if (MapSingleton.Instance.MainCastle != null)
                {
                    MapSingleton.Instance.MainCastle.GetComponent<HealthComponent>().DealDamage(damage);
                    GetComponent<HealthComponent>().DealDamage(100000);
                }
            }
        }
    }

    public bool isActive()
    {
        return state == State.Active;
    }

    public void Activate()
    {
        state = State.Active;
    }
}
