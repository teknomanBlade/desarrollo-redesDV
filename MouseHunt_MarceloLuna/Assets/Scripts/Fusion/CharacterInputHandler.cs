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
        //TODO: PROBAR HACER SUBCLASES DE CHARACTERINPUTHANDLER Y OVERRIDEAR LOS METODOS DE CHEQUEO DE INPUT
        _xMovement = Input.GetAxis("Horizontal");
        _zMovement = Input.GetAxis("Vertical");
        _isSprintPressed = Input.GetKey(KeyCode.LeftShift);
        _isAttackPressed = Input.GetMouseButton(0);
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
