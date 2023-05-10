using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    public static PlayerModel Local { get; private set; }
    public Camera Camera;
    protected IController _controller;
    public NetworkRigidbody NetworkRB { get; set; }
    public float Speed { get; set; }
    public float RunningSpeed { get; set; }
    public float RotateSpeed { get; set; }
    
    protected int _currentSignX;
    protected int _previousSignX;
    protected int _currentSignZ = 0;
    protected int _previousSignZ = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Rotate(Vector3 euler)
    {
        NetworkRB.Rigidbody.MoveRotation(Quaternion.Euler(euler));
    }
    protected void ManageRotation(Vector3 dir) 
    {
        _currentSignX = (int)Mathf.Sign(dir.x);
        _currentSignZ = (int)Mathf.Sign(dir.z);

        if (_previousSignZ != _currentSignZ)
        {
            _previousSignZ = _currentSignZ;

            if (_currentSignZ == -1)
            {
                Rotate(180 * Vector3.up);
            }
            else
            {
                Rotate(0 * Vector3.up);
            }
        }
        else if (_previousSignX != _currentSignX)
        {
            _previousSignX = _currentSignX;
            Rotate(_currentSignX * 90 * Vector3.up);
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("[Custom Message] Spawned own Player");
        }
        else
        {
            Debug.Log("[Custom Message] Spawned other (Proxy) Player");
        }

        if (Object.HasStateAuthority)
        {
            SetLife();
            Camera = Camera.main;
            Camera.GetComponent<ThirdPersonCamera>().Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
        }
    }

    public virtual void SetLife() 
    {
    
    }

    
}
