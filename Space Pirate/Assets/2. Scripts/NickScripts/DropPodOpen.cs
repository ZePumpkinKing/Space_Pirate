using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPodOpen : MonoBehaviour
{

    public Animator dropPodAnimator;

    public Collider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        dropPodAnimator.SetBool("IsInRange", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        dropPodAnimator.SetBool("IsInRange", true);

       // Debug.Log("is in trigger :D");
    }

    private void OnTriggerExit(Collider playerCollider)
    {
        dropPodAnimator.SetBool("IsInRange", false);
    }
}
