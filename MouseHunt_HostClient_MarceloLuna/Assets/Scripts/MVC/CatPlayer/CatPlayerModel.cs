using Fusion;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatPlayerModel : PlayerModel
{

    #region Properties
    public event Action OnIdleAnimation = delegate { };
    public event Action OnIdleFalseAnimation = delegate { };
    public event Action OnWalkingAnimation = delegate { };
    public event Action OnWalkingFalseAnimation = delegate { };
    public event Action OnRunningAnimation = delegate { };
    public event Action OnRunningFalseAnimation = delegate { };
    public event Action OnStunnedAnimation = delegate { };
    public event Action OnStunnedFalseAnimation = delegate { };
    public event Action OnAttackingAnimation = delegate { };
    public event Action OnSetInitialTexture = delegate { };
    public CatPlayerView View { get; private set; }
    public float Damage { get; set; }
    //public event Action<int> OnMiceCaptured = delegate { };// Es un tipo de delegate que te permite encapsular cualquier metodo que no devuelva ningun valor, y en este caso que pida un int como parametro
    
    //public int MicesCaptured { get; set; }
    public float _lastAttackTime { get; private set; }
    
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Speed = 2f;
        RunningSpeed = 4.5f;
        Damage = 20f;
        NetworkRB = transform.gameObject.GetComponent<NetworkRigidbody>();
        View = GetComponent<CatPlayerView>();
        _controller = new CatPlayerController(this, View);
        OnIdleAnimation();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_OnSetInitialTexture()
    {
        OnSetInitialTexture();
    }

    public override void SetInitialTexture()
    {
        RPC_OnSetInitialTexture();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        _controller.OnUpdate();
    }

    public void PlayerActions() 
    {
        var input = GetInput(out NetworkInputData networkInputData);
        //Debug.Log("MOVEMENT SPEED ACTIONS CAT..." + input);
        if (input)
        {
            Dir = new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement);
            if (networkInputData._isSprintPressed)
            {
                //Debug.Log("MOVEMENT SPEED RUNNING CAT...");
                OnRunningAnimation();
                Movement(Dir, RunningSpeed);
            }
            else
            {
                //Debug.Log("MOVEMENT SPEED NORMAL CAT...");
                OnWalkingAnimation();
                OnRunningFalseAnimation();
                Movement(Dir, Speed);
            }

            if (networkInputData._isAttackPressed) 
            {
                Attack();
            }
        }
    }

    public void Movement(Vector3 dir, float speed) 
    {
        if (dir.magnitude != 0)
        {
            dir.Normalize();
            NetworkRB.Rigidbody.MovePosition(transform.position + Runner.DeltaTime * speed * dir);
            ManageRotation(dir);
        }
        else 
        {
            OnIdleAnimation();
            OnWalkingFalseAnimation();
        }
    }
    
    public void Attack()
    {
        OnAttackingAnimation();
    }

    public override CharacterInputHandler GetInputHandler()
    {
        return GetComponent<CharacterInputHandlerCat>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out MouseNPCModel mouseNPCModel))
        {
            //Debug.Log("MOUSE HITTED - CAT...");
            mouseNPCModel.TakeDamage(Damage);
        }
    }
}
