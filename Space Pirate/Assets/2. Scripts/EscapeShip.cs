using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeShip : MonoBehaviour
{
    Input input;
    [SerializeField] GameObject text;
    bool inTrigger;
    bool finishedGame;

    private void Awake()
    {
        input = new Input();
        input.Gameplay.Interact.performed += context => SwitchScene();
    }
    private void OnEnable()
    {
        input.Enable();
        ActionEvents.DestroyedTesseract += EnableFunctionality;
    }
    private void OnDisable()
    {
        input.Disable();
        ActionEvents.DestroyedTesseract -= EnableFunctionality;
    }

    private void EnableFunctionality()
    {
        text.SetActive(true);
        finishedGame = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        inTrigger = true;
    }

    private void SwitchScene()
    {
        if (inTrigger && finishedGame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
