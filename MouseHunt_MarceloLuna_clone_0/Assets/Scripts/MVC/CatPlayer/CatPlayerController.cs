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
        _m.OnWalkingAnimation += _v.WalkingAnimation;
        _m.OnAttackingAnimation += _v.AttackingAnimation;
        _m.OnIdleAnimation += _v.IdleAnimation;
        _m.OnRunningAnimation += _v.RunningAnimation;
        _m.OnStunnedAnimation += _v.StunnedAnimation;
    }

    public void OnUpdate()
    {
        _m.PlayerActions();
    }
}
