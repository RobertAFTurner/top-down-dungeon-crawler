using UnityEngine;

public class CameraController : MonoBehaviour
{   
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothSpeed = 0.0125f;

    [SerializeField]
    private Vector3 offset;

    void LateUpdate() 
    {
        var desiredPosition = target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
