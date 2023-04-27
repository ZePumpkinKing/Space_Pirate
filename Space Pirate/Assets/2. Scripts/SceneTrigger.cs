using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTrigger : MonoBehaviour
{
    Input input;
    private bool inTrigger;
    public int sceneId;
    private void Awake()
    {
        input = new Input();
        input.Gameplay.Interact.performed += context => SwitchScene();
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void SwitchScene()
    {
        if (inTrigger)
        {
            SceneManager.LoadScene(sceneId);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
