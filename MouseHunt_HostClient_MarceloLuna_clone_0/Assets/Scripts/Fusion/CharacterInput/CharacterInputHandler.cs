using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    private float _xMovement;
    private float _zMovement;
    private bool _isSprintPressed;
    private bool _isAttackPressed;
    // Start is called before the first frame update
    void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInputsToNetworkVariables()
    {
        //Debug.Log("CARGA INPUTS? - ENTRADA");
        _xMovement = Input.GetAxis("Horizontal");
        //Debug.Log("CARGA INPUTS? - XAXIS - " + _xMovement);
        _zMovement = Input.GetAxis("Vertical");
        //Debug.Log("CARGA INPUTS? - ZAXIS - " + _zMovement);
        _isSprintPressed = Input.GetKey(KeyCode.LeftShift);
        _isAttackPressed = Input.GetMouseButton(0);
        //Debug.Log("CARGA INPUTS? - SALIDA");
    }

    public NetworkInputData GetInputData()
    {
        return new NetworkInputData()
        {
            xMovement = _xMovement,
            zMovement = _zMovement,
            _isSprintPressed = _isSprintPressed,
            _isAttackPressed = _isAttackPressed
        };
    }

    public virtual void CheckInputAuthority()
    {

    }
}
