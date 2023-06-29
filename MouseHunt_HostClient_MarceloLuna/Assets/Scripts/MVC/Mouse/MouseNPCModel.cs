using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouseNPCModel : PlayerModel
{
    public event Action<float> OnTakeDamage = delegate { };
    
    public MouseNPCView View { get; private set; }
    [Networked] float Life { get; set; }
    public bool IsMouseDead { get; set; }
    void Awake()
    {
        Speed = 4f;
        RunningSpeed = 6f;
        NetworkRB = GetComponent<NetworkRigidbody>();
        View = GetComponent<MouseNPCView>();
        _controller = new MouseNPCController(this, View);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override CharacterInputHandler GetInputHandler()
    {
        return GetComponent<CharacterInputHandlerCat>();
    }
    public override void DeactivateMouse()
    {
        RPC_DeactivateMouse();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_DeactivateMouse() 
    {
        gameObject.SetActive(false);
    }

    public override void SetLife()
    {
        if (Object.HasStateAuthority) 
        {
            Life = 100f;
        }
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        _controller.OnUpdate();
    }
   
    public void PlayerActions()
    {
        var input = GetInput(out NetworkInputData networkInputData);
        //Debug.Log("MOVEMENT SPEED ACTIONS MOUSE..." + input);
        if (input)
        {
            Dir = new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement);
            if (networkInputData._isSprintPressed)
            {
                //Debug.Log("MOVEMENT SPEED RUNNING MOUSE...");
                Movement(Dir, RunningSpeed);
            }
            else 
            {
                //Debug.Log("MOVEMENT SPEED NORMAL MOUSE...");
                Movement(Dir, Speed);
            }
        }
    }

    public void Movement(Vector3 dir, float speed)
    {
        if (dir != Vector3.zero)
        {
            NetworkRB.Rigidbody.MovePosition(transform.position + Runner.DeltaTime * speed * dir);
            ManageRotation(dir);
        }
    }

    public void TakeDamage(float dmg)
    {
        RPC_GetDamage(dmg);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_GetDamage(float dmg) 
    {
        Life -= dmg;
        Debug.Log("CURRENT LIFE:" + Life);
        RPC_OnTakeDamage(dmg);
        if (Life <= 0)
        {
            GameManager.Instance.RPC_IsMouseDead();
            FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("Player2Lose"))
                .FirstOrDefault().gameObject.SetActive(true);
            FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("BtnRestart"))
                .FirstOrDefault().gameObject.SetActive(true);
            StartCoroutine(DeadCoroutine());
        }
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_OnTakeDamage(float dmg) 
    {
        OnTakeDamage(dmg);
    }
    IEnumerator DeadCoroutine() 
    {
        yield return new WaitForSeconds(2f);
        Dead();
    }
    public void Dead() 
    {
        Runner.Despawn(Object);
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

    public void OnTriggerEnter(Collider other)
    {
        if(!Object || !Object.HasStateAuthority) return;

        if (other.gameObject.layer == 6)
        {
            Debug.Log("MOUSE REACHED GOAL...");
            GameManager.Instance.RPC_MouseHasReachedGoal();
            FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("Player2Win"))
                .FirstOrDefault().gameObject.SetActive(true);
            FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("BtnRestart"))
                .FirstOrDefault().gameObject.SetActive(true);

        }
    }
}
