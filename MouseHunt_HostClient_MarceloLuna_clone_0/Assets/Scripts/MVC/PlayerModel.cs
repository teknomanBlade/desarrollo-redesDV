using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    public static PlayerModel Local { get; private set; }
    public Camera Camera;
    protected IController _controller;
    public NetworkRigidbody NetworkRB { get; set; }
    public float Speed { get; set; }
    public float RunningSpeed { get; set; }
    public float RotateSpeed { get; set; }
    
    protected int _currentSignX;
    protected int _previousSignX;
    protected int _currentSignZ = 0;
    protected int _previousSignZ = 0;
    // Start is called before the first frame update
    void Start()
    {
        RotateSpeed = 720f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void ManageRotation(Vector3 dir) 
    {
        if (dir != Vector3.zero) 
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                Quaternion.LookRotation(dir, Vector3.up), RotateSpeed * Runner.DeltaTime);
        }
    }
    public virtual CharacterInputHandler GetInputHandler() 
    {
        return GetComponent<CharacterInputHandler>();
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
            SetLife();
            Camera = Camera.main;
            Camera.GetComponent<ThirdPersonCamera>().Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (GameManager.Instance && Runner.LocalPlayer.PlayerId == 0)
        {
            //Debug.Log("DEBUG EN EL ANTES PRIMER IF PARA VER SI SE VA ANTES...");
            if (GameManager.Instance.HasMouseReachedGoal)
            {
                FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("CatLoseParent"))
                .FirstOrDefault().gameObject.SetActive(true);
                Debug.Log("EL RATON SE HA ESCAPADO!!");
            }

            Debug.Log("Mouse Dead: " + GameManager.Instance.IsMouseDead);
            if (GameManager.Instance.IsMouseDead)
            {
                Debug.Log("INSIDE GAMEMANAGER CALL...");
                FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("CatWinParent"))
                .FirstOrDefault().gameObject.SetActive(true);
                Debug.Log("EL RATON HIZO KAPUTT...");
            }
            //Debug.Log("DEBUG EN EL MEDIO PARA VER SI SE VA ANTES...");

            //Debug.Log("DEBUG EN EL DESPUES SEGUNDO IF PARA VER SI SE VA ANTES...");
        }
    }
    public virtual void SetLife() 
    {
    
    }

    
}
