using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] List<StateMachine> enemies = new List<StateMachine>();
    [SerializeField] bool isChasing = false;

    [Header("Floor Note UI")]
    [SerializeField] GameObject floorNoteDisplay;
    [Header("Dungeon Fade In")]
    [SerializeField] Image fadeInUIImage;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeOutTime = 0.5f;

    TextMeshProUGUI floorNoteText;

    GlobalTeapot globalTeapot;
    AudioManager audioManager;

    List<FloorNote> floorNotes = new List<FloorNote>();

    // Start is called before the first frame update
    void Start()
    {
        globalTeapot = GameObject.FindWithTag("Global Teapot").GetComponent<GlobalTeapot>();
        audioManager = globalTeapot.audioManager;
        floorNotes = FindObjectsOfType<FloorNote>().ToList();
        floorNoteText = floorNoteDisplay.GetComponentInChildren<TextMeshProUGUI>();

        SetFloorNotes();
        StartCoroutine(FadeFromBlack());
    }

    /// <summary>
    /// Add a chasing enemy
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(StateMachine enemy)
    {
        if (enemy.currentState == StateMachine.State.Chasing)
        {
            enemies.Append(enemy);
            audioManager.PlayScream(enemy.GetComponent<StudioEventEmitter>());
            UpdateMusic();
        }
    }

    /// <summary>
    /// Remove an enemy from the list
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemy(StateMachine enemy)
    {
        enemies.Remove(enemy);
        UpdateMusic();
    }

    public void ActivateNote(FloorNote note)
    {
        foreach (var input in GetComponentsInChildren<PlayerInput>())
        {
            input.enabled = false;
        }
        globalTeapot.ObtainFloorNote(note.noteID);
        floorNoteDisplay.SetActive(true);
        floorNoteText.text = note.noteBody;
        if (note.disappear) { Destroy(note.gameObject); }
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
        }
    }

    public void DeactivateNote()
    {
        floorNoteDisplay.SetActive(false);
        floorNoteText.text = "";
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        foreach (var input in GetComponentsInChildren<PlayerInput>())
        {
            input.enabled = true;
        }

    }

    public void KillPlayer()
    {
        globalTeapot.numStoreCredits = globalTeapot.numNotesObtained;
        EndDungeon(true, false);
    }

    public void EndDungeon(bool death = false, bool artifact = false)
    {
        globalTeapot.hasDied = death;
        Action sceneChange;

        // If coming from intro state, move to floor one dungeon state (since the dungeon has been played)
        if (globalTeapot.currProgress == GlobalTeapot.TeaType.Intro)
        {
            globalTeapot.currProgress = GlobalTeapot.TeaType.Dungeon_F1;
        }

        if (artifact)
        {
            switch (globalTeapot.currProgress)
            {
                case GlobalTeapot.TeaType.Dungeon_F1:
                    globalTeapot.currProgress = GlobalTeapot.TeaType.Dungeon_F2;
                    break;
                case GlobalTeapot.TeaType.Dungeon_F2:
                    globalTeapot.currProgress = GlobalTeapot.TeaType.End;
                    break;
                default:
                    globalTeapot.currProgress = GlobalTeapot.TeaType.Dungeon_F1;
                    break;
            }
        }

        if (death)
        {
            globalTeapot.deathCount++;
            sceneChange = () => Loader.Load(Loader.Scene.DeathScene);
        }
        else
        {
            sceneChange = () => Loader.Load(Loader.Scene.Village);
        }
        StartCoroutine(FadeToBlack(sceneChange));
    }

    /// <summary>
    /// Update the music based on 
    /// </summary>
    void UpdateMusic()
    {
        if (!isChasing && enemies.Count > 0)
        {
            isChasing = true;
            audioManager.ChaseOST();
        }
        else if (isChasing && enemies.Count == 0)
        {
            isChasing = false;
            audioManager.DungeonOST();
        }
    }

    void SetFloorNotes()
    {
        foreach (FloorNote note in floorNotes)
        {
            if (note.noteID.Length > 0)
            {
                
                if (note.noteID.ToLower() == "destroy" || globalTeapot.journal.CheckFloorNote(note.noteID))
                {
                    Destroy(note.gameObject);
                }
                else
                {
                    note.noteTitle = "";
                    note.noteBody = globalTeapot.journal.ReadFloorNote(note.noteID);
                }
            }
        }
    }

    private IEnumerator FadeFromBlack()
    {
        Time.timeScale = 0f;
        fadeInUIImage.gameObject.SetActive(true);
        Color objectColor = fadeInUIImage.color; //Gets Object Color and Modifies values
        float timer = fadeInTime;
        while (fadeInUIImage.color.a > 0)
        {
            timer -= Time.unscaledDeltaTime;
            objectColor.a = Mathf.Lerp(-0.1f, 1, timer / fadeInTime);
            fadeInUIImage.color = objectColor;
            yield return null;
        }
        fadeInUIImage.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator FadeToBlack(Action sceneChange)
    {
        Time.timeScale = 0f;
        Color objectColor = fadeInUIImage.color; //Gets Object Color and Modifies values
        fadeInUIImage.gameObject.SetActive(true);
        float timer = fadeOutTime;
        while (fadeInUIImage.color.a < 1)
        {
            timer -= Time.unscaledDeltaTime;
            objectColor.a = Mathf.Lerp(1.1f, 0, timer / fadeOutTime);
            fadeInUIImage.color = objectColor;
            yield return null;
        }
        Time.timeScale = 1f;
        sceneChange();
    }
}