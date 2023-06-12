using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public GameObject CatSpawner { get; private set; }
    public GameObject MouseSpawner { get; private set; }
    //Prefab del Player
    [SerializeField] CatPlayerModel _catPlayerPrefab;
    [SerializeField] MouseNPCModel _mousePlayerPrefab;
    public CatPlayerModel CatPlayer { get; set; }
    public MouseNPCModel MousePlayer { get; set; }
    CharacterInputHandler _characterInputHandler;

    // Start is called before the first frame update
    void Awake()
    {
        CatSpawner = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("CatSpawner")).FirstOrDefault();
        MouseSpawner = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("MouseSpawner")).FirstOrDefault();
        _catPlayerPrefab = Resources.Load<CatPlayerModel>("CatModel");
        _mousePlayerPrefab = Resources.Load<MouseNPCModel>("Mouse");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!PlayerModel.Local) return;

        if (!_characterInputHandler) _characterInputHandler = PlayerModel.Local.GetInputHandler();
        else input.Set(_characterInputHandler.GetInputData());
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            if (player.PlayerId == 1)
            {
                runner.Spawn(_catPlayerPrefab, null, null, player);
            }
            else if (player.PlayerId == 2) 
            {
                runner.Spawn(_mousePlayerPrefab, null, null, player);
            }
            
            Debug.Log("[Custom Message] Player Joined - I'm THE LAW!!");
        }
        else 
        {
            Debug.Log("[Custom Message] Player Joined - I'm NOT the Server");
        }
    }
    #region Unused Callbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        throw new NotImplementedException();
    }

  

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    #endregion
}
