using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNetworkPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    //Prefab del Player
    [SerializeField] CatPlayerModel _catPlayerPrefab;
    [SerializeField] MouseNPCModel _mousePlayerPrefab;
    CatPlayerController _catPlayerController;
    MouseNPCController _mouseController;

    // Start is called before the first frame update
    void Start()
    {
        _catPlayerPrefab = Resources.Load<CatPlayerModel>("CatModel");
        _mousePlayerPrefab = Resources.Load<MouseNPCModel>("Mouse");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!CatPlayerModel.Local) return;

        if (!_catPlayerController) _catPlayerController = CatPlayerModel.Local.GetComponent<CatPlayerController>();
        else input.Set(_catPlayerController.GetInputData());
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared) 
        {
            runner.Spawn(_catPlayerPrefab, Vector3.zero, Quaternion.identity, runner.LocalPlayer);
            Debug.Log("[Custom Message] Connected to Server - Spawning Cat Player");
            
            runner.Spawn(_mousePlayerPrefab, Vector3.zero, Quaternion.identity, runner.LocalPlayer);
            Debug.Log("[Custom Message] Connected to Server - Spawning Mouse Player");
        }
    }
    #region Callbacks sin Usar
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
    #endregion
}
