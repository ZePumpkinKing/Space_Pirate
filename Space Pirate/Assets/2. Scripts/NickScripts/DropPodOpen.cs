using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DropPodOpen : MonoBehaviour
{

    public Animator dropPodAnimator;

    public Collider playerCollider;

    public GameObject dropPodText;

    // Start is called before the first frame update
    void Start()
    {
        dropPodText.SetActive(false);
        dropPodAnimator.SetBool("IsInRange", false);
    }


    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            dropPodText.SetActive(true);
            dropPodAnimator.SetBool("IsInRange", true);
        }
        
       // Debug.Log("is in trigger :D");
    }
    private void OnTriggerExit(Collider playerCollider)
    {
        dropPodText.SetActive(false);
        dropPodAnimator.SetBool("IsInRange", false);
    }


    
}
