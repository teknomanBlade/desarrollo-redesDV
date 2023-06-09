using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _currentRunner;

    public event Action OnJoinedLobby;

    public event Action<List<SessionInfo>> OnSessionListUpdate;
    // Start is called before the first frame update
    void Awake()
    {
        _runnerPrefab = Resources.Load<NetworkRunner>("Connection/NetworkRunner");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void JoinLobby() 
    {
        if (_currentRunner) Destroy(_currentRunner.gameObject);

        _currentRunner = Instantiate(_runnerPrefab);
        _currentRunner.AddCallbacks(this);

        var customLobby = JoinLobbyTask();
    }
    async Task JoinLobbyTask() 
    {
        var result = await _currentRunner.JoinSessionLobby(SessionLobby.Custom, "Normal Lobby");

        if (!result.Ok)
        {
            Debug.LogError("[Error Message] Unable to Join Lobby.");
        }
        else 
        {
            Debug.Log("[Custom Msg] Lobby Joined");
            OnJoinedLobby?.Invoke();
        }
    }

    public void CreateSession(string sessionName, string sceneName) 
    {
        var clientTask = InitializeSession(_currentRunner,GameMode.Host, sessionName, 
            SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"));
    }
    public void JoinSession(SessionInfo sessionInfo)
    {
        var clientTask = InitializeSession(_currentRunner, GameMode.Client, sessionInfo.Name, 
            SceneManager.GetActiveScene().buildIndex);
    }

    async Task InitializeSession(NetworkRunner runner, GameMode gameMode, string sessionName, SceneRef sceneRef) 
    {
        var sceneManager = runner.GetComponent<NetworkSceneManagerDefault>();

        var result = await runner.StartGame(new StartGameArgs 
        { 
            GameMode = gameMode,
            Scene = sceneRef,
            SessionName = sessionName,
            CustomLobbyName = "Normal Lobby",
            SceneManager = sceneManager
        });
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        OnSessionListUpdate?.Invoke(sessionList);

        /*if (sessionList.Count > 0) 
        {
            sessionList.ForEach(session => 
            {
                if (session.PlayerCount < session.MaxPlayers) 
                {
                    JoinSession(session);

                    return;
                }
            });
        }

        CreateSession("SessionSarasa", "Level");*/
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

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
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
