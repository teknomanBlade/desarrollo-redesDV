using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    public float MouseSensitivity = 6f;
    private float _offsetZ = -0.8f;

    private float verticalRotation;
    private float horizontalRotation;

    void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        Vector3 pos = Target.position;
        pos.z = _offsetZ;

        transform.position = pos;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }
}
