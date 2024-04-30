using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private static PauseManager instance;
    public Canvas pauseCanvas;
    [SerializeField] GameObject optionsMenu;
    public static bool isPaused;
    PlayerInput playerInput;
    InputAction pauseButton;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        ResumeGame();
        isPaused = false;
        playerInput = GetComponent<PlayerInput>();
        pauseButton = playerInput.actions.FindAction("Pause");
        pauseButton.performed += ctx => TogglePause();
    }

    void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseCanvas.enabled = true;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Options()
    {
        pauseCanvas.enabled = false;
        optionsMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseCanvas.enabled = false;
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}