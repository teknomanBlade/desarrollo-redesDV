using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseNPCController : IController
{
    private MouseNPCModel _m;
    private MouseNPCView _v;

    public MouseNPCController(MouseNPCModel m, MouseNPCView v)
    {
        _m = m;
        _v = v;
        _m.OnTakeDamage += _v.TakeLife;
        _m.OnSetLifeSprite += _v.SetSpriteLife;
    }

    public void OnUpdate()
    {
        _m.PlayerActions();
    }
}
