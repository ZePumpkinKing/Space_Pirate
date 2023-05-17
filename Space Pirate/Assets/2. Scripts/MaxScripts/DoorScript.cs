using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Animator DoorController;
    [SerializeField] private BoxCollider coll;
    AudioSource source;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;
    public bool exiting;
    public bool opening;
    bool open;
    bool closed;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void Update()
    {
        DoorController.SetBool("exited", exiting);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!opening) StartCoroutine(OpenDoor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!exiting) StartCoroutine(CloseDoor());
        }    
    }
    IEnumerator OpenDoor()
    {
        yield return new WaitUntil(() => !exiting);
        yield return new WaitUntil(() => !opening);
        if (!open)
        {
            opening = true;
            DoorController.SetTrigger("Open");
            source.time = 0.2f;
            source.PlayOneShot(doorOpen);
            yield return new WaitForSeconds(.5f);
            coll.isTrigger = true;
            yield return new WaitForSeconds(.5f);
            opening = false;
            open = true;
            closed = false;
        }
        
    }
    IEnumerator CloseDoor()
    {
        yield return new WaitUntil(() => !opening);
        yield return new WaitUntil(() => !exiting);
        if (!closed)
        {
            
            exiting = true;
            coll.isTrigger = false;
            //source.PlayOneShot(doorClose);
            DoorController.SetTrigger("Close");
            yield return new WaitForSeconds(1);
            exiting = false;
            closed = true;
            open = false;
        }
        
    }
}
