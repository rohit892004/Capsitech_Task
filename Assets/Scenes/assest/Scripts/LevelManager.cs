using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void RetryLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        int nextScene =
            SceneManager.GetActiveScene().buildIndex + 1;

        if(nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }
}