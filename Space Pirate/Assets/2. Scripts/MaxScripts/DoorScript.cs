using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator DoorController;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        DoorController.SetTrigger("Open");
        DoorController.SetBool("HasOpened", true);
    }

    private void OnTriggerExit(Collider other)
    {
        DoorController.SetTrigger("Close");
        StartCoroutine(DoorReset());
    }

    private IEnumerator DoorReset()
    {
        yield return new WaitForSeconds(0.958f);
        DoorController.SetBool("HasOpened", false);
    }
}
