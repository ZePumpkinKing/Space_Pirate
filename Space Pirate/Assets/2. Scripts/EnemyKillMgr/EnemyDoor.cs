using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    public float numOfEnemiesToOpenDoor;
    public void OpenDoor()
    {
        Destroy(gameObject);
    }
}
