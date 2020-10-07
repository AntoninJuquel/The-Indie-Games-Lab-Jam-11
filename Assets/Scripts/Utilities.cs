using UnityEngine;
public static class MyUtilities
{
    public static void RotateTowardZ(Vector3 target, Vector3 position, Transform toRotate, float speed)
    {
        Vector3 vectorToTarget = target - position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        toRotate.rotation = Quaternion.Slerp(toRotate.rotation, q, Time.deltaTime * speed);
    }
}
