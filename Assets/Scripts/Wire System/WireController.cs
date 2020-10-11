using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WireController : MonoBehaviour
{
    [SerializeField] Color color = new Color();
    [SerializeField] TextMeshProUGUI text = null;
    [SerializeField] WireType wireType = new WireType();
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
        text.text = "";//maxLinks.ToString();

        InitializeColors();
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, segments[0].transform.position) > segments.Count && handler)
            if (segments.Count < maxLinks)
                AddLink();
            else if (handler)
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
        text.color = color;
    }
    void AddLink()
    {
        int index = UnityEngine.Random.Range(0, GameManager.Instance.wireSegmentsPrefab.Length);
        GameObject newSeg = Instantiate(GameManager.Instance.wireSegmentsPrefab[index]);

        //Setup
        newSeg.transform.parent = transform;
        newSeg.transform.position = transform.position;
        newSeg.GetComponent<SpriteRenderer>().color = color;
        StartCoroutine(SetupSegment(newSeg));

        // Joints
        HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
        hj.connectedBody = hook;
        hj.anchor = new Vector2(0, -1);
        segments[segments.Count - 1].GetComponent<HingeJoint2D>().connectedBody = newSeg.GetComponent<Rigidbody2D>();
        segments[segments.Count - 1].GetComponent<HingeJoint2D>().anchor = new Vector2(0, -1);
        segments.Add(newSeg);
        text.text = ""; //(maxLinks - segments.Count).ToString();
    }

    private IEnumerator SetupSegment(GameObject wire)
    {
        col.enabled = false;
        switch (wireType)
        {
            case WireType.CLIMBABLE:
                if (wire.GetComponent<PlatformController>() == null)
                    wire.AddComponent<PlatformController>();
                wire.layer = LayerMask.NameToLayer(handler ? "Wire" : "Block");
                break;
            case WireType.UNCLIMBABLE:
                wire.layer = LayerMask.NameToLayer("Wire");
                break;
            case WireType.BLOCK:
                wire.layer = LayerMask.NameToLayer("Block");
                break;
            case WireType.DEADLY:
                wire.layer = LayerMask.NameToLayer("Block");
                //wire.GetComponent<Collider2D>().isTrigger = pluggedTo;
                if (pluggedTo)
                {
                    yield return new WaitForSeconds(3f);
                    if (wire.GetComponent<Animator>())
                        wire.GetComponent<Animator>().SetBool("Electrify", true);
                    wire.tag = "Deadly";
                    AudioManager.instance.Play("DeadlyWire");
                }
                else
                {
                    AudioManager.instance.Stop("DeadlyWire");
                    wire.tag = "Untagged";
                }
                break;
            default:
                break;
        }
        if (!handler && wire == firstSegment)
        {
            GameObject tempBoxCol = new GameObject("TempBoxCollider", typeof(BoxCollider2D));
            tempBoxCol.transform.position = PlayerController.Instance.transform.position - Vector3.up;
            tempBoxCol.layer = LayerMask.NameToLayer("Block");
            yield return new WaitForSeconds(.25f);
            Destroy(tempBoxCol);
        }
        col.enabled = !handler;
    }

    public WireController GrabWire(Transform handler)
    {
        if (Vector2.Distance(handler.position, firstSegment.transform.position) <= 1.5f)
            if (this.handler == null)
            {
                this.handler = handler;

                if (pluggedTo)
                {
                    for (int i = 1; i < segments.Count; i++)
                    {
                        segments[i].GetComponent<Animator>().SetBool("Electrify", false);
                    }
                    GetComponent<Receptor>().UnplugWire();
                    pluggedTo = null;
                    StopAllCoroutines();
                }

                firstSegment.GetComponent<Rigidbody2D>().freezeRotation = true;

                foreach (GameObject wire in segments)
                {
                    StartCoroutine(SetupSegment(wire));
                }

                return this;
            }
        return null;
    }
    public void PlugWire(Transform pluggedTo)
    {
        this.pluggedTo = pluggedTo;
        handler = null;

        firstSegment.transform.position = pluggedTo.position;
        firstSegment.GetComponent<Rigidbody2D>().freezeRotation = false;

        foreach (GameObject wire in segments)
        {
            StartCoroutine(SetupSegment(wire));
        }
    }
    public void DropWire()
    {
        handler = null;

        foreach (GameObject wire in segments)
        {
            StartCoroutine(SetupSegment(wire));
        }
        firstSegment.layer = LayerMask.NameToLayer("Interactable");
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
