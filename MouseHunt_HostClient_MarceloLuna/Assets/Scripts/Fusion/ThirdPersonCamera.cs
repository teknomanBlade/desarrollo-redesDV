using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform Target;
    private Vector3 offsetCat = new Vector3(0f, 0.35f, -1f);
    private Vector3 offsetMouse = new Vector3(0f, -0.1f, 1.2f);
    //private float MouseSensitivity = 6f;
    private float verticalRotation;
    //private float horizontalRotation;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;

    void LateUpdate()
    {
        if (Target == null)
        {
            //Debug.Log("TARGET NULL? ");
            return;
        }
       
        ComputePosition(GetOffsetByType(Target));

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //verticalRotation -= mouseY * MouseSensitivity;
        //verticalRotation = GetVerticalRotationByType(Target);

        //horizontalRotation += mouseX * MouseSensitivity;

        ComputeLookAt();
    }

    public void ComputeLookAt(/*Quaternion rot*/)
    {
        if (lookAt)
        {
            transform.forward = Target.forward;
        }
        /*else
        {
            transform.rotation = rot;
        }*/
    }

    public void ComputePosition(Vector3 offsetPos) 
    {
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = Target.TransformPoint(offsetPos);
        }
        else
        {
            transform.position = Target.position + offsetPos;
        }
    }
    private float GetVerticalRotationByType(Transform Target)
    {
        if (Target.gameObject.name.Contains("Cat"))
        {
            return Mathf.Clamp(verticalRotation, -70f, 30f);
        }
        else
        {
            return Mathf.Clamp(verticalRotation, -40f, 12f);
        }
    }

    private Vector3 GetOffsetByType(Transform Target) 
    {
        if (Target.gameObject.name.Contains("Cat"))
        {
            return offsetCat;
        }
        else 
        {
            Camera.main.nearClipPlane = 0.1f;
            return offsetMouse;
        }
    }
}
