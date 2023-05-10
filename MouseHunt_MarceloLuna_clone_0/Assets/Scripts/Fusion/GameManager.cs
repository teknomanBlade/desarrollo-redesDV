using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsMouseDead { get; set; }
    public bool HasMouseReachedGoal { get; set; }

    public override void Spawned()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
    }

    public override void FixedUpdateNetwork()
    {

    }

    [Rpc(RpcSources.All,RpcTargets.All)]
    public void RPC_IsMouseDead() 
    {
        IsMouseDead = true;
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MouseHasReachedGoal() 
    {
        HasMouseReachedGoal = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_MouseHasReachedGoalFalse()
    {
        HasMouseReachedGoal = false;
    }
}
