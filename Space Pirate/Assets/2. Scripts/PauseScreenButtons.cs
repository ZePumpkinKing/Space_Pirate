using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenButtons : MonoBehaviour
{
    public void Continue()
    {
        ActionEvents.OnPause();
    }

    public void Restart()
    {
        ActionEvents.OnPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        ActionEvents.OnPause();
        SceneManager.LoadScene(0);
    }


}
