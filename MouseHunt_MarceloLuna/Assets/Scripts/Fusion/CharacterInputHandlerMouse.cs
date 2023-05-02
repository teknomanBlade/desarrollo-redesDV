using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandlerMouse : CharacterInputHandler
{
    private MouseNPCModel _mouseNPCModel;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("GETTING MOUSE MODEL...");
        _mouseNPCModel = GetComponent<MouseNPCModel>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputAuthority();
        SetInputsToNetworkVariables();
    }

    public override void CheckInputAuthority()
    {
        if (_mouseNPCModel)
        {
            Debug.Log("SALE ANTES? - MOUSE NOT NULL");
            if (!_mouseNPCModel.HasInputAuthority)
            {
                Debug.Log("SALE ANTES? - MOUSE NOT AUTHORITY INPUT");
                return;
            }
        }
    }
}
