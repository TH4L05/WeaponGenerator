using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IngameMenu : MonoBehaviour
{
    [Header("General")]
    public static bool GamePaused;
    public bool pauseMenuActive;
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public IngameUI ingameUI;


    [Header("Playables")]
    public PlayableDirector showPauseMenu;
    public PlayableDirector hidePauseMenu;
    public PlayableDirector showOptionsMenu;
    public PlayableDirector hideOptionsMenu;

    public void Awake()
    {
        GamePaused = false;
    }

    public void ToggleMenu()
    {
        if (GamePaused && !pauseMenuActive)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void ToggleOptionMenu(bool active)
    {
        pauseMenuActive = active;
        optionMenu.SetActive(active);
        pauseMenu.SetActive(!active);
    }

    public void Pause()
    {
        GamePaused = true;
        ChangeCursorVisibility(true);
        //showPauseMenu?.Play();
        Game.instance.inputHandler.InputHandlerIsPaused(true);
        Game.instance.inputHandler.EnableInputActions(false);
        pauseMenu.SetActive(true);
        SetTimeScale(0f);

        //Debug.Log(GamePaused);
    }
    public void Resume()
    {
        GamePaused = false;
        SetTimeScale(1f);
        pauseMenu.SetActive(false);
        //hidePauseMenu.Play();
        Game.instance.inputHandler.EnableInputActions(true);
        Game.instance.inputHandler.InputHandlerIsPaused(false);
        ChangeCursorVisibility(false);
        //Debug.Log(GamePaused);
    }

    public void ChangeCursorVisibility(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        //Debug.Log(Time.timeScale);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
    
#endif
    }
}
