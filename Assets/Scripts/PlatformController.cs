using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    Color color;
    public IEnumerator LetGo()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), PlayerController.Instance.GetComponent<Collider2D>(), true);
        color = Color.red;
        yield return new WaitForSeconds(.5f);
        yield return new WaitWhile(() => Physics2D.OverlapCircle(transform.position, .5f, PlayerController.Instance.gameObject.layer));
        color = Color.green;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), PlayerController.Instance.GetComponent<Collider2D>(), false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, .5f);
    }
}
