using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class WireController : MonoBehaviour
{
    [SerializeField] WireColor color = new WireColor();

    [SerializeField] Transform handler;
    [SerializeField] Transform pluggedTo;

    Collider2D col;
    HingeJoint2D hj;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        hj = GetComponent<HingeJoint2D>();
    }
    private void Update()
    {
        if (handler)
        {
            transform.position = handler.position;
            MyUtilities.RotateTowardZ(hj.connectedBody.transform.position, transform.position, transform, 100f);
        }
    }
    public WireController GrabWire(Transform handler)
    {
        if (this.handler == null)
        {
            if (pluggedTo)
            {
                pluggedTo.GetComponent<Receptor>().UnplugWire();
                pluggedTo = null;
            }

            transform.rotation = Quaternion.Euler(0, 0, -90);
            GetComponent<Rigidbody2D>().freezeRotation = true;

            this.handler = handler;
            col.enabled = false;
            return this;
        }
        return null;
    }
    public void PlugWire(Transform pluggedTo)
    {
        this.pluggedTo = pluggedTo;
        handler = null;
        col.enabled = true;
        transform.position = pluggedTo.position;
    }
    public WireColor GetWireColor()
    {
        return color;
    }
}*/
