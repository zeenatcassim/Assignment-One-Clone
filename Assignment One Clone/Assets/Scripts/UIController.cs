using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] AudioState audioState;

    /**
     * Menu Controller 
     **/
    public void levelOne()
    {
        SceneManager.LoadScene(1);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit();
    }

    /**
     * UI Controller
     **/

    public void audioController()
    {
        if (audioState.state) audioState.state = false;
        else audioState.state = true;
    }
}
