using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    const float OFFSET_POS_Y = 2.5f;

    Transform _owner;

    Text _myText;
    RectTransform _rect;
    public NicknameText SetOwner(PlayerModel owner) 
    {
        _myText = GetComponent<Text>();
        _rect = GetComponent<RectTransform>();
        
        if (owner.gameObject.name.Contains("Cat"))
        {
            gameObject.name += " Cat";
            _rect.offsetMax = new Vector2(-2f, -1.5f);
        }
        else 
        {
            gameObject.name += " Mouse";
            _rect.offsetMax = new Vector2(-2f, -35f);
        }
        _owner = owner.transform;
        return this;
    }

    public void UpdateNickname(string nick) 
    {
        _myText.text = nick;
    }

    public void UpdatePosition() 
    {
        transform.position = _owner.position + Vector3.up * OFFSET_POS_Y;
    }
    
}
