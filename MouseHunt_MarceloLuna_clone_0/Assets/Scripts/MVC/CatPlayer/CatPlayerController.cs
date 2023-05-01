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
    }

    public void OnUpdate()
    {
        _m.PlayerActions();
    }
}
