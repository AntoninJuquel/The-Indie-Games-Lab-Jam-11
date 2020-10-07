using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeprecatedRope : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] ropeSegmentsPrefab;
    public int numLinks = 5;

    private void Start()
    {
        GenerateRope();
    }

    void GenerateRope()
    {
        Rigidbody2D prevBody = hook;
        for (int i = 0; i < numLinks; i++)
        {
            int index = Random.Range(0, ropeSegmentsPrefab.Length);
            GameObject newSeg = Instantiate(ropeSegmentsPrefab[index]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBody;

            prevBody = newSeg.GetComponent<Rigidbody2D>();
        }
    }
}
