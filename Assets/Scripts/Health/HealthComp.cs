using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthComp : MonoBehaviour
{
    public float startingHealth = 100f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }


    public abstract void TakeDamage(float amount);



}
