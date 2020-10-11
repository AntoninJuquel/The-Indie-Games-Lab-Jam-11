using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] wireSegmentsPrefab;
    int receptorCount = 0;
    int pluggedWire = 0;

    [SerializeField] GameObject pausedScreen = null;
    [SerializeField] GameObject pausedScreenBis = null;
    [SerializeField] GameObject confirmScreen = null;
    [SerializeField] TextMeshProUGUI confirmText = null;
    [SerializeField] TextMeshProUGUI totalTimers = null;
    [SerializeField] TextMeshProUGUI runTimers = null;
    [SerializeField] TextMeshProUGUI levelTimers = null;
    [SerializeField] GameObject endObject = null;
    bool paused = false;

    enum Confirm
    {
        MainMenu,
        Quit
    }
    Confirm confirmationState;
    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        pausedScreen.SetActive(paused);
        pausedScreenBis.SetActive(paused);
        confirmScreen.SetActive(false);
        endObject.SetActive(false);
        Time.timeScale = paused ? 0 : 1;

        ScootRopes();
        PlayerController.Instance.OnDie += RestartLevel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
        if (Input.GetKeyDown(KeyCode.Backspace))
            RestartLevel();
    }

    public void TogglePause()
    {
        paused = !paused;
        pausedScreen.SetActive(paused);
        pausedScreenBis.SetActive(paused);
        confirmScreen.SetActive(false);

        if (DataManager.Instance != null)
        {
            totalTimers.text = string.Concat(DataManager.Instance.GetTotalTime()[0].ToString(), " : ", DataManager.Instance.GetTotalTime()[1].ToString(), " : ", DataManager.Instance.GetTotalTime()[2].ToString("000"));
            runTimers.text = string.Concat(DataManager.Instance.GetTimeOfCurrentRun()[0].ToString(), " : ", DataManager.Instance.GetTimeOfCurrentRun()[1].ToString(), " : ", DataManager.Instance.GetTimeOfCurrentRun()[2].ToString("000"));
            levelTimers.text = string.Concat(DataManager.Instance.GetTimeOfCurrentLevel()[0].ToString(), " : ", DataManager.Instance.GetTimeOfCurrentLevel()[1].ToString(), " : ", DataManager.Instance.GetTimeOfCurrentLevel()[2].ToString("000"));
        }

        Time.timeScale = paused ? 0 : 1;
    }
    public void MainMenu()
    {
        confirmText.text = "Go to main menu ?";
        confirmScreen.SetActive(true);
        pausedScreenBis.SetActive(false);
        confirmationState = Confirm.MainMenu;
    }
    public void QuitGame()
    {
        confirmText.text = "Quit the game ?";
        confirmScreen.SetActive(true);
        pausedScreenBis.SetActive(false);
        confirmationState = Confirm.Quit;
    }
    public void Yes()
    {
        if (DataManager.Instance)
            Destroy(DataManager.Instance.gameObject);
        switch (confirmationState)
        {
            case Confirm.MainMenu:
                SceneManager.LoadScene(0);
                break;
            case Confirm.Quit:
                Application.Quit();
                break;
            default:
                break;
        }
    }
    public void No()
    {
        confirmScreen.SetActive(false);
        pausedScreenBis.SetActive(true);
    }

    void ScootRopes()
    {
        receptorCount = 0;
        pluggedWire = 0;
        foreach (Receptor receptor in FindObjectsOfType<Receptor>())
        {
            receptorCount++;
            receptor.OnPlug += HandleWirePlugged;
            receptor.OnUnplug += HandleWireUnplugged;
        }
    }

    void HandleWirePlugged()
    {
        pluggedWire++;
        AudioManager.instance.Play("Plug");
        if (pluggedWire == receptorCount)
        {
            endObject.SetActive(true);
            EndScreenManager end = GetComponent<EndScreenManager>();
            if (end)
                end.Setup();
        }
    }

    void HandleWireUnplugged()
    {
        pluggedWire--;
        endObject.SetActive(false);
    }
    public void NextLevel()
    {
        if (DataManager.Instance != null)
            DataManager.Instance.NewLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartRun()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.StartRun();
            DataManager.Instance.NewLevel();
        }
        SceneManager.LoadScene(1);
    }
}
