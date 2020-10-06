using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    public event System.Action OnPlug;
    public event System.Action OnUnplug;

    [SerializeField] WireColor color = new WireColor();

    [SerializeField] Transform handler;
    [SerializeField] Transform pluggedTo;

    Collider2D col;
    LineRenderer lr;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPositions(new Vector3[] { transform.position, transform.position });
    }
    private void Update()
    {
        if (handler)
        {
            lr.SetPosition(1, transform.position);
            transform.position = handler.position;
        }
        else if (pluggedTo)
            transform.position = pluggedTo.position;
    }
    public WireController GrabWire(Transform handler)
    {
        if (this.handler == null)
        {
            if (pluggedTo)
                pluggedTo.GetComponent<WireController>().UnplugWire(this);
            
            this.handler = handler;
            col.enabled = false;
            return this;
        }
        return null;
    }
    public bool PlugWire(WireController grabbedWire, Transform pluggedTo)
    {
        if (grabbedWire.GetWireColor() == color)
        {
            OnPlug();
            this.pluggedTo = pluggedTo;
            col.enabled = true;
            grabbedWire.PlugWire(transform);
            return true;
        }
        return false;
    }
    public void PlugWire(Transform pluggedTo)
    {
        OnPlug();
        transform.position = pluggedTo.position;
        lr.SetPosition(1, transform.position);
        this.pluggedTo = pluggedTo;
        handler = null;
        col.enabled = true;
    }
    public void UnplugWire(WireController wire)
    {
        wire.UnplugWire();
        OnUnplug();
        pluggedTo = null;
    }
    public void UnplugWire()
    {
        OnUnplug();
        pluggedTo = null;
    }
    public WireColor GetWireColor()
    {
        return color;
    }
}
