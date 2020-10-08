using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public IEnumerator LetGo()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 11, true);
        yield return new WaitForSeconds(.5f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 11, false);
    }
}
