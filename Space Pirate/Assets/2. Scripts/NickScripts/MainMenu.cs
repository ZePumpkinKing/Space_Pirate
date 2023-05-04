using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneNumber;

    public void SceneTransition()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
