using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    private Vector3 offsetCat = new Vector3(0f, 0.35f, -1f);
    private Vector3 offsetMouse = new Vector3(0f, 0.15f, -0.6f);
    private float MouseSensitivity = 6f;
    private float verticalRotation;
    private float horizontalRotation;

    void LateUpdate()
    {
        if (Target == null)
        {
            //Debug.Log("TARGET NULL? ");
            return;
        }

        transform.position = Target.position + GetOffsetByType(Target);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 30f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }

    private Vector3 GetOffsetByType(Transform Target) 
    {
        if (Target.gameObject.name.Contains("Cat"))
        {
            return offsetCat;
        }
        else 
        {
            return offsetMouse;
        }
    }
}
