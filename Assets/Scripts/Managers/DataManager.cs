using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    float timeThisLevelStarted;
    float timeThisRunStarted;

    int deaths;

    [SerializeField]List<float[]> timesLevelEnded = new List<float[]>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        StartRun();
    }
    public float[] GetTotalTime()
    {
        return FormatTime(Time.time);
    }
    public float[] GetTimeOfCurrentRun()
    {
        return FormatTime(Time.time - timeThisRunStarted);
    }
    public float[] GetTimeOfCurrentLevel()
    {
        return FormatTime(Time.time - timeThisLevelStarted);
    }
    public List<float[]> GetAllLevelTimers()
    {
        return timesLevelEnded;
    }
    public void StartRun()
    {
        deaths = 0;
        timesLevelEnded = new List<float[]>();
        timeThisRunStarted = Time.time;
        timeThisLevelStarted = Time.time;
    }
    public void NewLevel()
    {
        timesLevelEnded.Add(GetTimeOfCurrentLevel());
        timeThisLevelStarted = Time.time;
    }

    float[] FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        float ms = time * 1000;
        ms %= 1000;

        return new float[3] { minutes, seconds, ms };
    }
    public void AddDeath()
    {
        deaths++;
    }
    public int GetDeaths()
    {
        return deaths;
    }
}
