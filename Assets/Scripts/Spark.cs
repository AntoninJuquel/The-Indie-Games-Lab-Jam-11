using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Spark : MonoBehaviour
{
    public float mult;
    [SerializeField] Transform sparkGo = null;
    [SerializeField] Transform checkpoints = null;

    [SerializeField] float[] waitTimes = new float[] { };
    [SerializeField] float[] checkpointsPerSec = new float[] { };

    [SerializeField] bool pingPong = false;
    bool going = true;

    int waitTimeIndex;
    int checkPointPerSecIndex;
    int checkpointIndex;

    [SerializeField] bool activated = true;

    ParticleSystem ps;
    SpriteRenderer sr;
    Collider2D cl;
    Light2D l;
    private void Awake()
    {
        ps = sparkGo.GetComponentInChildren<ParticleSystem>();
        sr = sparkGo.GetComponent<SpriteRenderer>();
        cl = sparkGo.GetComponent<Collider2D>();
        l = sparkGo.GetComponentInChildren<Light2D>();
    }
    private void Start()
    {
        if (activated)
            StartCoroutine(Action());
        else
        {
            ps.Stop();
            sr.enabled = false;
            cl.enabled = false;
            l.enabled = false;
        }
    }

    IEnumerator Action()
    {
        while (true)
        {
            going = pingPong ? !going : going;
            checkpointIndex = going ? 0 : checkpoints.childCount - 1;
            sparkGo.position = checkpoints.GetChild(checkpointIndex).position;
            checkpointIndex = going ? checkpointIndex + 1 : checkpointIndex - 1;
            checkPointPerSecIndex = checkPointPerSecIndex + 1 < checkpointsPerSec.Length ? checkPointPerSecIndex + 1 : 0;
            waitTimeIndex = waitTimeIndex + 1 < waitTimes.Length ? waitTimeIndex + 1 : 0;

            var main = ps.main;
            main.startSpeed = checkpointsPerSec[checkPointPerSecIndex] * Random.Range(2f, 4f);
            ps.Play();
            sr.enabled = true;
            cl.enabled = true;
            l.enabled = true;
            Coroutine coroutine = StartCoroutine(PlaySound());

            while (0 <= checkpointIndex && checkpointIndex < checkpoints.childCount)
            {
                float t = 0f;
                Vector2 start = sparkGo.position;
                Vector2 end = checkpoints.GetChild(checkpointIndex).position;
                while (t < 1f / checkpointsPerSec[checkPointPerSecIndex])
                {
                    t += Time.deltaTime;
                    RotateTowardZ(end, sparkGo.position, sparkGo, 100);
                    sparkGo.position = Vector3.Lerp(start, end, checkpointsPerSec[checkPointPerSecIndex] * t);
                    yield return null;
                }

                checkpointIndex = going ? checkpointIndex + 1 : checkpointIndex - 1;
            }
            StopCoroutine(coroutine);
            ps.Stop();
            sr.enabled = false;
            cl.enabled = false;
            l.enabled = false;
            yield return new WaitForSeconds(waitTimes[waitTimeIndex]);
        }
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            AudioManager.instance.Play("Spark");
            yield return new WaitForSeconds(1f / (checkpointsPerSec[checkPointPerSecIndex] * mult));
        }
    }

    public void ToggleSpark()
    {
        activated = !activated;
        if (activated)
        {
            ps.Play();
            StartCoroutine(Action());
        }
        else
        {
            ps.Stop();
            StopAllCoroutines();
        }
        sr.enabled = activated;
        cl.enabled = activated;
        l.enabled = activated;
    }

    void RotateTowardZ(Vector3 target, Vector3 position, Transform toRotate, float speed)
    {
        Vector3 vectorToTarget = target - position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        toRotate.rotation = Quaternion.Slerp(toRotate.rotation, q, Time.deltaTime * speed);
    }
}
