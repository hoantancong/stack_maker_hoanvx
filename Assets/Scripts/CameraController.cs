using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target; 
    public Vector3 offset; // Khoảng cách giữa camera và đối tượng
    public float smoothSpeed = 0.125f; // Tốc độ di chuyển camera

    void LateUpdate()
    {
        if (target == null)
        {
            target = FindObjectOfType<PlayerController>()?.gameObject?.transform;
        }
        else
        {
  
            Vector3 desiredPosition = target.position + offset;
            // move camera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // update
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }
    }

}
