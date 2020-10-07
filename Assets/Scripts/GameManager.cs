using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] ropeSegmentsPrefab;
    int receptorCount = 0;
    int pluggedWire = 0;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ScootRopes();
    }

    void ScootRopes()
    {
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
    }

    void HandleWireUnplugged()
    {
        pluggedWire--;
    }
}
