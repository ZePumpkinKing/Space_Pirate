using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Input input;

    private void Awake()
    {
        input = new Input();
        input.Gameplay.RestartScene.performed += context => RestartScene();
    }

    private void RestartScene()
    {
        Debug.Log("test");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
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
