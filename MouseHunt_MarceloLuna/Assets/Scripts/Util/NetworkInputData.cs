using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float xMovement;
    public float zMovement;
    public NetworkBool _isAttackPressed;
}
