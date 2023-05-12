using Fusion;
using System;
using System.Linq;
using UnityEngine;

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
    public CatPlayerView View { get; private set; }
    public float Damage { get; set; }
    //public event Action<int> OnMiceCaptured = delegate { };// Es un tipo de delegate que te permite encapsular cualquier metodo que no devuelva ningun valor, y en este caso que pida un int como parametro
    
    //public int MicesCaptured { get; set; }
    public float _lastAttackTime { get; private set; }
    private float AttackRate;
    
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Speed = 2f;
        RunningSpeed = 4.5f;
        Damage = 20f;
        AttackRate = 0.45f;
        NetworkRB = transform.gameObject.GetComponent<NetworkRigidbody>();
        View = GetComponent<CatPlayerView>();
        _controller = new CatPlayerController(this, View);
        OnIdleAnimation();
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
            if (networkInputData._isSprintPressed)
            {
                //Debug.Log("MOVEMENT SPEED RUNNING CAT...");
                OnRunningAnimation();
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), RunningSpeed);
            }
            else
            {
                //Debug.Log("MOVEMENT SPEED NORMAL CAT...");
                OnWalkingAnimation();
                OnRunningFalseAnimation();
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), Speed);
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
        /*var attackDelta = Time.time - _lastAttackTime;
        //Debug.Log("ATTACK RATE CURRENT BEFORE RETURN: " + attackDelta);
        if (attackDelta < AttackRate) return;

        _lastAttackTime = Time.time;*/
        //Debug.Log("ATTACK RATE CURRENT AFTER RETURN: " + attackDelta);
        //Debug.Log("ATTACK MOUSE...");
        OnAttackingAnimation();
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
            Debug.Log("MOUSE HITTED - CAT...");
            mouseNPCModel.TakeDamage(Damage);
            //Debug.Log("BEFORE GAMEMANAGER CALL...");
            /*Debug.Log("Mouse Dead: " + GameManager.Instance.IsMouseDead);
            if (GameManager.Instance.IsMouseDead)
            {
                Debug.Log("INSIDE GAMEMANAGER CALL...");
                FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("CatWinParent"))
                .FirstOrDefault().gameObject.SetActive(true);
                Debug.Log("EL RATON HIZO KAPUTT...");
            }*/
            //Debug.Log("AFTER GAMEMANAGER CALL...");
        }
    }
}
