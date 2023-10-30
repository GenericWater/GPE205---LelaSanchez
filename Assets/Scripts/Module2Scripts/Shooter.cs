using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    // Start is called before the first frame update
    public abstract void Start();

    // Update is called once per frame
    public abstract void Update();

    public abstract void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan);
}
