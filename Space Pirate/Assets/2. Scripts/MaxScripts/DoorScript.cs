using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Animator DoorController;
    [SerializeField] private BoxCollider coll;
    public bool exiting;
    public bool opening;

    private void Update()
    {
        DoorController.SetBool("exited", exiting);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!opening) StartCoroutine(OpenDoor());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!exiting) StartCoroutine(CloseDoor());
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitUntil(() => !exiting);
        opening = true;
        DoorController.SetTrigger("Open");
        yield return new WaitForSeconds(.5f);
        coll.isTrigger = true;
        yield return new WaitForSeconds(.5f);
        opening = false;
    }
    IEnumerator CloseDoor()
    {
        yield return new WaitUntil(() => !opening);
        exiting = true;
        coll.isTrigger = false;
        DoorController.SetTrigger("Close");
        yield return new WaitForSeconds(1);
        exiting = false;
    }

    

}
