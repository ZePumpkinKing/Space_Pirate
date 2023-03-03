using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamingVolume : MonoBehaviour
{
    [SerializeField] private List<GameObject> objsInVolume;

    private void Start()
    {
        StartCoroutine(BufferLoad());
    }
    private void OnTriggerEnter(Collider other)
    {
        ProcessCollision(other.gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EnableObjects();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromList(other.gameObject);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DisableObjects();
        }
    }

    private void ProcessCollision(GameObject obj)
    {
        if (obj.layer != LayerMask.NameToLayer("Player")
            && obj.layer != LayerMask.NameToLayer("UI")
            && obj.layer != LayerMask.NameToLayer("Weapons")
            && obj.CompareTag("Streaming") == false)
        {
            if (!objsInVolume.Contains(obj))
            {
                objsInVolume.Add(obj);
            }
        }
    }
    private void DisableObjects()
    {
        foreach (GameObject obj in objsInVolume)
        {
            obj.SetActive(false);
        }
    }
    private void EnableObjects()
    {
        foreach (GameObject obj in objsInVolume)
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
            }
        }
    }

    private void RemoveFromList(GameObject obj)
    {
        if (this.objsInVolume.Contains(obj))
        {
            this.objsInVolume.Remove(obj);
        }
    }

    private IEnumerator BufferLoad()
    {
        yield return new WaitForSeconds(.001f);
        
        DisableObjects();
    }
}
