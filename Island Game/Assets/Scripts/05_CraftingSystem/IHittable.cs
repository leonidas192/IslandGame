using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable 
{
    int Health {get;}
    void GetHit(WeaponItemSO weapon, Vector3 hitpoint);
}
