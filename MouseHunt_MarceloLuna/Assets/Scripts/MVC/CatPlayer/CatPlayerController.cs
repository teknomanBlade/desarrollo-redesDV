using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayerController : MonoBehaviour
{
    private float Tick;
    private float AttackRate;
    private float _speedH;
    private float _speedV;
    private float _yaw;
    private float _pitch;
    private float _xMovement;
    private float _zMovement;
    private bool _isAttackPressed;

    private void Start()
    {

        _speedH = 2.0f;
        _speedV = 2.0f;
        _yaw = 0f;
        _pitch = 0f;
        AttackRate = 0.25f;

        //StartCoroutine(SendPackages()); //Simula el Update
    }

    private void Update()
    {
        _xMovement = Input.GetAxis("Horizontal");
        _zMovement = Input.GetAxis("Vertical");
        _isAttackPressed = Input.GetMouseButton(0);
    }

    public NetworkInputData GetInputData() 
    {
        return new NetworkInputData() 
        {
            xMovement = _xMovement,
            zMovement = _zMovement,
            _isAttackPressed = _isAttackPressed
        };
    }

    /*public IEnumerator SendPackages()
    {
        while (true)
        {
            Tick += Time.deltaTime;

            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            var dir = new Vector3(x, 0, z);
            _yaw += _speedH * Input.GetAxis("Mouse X");
            _pitch -= _speedV * Input.GetAxis("Mouse Y");
            var lookingDir = new Vector3(_yaw, _pitch, 0f);
            //MyServer.Instance.RequestRotate(PhotonNetwork.LocalPlayer, x);


            if (Tick > AttackRate && Input.GetMouseButton(0))
            {
                //Debug.Log("TICK? " + Tick);
                Debug.Log("AttackRate: " + AttackRate);
                Tick = 0f;
            }

            yield return new WaitForSeconds(1 / MyServer.Instance.PackagePerSecond); //Determina el "refresco" que va a tener este "Update" para enviar paquetes
        }
    }*/
}
