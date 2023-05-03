using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseNPCModel : PlayerModel
{
    public MouseNPCView View { get; private set; }
    [Networked] float Life { get; set; }
    
    void Awake()
    {
        Speed = 6f;
        RunningSpeed = 10f;
        RotateSpeed = 5f;
        NetworkRB = GetComponent<NetworkRigidbody>();
        View = GetComponent<MouseNPCView>();
        _controller = new MouseNPCController(this, View);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void SetLife()
    {
        Life = 100f;
    }
    public override void FixedUpdateNetwork()
    {
        _controller.OnUpdate();
    }

    public void PlayerActions()
    {
        var input = GetInput(out NetworkInputData networkInputData);
        Debug.Log("MOVEMENT SPEED ACTIONS MOUSE..." + input);
        if (input)
        {
            if (networkInputData._isSprintPressed)
            {
                Debug.Log("MOVEMENT SPEED RUNNING MOUSE...");
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), RunningSpeed);
            }
            else 
            {
                Debug.Log("MOVEMENT SPEED NORMAL MOUSE...");
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), Speed);
            }
        }
    }

    public void Movement(Vector3 dir, float speed)
    {
        if (dir != Vector3.zero)
        {
            NetworkRB.Rigidbody.MovePosition(transform.position + dir * speed * Runner.DeltaTime);
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
