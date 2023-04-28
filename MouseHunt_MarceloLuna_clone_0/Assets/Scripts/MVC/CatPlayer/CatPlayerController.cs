using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayerController : IController
{
    private CatPlayerModel _m;
    private CatPlayerView _v;

    public CatPlayerController(CatPlayerModel m, CatPlayerView v)
    {
        _m = m;
        _v = v;
    }

    public void OnUpdate()
    {

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
