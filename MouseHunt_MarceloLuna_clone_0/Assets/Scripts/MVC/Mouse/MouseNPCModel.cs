using Fusion;
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
    public Rigidbody RB { get; set; }
    protected float Tick;
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
    void Awake()
    {
        Speed = 9f;
        RB = GetComponent<Rigidbody>();
        View = GetComponent<MouseNPCView>();
        _controller = new MouseNPCController(this, View);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<ThirdPersonCamera>().Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
        }

        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("[Custom Message] Spawned own Player");
        }
        else
        {
            Debug.Log("[Custom Message] Spawned other (Proxy) Player");
        }

        
    }
    public void Move()
    {
        //_currentMove.Move();
    }

    public void TimeTick()
    {
        Tick += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {

        var character = other.gameObject.GetComponent<CatPlayerModel>();
        if (character)
        {
            Debug.Log("MOUSE HITTED...");
            /*if (character.Armor < 0)
            {
                character.TakeDamage(Model.Damage);
                character.gameObject.GetComponent<SpaceShipView>().RepaintLife(character.Life);
            }
            else
            {
                character.TakeShields(Model.Damage);
                character.gameObject.GetComponent<SpaceShipView>().RepaintArmor(character.Armor);
            }*/
            /*if (!PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }*/

        }
    }
}
