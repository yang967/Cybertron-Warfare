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

    public void RefreshCharacterBar(int team)
    {
        MainMenu.instance.RefreshCharacterBar(team);
    }

    public void RefreshSkill(int skill)
    {
        if (MainMenu.instance == null)
            return;

        MainMenu.instance.RefreshSkillDescription(skill);
    }
}
