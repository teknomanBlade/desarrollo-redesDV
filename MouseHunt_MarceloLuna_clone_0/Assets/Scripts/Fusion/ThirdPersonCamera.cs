using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    private float MouseSensitivity = 6f;
    private float _offsetZ = -0.8f;
    private float _offsetY = 0.1f;
    private float verticalRotation;
    private float horizontalRotation;

    void LateUpdate()
    {
        if (Target == null)
        {
            Debug.Log("TARGET NULL? ");
            return;
        }

        Vector3 pos = Target.position;
        Debug.Log("TARGET: " + Target.name);
        pos.z = _offsetZ;
        pos.y = _offsetY;
        Debug.Log("CAMERA OFFSET Z: " + _offsetZ);
        transform.position = pos;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 30f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }
}
