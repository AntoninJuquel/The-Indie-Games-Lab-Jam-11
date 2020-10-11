using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Receptor : MonoBehaviour
{
    [SerializeField] GameObject receptor = null;
    Collider2D col;
    HingeJoint2D hj;
    public event System.Action OnPlug;
    public event System.Action OnUnplug;

    public UnityEvent PlugEvent;
    public UnityEvent UnplugEvent;
    private void Awake()
    {
        col = receptor.GetComponent<Collider2D>();
        hj = receptor.GetComponent<HingeJoint2D>();
    }
    public bool PlugWire(WireController grabbedWire, Transform player)
    {
        if (grabbedWire.GetWireColor() == GetComponent<WireController>().GetWireColor() && Vector2.Distance(player.position, receptor.transform.position)<=1.5f)
        {
            hj.connectedBody = grabbedWire.GetFirstSegmentRb();
            grabbedWire.PlugWire(receptor.transform);
            OnPlug();
            PlugEvent.Invoke();
            return true;
        }
        return false;
    }

    public void UnplugWire()
    {
        hj.connectedBody = null;
        col.enabled = true;
        OnUnplug();
        UnplugEvent.Invoke();
    }
}
