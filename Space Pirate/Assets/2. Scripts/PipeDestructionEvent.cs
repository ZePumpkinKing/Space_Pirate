using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeDestructionEvent : MonoBehaviour
{
    private void OnDestroy()
    {
        ActionEvents.DestroyPipeSwitchGravity();
    }
}
