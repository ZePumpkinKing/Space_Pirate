using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DropPodOpen : MonoBehaviour
{

    public Animator dropPodAnimator;

    public Collider playerCollider;

    public Camera swapCam; 

    public Camera playerDefaultCam;

    private bool entered;

    Player player;

    Input input;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        input.Gameplay.Interact.performed += context => CameraSwap();
        input = new Input();
        swapCam.enabled = false;
        playerDefaultCam.enabled = true;
        dropPodAnimator.SetBool("IsInRange", false);
    }
    private void OnEnable() 
    {
        input.Enable();
    }

    private void OnDisable() 
    {
        input.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider playerCollider)
    {
        dropPodAnimator.SetBool("IsInRange", true);

        entered = true;

       // Debug.Log("is in trigger :D");
    }
    private void OnTriggerExit(Collider playerCollider)
    {
        entered = false;
        dropPodAnimator.SetBool("IsInRange", false);
    }


    private void CameraSwap()
    {
        if (entered)
        {
            playerDefaultCam.enabled = false;
            Destroy(player.gameObject);
            Debug.Log("Camera Swap");
            swapCam.enabled = true;
        }
        
    }

    
}
