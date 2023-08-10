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
        _m.OnWalkingAnimation += _v.WalkingAnimation;
        _m.OnWalkingFalseAnimation += _v.WalkingFalseAnimation;
        _m.OnIdleAnimation += _v.IdleAnimation;
        _m.OnIdleFalseAnimation += _v.IdleFalseAnimation;
        _m.OnRunningAnimation += _v.RunningAnimation;
        _m.OnRunningFalseAnimation += _v.RunningFalseAnimation;
        _m.OnTakeDamage += _v.TakeLife;
        _m.OnSetLifeSprite += _v.SetSpriteLife;
        _m.OnStartSqueaksSound += _v.PlayStartSqueaks;
        _m.OnMovementSqueaksSound += _v.PlayMovementSqueaks;
        _m.OnHittedSound += _v.PlayHittedSound;
    }

    public void OnUpdate()
    {
        _m.PlayerActions();
    }
}
