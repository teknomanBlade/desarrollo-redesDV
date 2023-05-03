using Fusion;
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
