using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseNPCModel : PlayerModel
{
    public event Action<float> OnTakeDamage = delegate { };
    public event Action OnStartSqueaksSound = delegate { };
    public event Action OnMovementSqueaksSound = delegate { };
    public event Action OnHittedSound = delegate { };
    public event Action OnSetLifeSprite = delegate { };
    public event Action OnResetLifeSprite = delegate { };
    public event Action OnIdleAnimation = delegate { };
    public event Action OnIdleFalseAnimation = delegate { };
    public event Action OnWalkingAnimation = delegate { };
    public event Action OnWalkingFalseAnimation = delegate { };
    public event Action OnRunningAnimation = delegate { };
    public event Action OnRunningFalseAnimation = delegate { };
    public MouseNPCView View { get; private set; }
    [Networked] float Life { get; set; }
    public bool IsMouseDead { get; set; }
    public bool IsMouseStaggered;
    public float StaggeredCoef { get; set; }
    void Awake()
    {
        Speed = 2.8f;
        RunningSpeed = 4f;
        StaggeredCoef = 1.2f;
        NetworkRB = GetComponent<NetworkRigidbody>();
        View = GetComponent<MouseNPCView>();
        _controller = new MouseNPCController(this, View);
        OnIdleAnimation();
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

    public override void SetPostProcessVolume()
    {
        if (Runner.CurrentScene == 2)
            volume = Camera.gameObject.GetComponent<PostProcessVolume>();
    }
    public override void ResetLife()
    {
        RPC_OnResetLifeSprite();
    }
    public override void SetLife()
    {
        if (Object.HasStateAuthority) 
        {
            Life = 100f;
            if (Runner.CurrentScene == 2)
                RPC_OnSetLifeSprite();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_OnSetLifeSprite()
    {
        OnSetLifeSprite();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_OnResetLifeSprite()
    {
        Life = 100f;
        OnResetLifeSprite();
    }

    public override void SetPlayerInSpawner()
    {
        transform.position = GameManager.Instance.MouseSpawner.transform.position;
        OnStartSqueaksSound();
        GameManager.Instance.RPC_ActivateGameHUD(true, gameObject.name);
    }
    public override void SetPlayerNick()
    {
        if (_isFirst)
        {
            MyNickname = NicknamesHandler.Instance.AddNickname(this);
            SetNickname(GameManager.Instance.NicknamePlayer2);
            _isFirst = false;
        }
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        _controller.OnUpdate();
    }
   
    public void PlayerActions()
    {
        if (IsMouseStaggered)
        {
            Speed = 2.3f;
            RunningSpeed = 3.3f;
            Debug.Log("SPEED REDUCED: " + Speed);
            Debug.Log("RUNNING SPEED REDUCED: " + RunningSpeed);
        }
        var input = GetInput(out NetworkInputData networkInputData);
        //Debug.Log("MOVEMENT SPEED ACTIONS MOUSE..." + input);
        if (input)
        {
            Dir = new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement);
            //OnMovementSqueaksSound();
            if (networkInputData._isSprintPressed)
            {
                OnRunningAnimation();
                //Debug.Log("MOVEMENT SPEED RUNNING MOUSE...");
                Movement(Dir, RunningSpeed);
            }
            else 
            {
                OnWalkingAnimation();
                OnRunningFalseAnimation();
                //Debug.Log("MOVEMENT SPEED NORMAL MOUSE...");
                Movement(Dir, Speed);
            }
        }
    }

    public void Movement(Vector3 dir, float speed)
    {
        if (dir.magnitude != 0)
        {
            NetworkRB.Rigidbody.MovePosition(transform.position + Runner.DeltaTime * speed * dir);
            ManageRotation(dir);
        }
        else
        {
            OnIdleAnimation();
            OnWalkingFalseAnimation();
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
            RPC_DeactivateMouse();
        }
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_OnTakeDamage(float dmg) 
    {
        IsMouseStaggered = true;
        OnTakeDamage(dmg);
        ActiveStunnedEffect(() => { RPC_MouseStaggeredModeOff(); });
        OnHittedSound();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_MouseStaggeredModeOff() 
    {
        IsMouseStaggered = false;
        Speed = 2.8f;
        RunningSpeed = 4f;
        Debug.Log("SPEED RESTORED");
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
