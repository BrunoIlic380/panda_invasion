using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //needed to load the level from the main menu

public class ButtonFunctionality : MonoBehaviour
{
    [Tooltip("only the 'instructionsScreen needs this reference, it can be null in other instances")]
    public GameObject instructions;

    public void NewGame()
    {
        SceneManager.LoadScene(1);  //scenes are indexed the same was they are indexed in the build settings
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameManagerScript manager = FindObjectOfType<GameManagerScript>();
        manager.pauseScreen.SetActive(false);
    }

    public void OpenCloseInstructions(bool flag)
    {

        if (flag == true)
        {
            instructions.SetActive(true);
        }
        else if (flag == false)
        {
            instructions.SetActive(false);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();         //application.quit() does not work in the editor or in web based games
    }
}
