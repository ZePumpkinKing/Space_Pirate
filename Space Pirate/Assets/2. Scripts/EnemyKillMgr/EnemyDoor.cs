using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    [SerializeField] private Animator DoorController;
    [SerializeField] private BoxCollider coll;
    public bool exiting;
    public bool opening;

    public float numOfEnemiesToOpenDoor;

    public IEnumerator OpenDoor()
    {
        yield return new WaitUntil(() => !exiting);
        opening = true;
        DoorController.SetTrigger("Open");
        yield return new WaitForSeconds(.5f);
        coll.isTrigger = true;
        yield return new WaitForSeconds(.5f);
    }
}
