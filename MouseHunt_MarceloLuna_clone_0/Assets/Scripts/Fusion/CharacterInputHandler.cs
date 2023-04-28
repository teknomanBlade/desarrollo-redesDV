using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    private MouseNPCModel _mouseNPCModel;
    private CatPlayerModel _catPlayerModel;

    private float _xMovement;
    private float _zMovement;
    private bool _isAttackPressed;
    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.name.Contains("Cat"))
        {
            Debug.Log("GETTING CAT MODEL...");
            _catPlayerModel = GetComponent<CatPlayerModel>();
        }
        else 
        {
            Debug.Log("GETTING MOUSE MODEL...");
            _mouseNPCModel = GetComponent<MouseNPCModel>();
        }
    }

    // Update is called once per frame
    void Update()
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
}
