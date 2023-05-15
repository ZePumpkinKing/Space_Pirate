using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTesseract : MonoBehaviour, IDamageable
{
    int destroyedEmitters;
    bool destructible;
    public void TakeDamage(float dmg)
    {
        if (destructible)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateEmitterCount()
    {
        destroyedEmitters++;
        if (destroyedEmitters == 4)
        {
            destructible = true;
        }
    }

    private void OnEnable()
    {
        ActionEvents.DestroyedEmitter += UpdateEmitterCount;
    }
    private void OnDisable()
    {
        ActionEvents.DestroyedEmitter -= UpdateEmitterCount;
    }

}
