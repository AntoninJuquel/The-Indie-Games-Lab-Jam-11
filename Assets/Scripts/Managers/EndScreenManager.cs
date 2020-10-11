using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] GameObject timerItem = null;
    [SerializeField] Transform timersList = null;
    [SerializeField] TextMeshProUGUI runTimer = null;
    [SerializeField] TextMeshProUGUI deathCounter = null;

    public void Setup()
    {
        if (DataManager.Instance)
        {
            Time.timeScale = 0;
            DataManager.Instance.NewLevel();
            runTimer.text = string.Concat(DataManager.Instance.GetTimeOfCurrentRun()[0], " : ", DataManager.Instance.GetTimeOfCurrentRun()[1], " : ", DataManager.Instance.GetTimeOfCurrentRun()[2].ToString("000"));

            int i = 0;
            foreach (float[] timers in DataManager.Instance.GetAllLevelTimers())
            {
                i++;
                GameObject ti = Instantiate(timerItem, timersList);
                ti.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i == DataManager.Instance.GetAllLevelTimers().Count ? "Final Level" : string.Concat("Level ", i);
                ti.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Concat(timers[0], " : ", timers[1], " : ", timers[2].ToString("000"));
            }
            deathCounter.text = string.Concat("Deaths : ", DataManager.Instance.GetDeaths());
        }
    }
}
