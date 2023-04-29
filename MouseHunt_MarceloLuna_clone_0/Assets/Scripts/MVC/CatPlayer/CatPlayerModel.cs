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
    public float Thrust { get; set; }
    public float RunningThrust { get; set; }
    public float RotateThrust { get; set; }
    public event Action<int> OnMiceCaptured = delegate { };// Es un tipo de delegate que te permite encapsular cualquier metodo que no devuelva ningun valor, y en este caso que pida un int como parametro
    public Rigidbody RB { get; set; }
    public int MicesCaptured { get; set; }
    private float Tick;
    private float AttackRate;
    private float _speedH;
    private float _speedV;
    private float _yaw;
    private float _pitch;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        Thrust = 8f;
        RunningThrust = 16f;
        RotateThrust = 2.1f;
        _speedH = 2.0f;
        _speedV = 2.0f;
        _yaw = 0f;
        _pitch = 0f;
        AttackRate = 0.25f;
       
        RB = transform.gameObject.GetComponent<Rigidbody>();
        View = GetComponent<CatPlayerView>();
        _controller = new CatPlayerController(this, View);
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
    //Builder que ejecuta el server al instanciar el model
    /*public CatPlayerModel SetInitialParameters(Player localPlayer)
    {
        Debug.Log("Player:" + localPlayer);
        _owner = localPlayer;

        photonView.RPC("SetLocalParams", localPlayer, MicesCaptured, localPlayer);
        Debug.Log("MicesCaptured:" + MicesCaptured);
        //Paso los parametros (en este caso solo se va a ejecutar en el Avatar del LocalPlayer, o sea, el avatar que ve el jugador que lo maneja)
        //photonView.RPC("ChangeColor", localPlayer);
        return this;
    }*/
    /*void SetLocalParams(int micesCaptured, Player player)
    {
        Debug.Log("INICIO LOCAL PARAMS...");
        MicesCaptured = micesCaptured;
        int playerNumber = 0;
        if (player.ActorNumber != 1)
            playerNumber = player.ActorNumber;

        Debug.Log("Player Number: " + player.ActorNumber + " - " + player.IsMasterClient);
        Debug.Log("Player Tag: " + (playerNumber - 1));
        transform.tag = "Player" + (playerNumber - 1);
        Debug.Log("FIN LOCAL PARAMS...");
    }*/

    public void Move(Vector3 dir)
    {
        RB.AddRelativeForce(dir * Thrust, ForceMode.Force);
        //Debug.Log("CURRENT VELOCITY: " + RB.velocity.magnitude);
        if (RB.velocity.magnitude > 6f)
        {
            RB.velocity *= 0.92f;
            //Debug.Log("ENTRA EN REDUCCION VELOCITY? " + RB.velocity.magnitude);
        }
    }
    public void LookingCamera(Vector3 dir)
    {
        Camera.transform.eulerAngles = dir;
    }
    public void Rotate(float turn)
    {
        RB.AddTorque(transform.up * RotateThrust * turn);
        //Debug.Log("Current Velocidad de giro? " + RB.angularVelocity);
        if (RB.angularVelocity.magnitude > 8f)
        {
            RB.angularVelocity *= 0.25f;
            //Debug.Log("Reduce la Velocidad de giro? " + RB.angularVelocity);
        }
        else if (RB.angularVelocity.magnitude < -8f)
        {
            RB.angularVelocity *= -0.25f;
            //Debug.Log("Reduce la Velocidad de giro? " + RB.angularVelocity);
        }
    }

    public void Attack()
    {
        Debug.Log("ATTACK MOUSE...");
    }

    // Update is called once per frame
    void Update()
    {
        
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
