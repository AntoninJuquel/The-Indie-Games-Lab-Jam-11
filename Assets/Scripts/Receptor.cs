using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptor : MonoBehaviour
{
    [SerializeField] GameObject receptor = null;
    Collider2D col;
    HingeJoint2D hj;
    public event System.Action OnPlug;
    public event System.Action OnUnplug;
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
            col.enabled = false;
            grabbedWire.PlugWire(receptor.transform);
            OnPlug();
            return true;
        }
        return false;
    }

    public void UnplugWire()
    {
        col.enabled = true;
        OnUnplug();
    }
}
