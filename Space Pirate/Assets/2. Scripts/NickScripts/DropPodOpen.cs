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

    float timerCap = 2.5f;
    float timer;
    bool inTrigger;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerCap;
        dropPodText.SetActive(false);
        dropPodAnimator.SetBool("IsInRange", false);
    }
    private void Update()
    {
        if (inTrigger)
        {
            timer -= Time.deltaTime;
        }
        if (inTrigger && timer < 0)
        {
            dropPodText.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            inTrigger = true;
            dropPodAnimator.SetBool("IsInRange", true);
        }
    }
    private void OnTriggerExit(Collider playerCollider)
    {
        timer = timerCap;
        dropPodText.SetActive(false);
        dropPodAnimator.SetBool("IsInRange", false);
    }


    
}
