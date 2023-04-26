using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunction : MonoBehaviour
{
    public void SwitchScene(int scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    public void Pause()
    {
        GameManager.instance.Pause();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManager.instance.Resume();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
