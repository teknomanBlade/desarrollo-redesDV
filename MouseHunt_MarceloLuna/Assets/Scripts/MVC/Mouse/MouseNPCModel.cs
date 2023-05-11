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
        RunningSpeed = 8f;
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
        View.MouseLife = FindObjectsOfType<RectTransform>(true)
                        .Where(x => x.gameObject.name.Equals("MouseLife"))
                        .FirstOrDefault().GetComponent<Image>();
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
            if (networkInputData._isSprintPressed)
            {
                //Debug.Log("MOVEMENT SPEED RUNNING MOUSE...");
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), RunningSpeed);
            }
            else 
            {
                //Debug.Log("MOVEMENT SPEED NORMAL MOUSE...");
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), Speed);
            }
        }
    }

    public void Movement(Vector3 dir, float speed)
    {
        if (dir != Vector3.zero)
        {
            NetworkRB.Rigidbody.MovePosition(transform.position + dir * speed * Runner.DeltaTime);
            ManageRotation(dir);
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
        OnTakeDamage(dmg);
        if (Life <= 0)
        {
            GameManager.Instance.RPC_IsMouseDead();
            FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("MouseLoseParent"))
                .FirstOrDefault().gameObject.SetActive(true);
            StartCoroutine(DeadCoroutine());
        }
    }
    IEnumerator DeadCoroutine() 
    {
        yield return new WaitForSeconds(3f);
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
                .Where(x => x.gameObject.name.Equals("MouseWinParent"))
                .FirstOrDefault().gameObject.SetActive(true);
            
        }
    }
}
