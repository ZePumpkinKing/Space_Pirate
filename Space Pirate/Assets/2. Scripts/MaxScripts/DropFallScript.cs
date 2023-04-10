using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFallScript : MonoBehaviour
{
    public Animator DropFall_Door;

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
        DropFall_Door.SetTrigger("Open");
    }

    private void OnTriggerExit(Collider other)
    {
        DropFall_Door.SetTrigger("Close");
    }
}
