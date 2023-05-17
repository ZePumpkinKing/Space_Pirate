using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDestructionEvent : MonoBehaviour, IDamageable
{
    public void TakeDamage(float dmg)
    {
        Debug.Log("DESTROY DESTROY DESTROY");
        ActionEvents.DestroyPipeSwitchGravity();
        Destroy(gameObject);
    }
   
}
