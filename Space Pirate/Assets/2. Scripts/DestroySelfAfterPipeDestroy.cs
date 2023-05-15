using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfAfterPipeDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        ActionEvents.DestroyPipeSwitchGravity += DestroySelf;
    }
    private void OnDisable()
    {
       ActionEvents.DestroyPipeSwitchGravity -= DestroySelf;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
