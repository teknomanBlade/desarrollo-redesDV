using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNPCModel : NetworkBehaviour
{
    //private IMove _currentMove;
    public Camera Camera;
    public static MouseNPCModel Local { get; private set; }
    public MouseNPCView View { get; private set; }
    private IController _controller;
    public NetworkRigidbody NetworkRB { get; set; }
    [Networked] float Life { get; set; }

    private float _speed;
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }
    public float RunningSpeed { get; set; }
    public float RotateSpeed { get; set; }
    void Awake()
    {
        Speed = 6f;
        RunningSpeed = 15f;
        RotateSpeed = 5f;
        Life = 100f;
        NetworkRB = GetComponent<NetworkRigidbody>();
        View = GetComponent<MouseNPCView>();
        _controller = new MouseNPCController(this, View);
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
            Camera = Camera.main;
            Camera.GetComponent<ThirdPersonCamera>().Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
        }

    }
    public override void FixedUpdateNetwork()
    {
        _controller.OnUpdate();
    }

    public void PlayerActions()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            if (networkInputData._isSprintPressed)
            {
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), RunningSpeed);
            }
            else 
            {
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), Speed);
            }
        }
    }

    public void Movement(Vector3 dir, float speed)
    {
        if (dir != Vector3.zero)
        {
            NetworkRB.Rigidbody.MovePosition(dir * speed * Runner.DeltaTime);
        }
    }
    public void TakeDamage(float dmg)
    {
        RPC_GetDamage(dmg);
    }

    [Rpc(RpcSources.Proxies, RpcTargets.StateAuthority)]
    void RPC_GetDamage(float dmg) 
    {
        Life -= dmg;

        if (Life <= 0)
        {
            Dead();
        }
    }

    public void Dead() 
    {
        Runner.Shutdown();
    }
    /*public static void OnLifeChanged(Changed<MouseNPCModel> changed)
    {
        float newLife = changed.Behaviour.Life;
        changed.LoadOld();

        if (newLife < changed.Behaviour.Life) 
        {
            changed.Behaviour.TakeDamage();
        }
    }*/

    /*public void OnTriggerEnter(Collider other)
    {
        if(!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out CatPlayerModel catPlayerModel))
        {
            Debug.Log("MOUSE HITTED - MOUSE...");
            TakeDamage(catPlayerModel.Damage);
            //gameObject.GetComponent<SpaceShipView>().RepaintLife(character.Life);
        }
    }*/
}
