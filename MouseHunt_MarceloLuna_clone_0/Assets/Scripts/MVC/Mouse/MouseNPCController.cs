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
    }

    public void OnUpdate()
    {
        //_m.Move();
        //_m.TimeTick();
    }
}
