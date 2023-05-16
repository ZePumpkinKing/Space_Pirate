using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : MonoBehaviour
{
    //refs
    [SerializeField] private Animator DoorController;
    [SerializeField] private BoxCollider coll;
    VoiceLinePlayer audioBank;
    
    //publics
    public float enemiesDestroyed { get; private set; } 
    
    [HideInInspector] public bool opening;
    private bool enteredRoom;
    [SerializeField] bool playVoiceLineOnOpen;
    [SerializeField] int voiceLineNum;
    [SerializeField] int voiceLineEnemiesKilled;
    public float numOfEnemiesToOpenDoor;

    [HideInInspector] public int enemiesKilled;
    //privs
    private bool finalDoor;
    private GameObject[] fields;
    AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioBank = FindObjectOfType<VoiceLinePlayer>();
    }

    private void Update()
    {
        if (enemiesDestroyed >= numOfEnemiesToOpenDoor && !opening)
        {
            if (!finalDoor) StartCoroutine(OpenDoor());
            else
            {
                DestroyForcefields();
                Destroy(this.gameObject);
            }
        }
    }
    private void DestroyForcefields()
    {
        ActionEvents.DestroyedForcefields();
        audioBank.PlayVoiceLine(voiceLineEnemiesKilled);
        fields = GameObject.FindGameObjectsWithTag("Forcefield");
        foreach (GameObject forcefield in fields)
        {
            Destroy(forcefield);
        }
        
    }
    public IEnumerator OpenDoor()
    {
        ActionEvents.ExitBattleRoom();
        if (playVoiceLineOnOpen)
        {
            audioBank.PlayVoiceLine(voiceLineNum);
        }
        audioSrc.Play();
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
            ActionEvents.EnteredBattleRoom();
            
            if (gameObject.CompareTag("FinalRoom"))
            {
                finalDoor = true;
            }
        }
    }

   
}
