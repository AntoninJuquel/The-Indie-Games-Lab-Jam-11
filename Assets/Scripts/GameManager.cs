using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int wireCount = 0;
    int pluggedWire = 0;
    private void Awake()
    {
        foreach (WireController wire in FindObjectsOfType<WireController>())
        {
            wireCount++;
            wire.OnPlug += HandleWirePlugged;
            wire.OnUnplug += HandleWireUnplugged;
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
