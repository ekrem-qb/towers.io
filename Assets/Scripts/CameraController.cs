using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance;
    public float followingSpeed;
    void Update()
    {
        if (target)
        {
            this.transform.position = Vector3.Slerp(this.transform.position, (-this.transform.forward * distance) + target.position, Time.deltaTime * followingSpeed);
        }
    }
}