using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int sceneNumber;

    public void SceneTransition()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
