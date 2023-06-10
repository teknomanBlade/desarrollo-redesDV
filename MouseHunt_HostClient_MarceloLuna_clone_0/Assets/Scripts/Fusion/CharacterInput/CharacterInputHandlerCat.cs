using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandlerCat : CharacterInputHandler
{
    private CatPlayerModel _catPlayerModel;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("GETTING CAT MODEL...");
        _catPlayerModel = GetComponent<CatPlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputAuthority();
        SetInputsToNetworkVariables();
    }

    public override void CheckInputAuthority()
    {
        if (_catPlayerModel)
        {
            //Debug.Log("SALE ANTES? - CAT MODEL NOT NULL");
            if (!_catPlayerModel.HasInputAuthority)
            {
                //Debug.Log("SALE ANTES? - CAT NOT AUTHORITY INPUT");
                return;
            }
        }
    }
}
