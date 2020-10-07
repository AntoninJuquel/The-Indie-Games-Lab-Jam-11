using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeprecatedRopeSegment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;

    private void Start()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        DeprecatedRopeSegment aboveSegment = connectedAbove.GetComponent<DeprecatedRopeSegment>();
        if(aboveSegment!=null)
        {
            aboveSegment.connectedBelow = gameObject;
            //float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            float spriteBottom = connectedAbove.transform.localScale.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom * -1);
        }
        else
            GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;

    }
}
