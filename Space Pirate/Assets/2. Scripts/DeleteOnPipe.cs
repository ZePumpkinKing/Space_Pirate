using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnPipe : MonoBehaviour
{
    DoorScript doorScr;
    private void Start()
    {
        doorScr = GetComponent<DoorScript>();
    }
    private void OnEnable()
    {
        ActionEvents.DestroyPipeSwitchGravity += DestroyDoorScript;
    }
    private void OnDisable()
    {
        ActionEvents.DestroyPipeSwitchGravity -= DestroyDoorScript;
    }
    private void DestroyDoorScript()
    {
        Destroy(doorScr);
    }
}
