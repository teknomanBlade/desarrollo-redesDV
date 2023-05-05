using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatPlayerModel : PlayerModel
{

    #region Properties
    public event Action OnIdleAnimation = delegate { };
    public event Action OnWalkingAnimation = delegate { };
    public event Action OnRunningAnimation = delegate { };
    public event Action OnStunnedAnimation = delegate { };
    public event Action OnAttackingAnimation = delegate { };
    public CatPlayerView View { get; private set; }
    public float Damage { get; set; }
    public event Action<int> OnMiceCaptured = delegate { };// Es un tipo de delegate que te permite encapsular cualquier metodo que no devuelva ningun valor, y en este caso que pida un int como parametro
    
    public int MicesCaptured { get; set; }
    public float _lastAttackTime { get; private set; }
    private float AttackRate;
    
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Speed = 3.2f;
        RunningSpeed = 6.5f;
        RotateSpeed = 2.1f;
        Damage = 20f;
        AttackRate = 0.35f;
        NetworkRB = transform.gameObject.GetComponent<NetworkRigidbody>();
        View = GetComponent<CatPlayerView>();
        _controller = new CatPlayerController(this, View);
        OnIdleAnimation();
    }
    
    public override void FixedUpdateNetwork()
    {
        _controller.OnUpdate();
    }

    public void PlayerActions() 
    {
        var input = GetInput(out NetworkInputData networkInputData);
        Debug.Log("MOVEMENT SPEED ACTIONS CAT..." + input);
        if (input)
        {
            if (networkInputData._isSprintPressed)
            {
                Debug.Log("MOVEMENT SPEED RUNNING CAT...");
                OnRunningAnimation();
                Movement(new Vector3(networkInputData.xMovement, 0, networkInputData.zMovement), RunningSpeed);
            }
            else
            {
                Debug.Log("MOVEMENT SPEED NORMAL CAT...");
                OnWalkingAnimation();
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
        if (dir != Vector3.zero)
        {
            NetworkRB.Rigidbody.MovePosition(transform.position + Runner.DeltaTime * speed * dir);
            ManageRotation(dir);
        }
    }
    
    public void Attack()
    {
        if (Time.time - _lastAttackTime < AttackRate) return;

        _lastAttackTime = Time.time;

        Debug.Log("ATTACK MOUSE...");
        OnAttackingAnimation();
        Debug.Log("ACA LA REFERENCIA DE LA VISTA PARA LA ANIMACION");
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
            //gameObject.GetComponent<SpaceShipView>().RepaintLife(character.Life);
        }
    }
}
