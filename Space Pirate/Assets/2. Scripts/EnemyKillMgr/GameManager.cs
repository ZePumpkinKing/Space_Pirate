using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Input input;
    public bool paused { get; private set; }

    private void Awake()
    {
        input = new Input();
        input.Gameplay.RestartScene.performed += context => PauseGame();
    }

    private void PauseGame()
    {
        if (paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            paused = false;
            return;
        }
        
        if (!paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            paused = true;
        }
        
        
    }



    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
