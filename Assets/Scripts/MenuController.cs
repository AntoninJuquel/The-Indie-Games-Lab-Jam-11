using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject play = null;

    [SerializeField] GameObject plus = null;
    [SerializeField] GameObject minus = null;
    [SerializeField] GameObject[] dots = null;
    int volume = 4;

    [SerializeField] AudioMixer mixer = null;

    private void Start()
    {
        UpdateDots();
    }

    private void Update()
    {
        HandleQuit();
        HandlePlay();
        HandleVolume();
    }

    void HandleQuit()
    {
        if (PlayerController.Instance.transform.position.x <= -9.44f)
        {
            Application.Quit();
        }
    }
    void HandlePlay()
    {
        if (Input.GetKeyDown(KeyCode.F) && Vector2.Distance(PlayerController.Instance.transform.position, play.transform.position) <= 1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void HandleVolume()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), minus.transform.position) < 1f)
                volume = Mathf.Clamp(volume - 1, 0, 4);
            else if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), plus.transform.position) < 1f)
                volume = Mathf.Clamp(volume + 1, 0, 4);
            UpdateDots();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(PlayerController.Instance.transform.position, minus.transform.position) < 1f)
                volume = Mathf.Clamp(volume - 1, 0, 4);
            else if (Vector2.Distance(PlayerController.Instance.transform.position, plus.transform.position) < 1f)
                volume = Mathf.Clamp(volume + 1, 0, 4);
            UpdateDots();
        }
    }

    void UpdateDots()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            if (i < volume)
                dots[i].GetComponent<SpriteRenderer>().color = Color.white;
            else
                dots[i].GetComponent<SpriteRenderer>().color = Color.grey;
        }
        mixer.SetFloat("Master", Mathf.Log10(volume == 0 ? 0.0001f : volume /4f) * 20);
    }
}
