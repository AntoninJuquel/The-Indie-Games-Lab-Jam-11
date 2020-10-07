using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
    [SerializeField] Color color = new Color();
    [SerializeField] Rigidbody2D hook = null;
    [SerializeField] int maxLinks = 5;

    List<GameObject> segments = new List<GameObject>();

    // First wire

    [SerializeField] GameObject firstSegment = null;
    [SerializeField] Transform handler;
    [SerializeField] Transform pluggedTo;

    Collider2D col;
    HingeJoint2D hj;
    private void OnValidate()
    {
        InitializeColors();
    }
    private void Awake()
    {
        col = firstSegment.GetComponent<Collider2D>();
        hj = firstSegment.GetComponent<HingeJoint2D>();
        segments.Add(firstSegment);

        InitializeColors();
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, segments[0].transform.position) > segments.Count)
            if (segments.Count < maxLinks)
                AddLink();
            else
                handler.GetComponent<Rigidbody2D>().velocity += (Vector2)(transform.position - handler.transform.position);

        if (handler)
        {
            firstSegment.transform.position = handler.position;
            MyUtilities.RotateTowardZ(hj.connectedBody.transform.position, firstSegment.transform.position, firstSegment.transform, 100f);
        }
    }
    void InitializeColors()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.color = color;
        }
    }
    void AddLink()
    {
        int index = Random.Range(0, GameManager.Instance.ropeSegmentsPrefab.Length);
        GameObject newSeg = Instantiate(GameManager.Instance.ropeSegmentsPrefab[index]);
        newSeg.transform.parent = transform;
        newSeg.transform.position = transform.position;
        newSeg.GetComponent<SpriteRenderer>().color = color;
        HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
        hj.connectedBody = hook;
        hj.anchor = new Vector2(0, -1);
        segments[segments.Count - 1].GetComponent<HingeJoint2D>().connectedBody = newSeg.GetComponent<Rigidbody2D>();
        segments[segments.Count - 1].GetComponent<HingeJoint2D>().anchor = new Vector2(0, -1);
        segments.Add(newSeg);
    }

    public WireController GrabWire(Transform handler)
    {
        if (Vector2.Distance(handler.position, firstSegment.transform.position) <= 1.5f)
            if (this.handler == null)
            {
                if (pluggedTo)
                {
                    GetComponent<Receptor>().UnplugWire();
                    pluggedTo = null;
                }

                firstSegment.GetComponent<Rigidbody2D>().freezeRotation = true;

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
        firstSegment.transform.position = pluggedTo.position;
        firstSegment.GetComponent<Rigidbody2D>().freezeRotation = false;
    }
    public Color GetWireColor()
    {
        return color;
    }
    public Rigidbody2D GetFirstSegmentRb()
    {
        return firstSegment.GetComponent<Rigidbody2D>();
    }
}
