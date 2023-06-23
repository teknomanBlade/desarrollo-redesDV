using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    const float OFFSET_POS_Y = 2.5f;

    Transform _owner;

    Text _myText;

    public NicknameText SetOwner(PlayerModel owner) 
    {
        _myText = GetComponent<Text>();
        _owner = owner.transform;
        return this;
    }

    public void UpdateNickname(string nick) 
    {
        _myText.text = nick + "\n";
    }

    public void UpdatePosition() 
    {
        transform.position = _owner.position + Vector3.up * OFFSET_POS_Y;
    }
    
}
