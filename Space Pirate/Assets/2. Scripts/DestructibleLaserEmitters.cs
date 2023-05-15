using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleLaserEmitters : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject explosion;
    bool forcefieldsDestroyed;
    public void TakeDamage(float dmg)
    {
        if (forcefieldsDestroyed)
        {
            Destroy(this.gameObject);
            Debug.Log("if statement accessed");
            Instantiate(explosion, transform.position, transform.rotation);
            ActionEvents.DestroyedEmitter();
        }

    }


    private void OnEnable()
    {
        ActionEvents.DestroyedForcefields += UpdateForceFieldsDestroyedBool;
    }

    private void OnDisable()
    {
        ActionEvents.DestroyedForcefields -= UpdateForceFieldsDestroyedBool;
    }

    private void UpdateForceFieldsDestroyedBool()
    {
        Debug.Log("UpdateForceFieldsDestroyedBool");
        forcefieldsDestroyed = true;
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }

}
