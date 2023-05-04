using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    //refs
    [SerializeField] private Animator DoorController;
    [SerializeField] private BoxCollider coll;
    
    //publics
    public float enemiesDestroyed { get; private set; }
    
    public bool opening;
    [SerializeField] private bool enteredRoom;
    public float numOfEnemiesToOpenDoor;

    [HideInInspector] public int enemiesKilled;

    private void Update()
    {
        if (enemiesDestroyed >= numOfEnemiesToOpenDoor && !opening)
        {
            StartCoroutine(OpenDoor());
        }
    }
    public IEnumerator OpenDoor()
    {
        opening = true;
        DoorController.SetTrigger("Open");
        yield return new WaitForSeconds(.5f);
        Destroy(coll);
        yield return new WaitForSeconds(.5f);
        enemiesDestroyed = 0;
    }
    public void UpdateEnemyCount()
    {
        if (enteredRoom)
        {
            enemiesDestroyed++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!enteredRoom && other.CompareTag("Player"))
        {
            enteredRoom = true;
        }
    }
}
