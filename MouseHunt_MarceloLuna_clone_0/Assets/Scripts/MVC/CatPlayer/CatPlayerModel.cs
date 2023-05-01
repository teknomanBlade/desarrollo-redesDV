using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayerModel : NetworkBehaviour
{

    #region Properties
    public static CatPlayerModel Local { get; private set; }
    public Camera Camera;
    private IController _controller;
    public CatPlayerView View { get; private set; }
    public float Speed { get; set; }
    public float RunningSpeed { get; set; }
    public float RotateSpeed { get; set; }
    public float Damage { get; set; }
    public event Action<int> OnMiceCaptured = delegate { };// Es un tipo de delegate que te permite encapsular cualquier metodo que no devuelva ningun valor, y en este caso que pida un int como parametro
    public NetworkRigidbody NetworkRB { get; set; }
    public int MicesCaptured { get; set; }
    public float _lastAttackTime { get; private set; }
    private float AttackRate;
    
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Speed = 8f;
        RunningSpeed = 16f;
        RotateSpeed = 2.1f;
        Damage = 20f;
        AttackRate = 0.35f;
        DontDestroyOnLoad(this);

        NetworkRB = transform.gameObject.GetComponent<NetworkRigidbody>();
        View = GetComponent<CatPlayerView>();
        _controller = new CatPlayerController(this, View);
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
            //Camera.transform.position
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
            NetworkRB.Rigidbody.MovePosition(dir * speed * Runner.DeltaTime);
        }
    }
    public void Attack()
    {
        if (Time.time - _lastAttackTime < AttackRate) return;

        _lastAttackTime = Time.time;

        Debug.Log("ATTACK MOUSE...");
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
