using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour
{
    private static PauseManager instance;
    private GameObject evHandler;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
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
        if(!isPaused)
        {
            pauseCanvas.enabled = true;
            Time.timeScale = 0f;
            isPaused = true;
        }

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