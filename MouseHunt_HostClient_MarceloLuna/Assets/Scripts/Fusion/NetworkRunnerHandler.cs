using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] NetworkRunner _runnerPrefab;
    NetworkRunner _currentRunner;
    public string Nick;
    public event Action OnJoinedLobby;

    public event Action<List<SessionInfo>> OnSessionListUpdate;
    public GameObject GameHUDCanvas { get; private set; }
    Vector3 initialPos = Vector3.zero;
    //Prefab del Player
    [SerializeField] CatPlayerModel _catPlayerPrefab;
    [SerializeField] MouseNPCModel _mousePlayerPrefab;
    public CatPlayerModel CatPlayer { get; set; }
    public MouseNPCModel MousePlayer { get; set; }
    CharacterInputHandler _characterInputHandler;
    // Start is called before the first frame update
    void Awake()
    {
        _runnerPrefab = Resources.Load<NetworkRunner>("Connection/NetworkRunner");
        _catPlayerPrefab = Resources.Load<CatPlayerModel>("CatModel");
        _mousePlayerPrefab = Resources.Load<MouseNPCModel>("Mouse");
        DontDestroyOnLoad(this);
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
        var scenePath = $"Assets/Scenes/{sceneName}.unity";
        //NOTA: para que ande hay que agregarle la extensión a las Scenes.
        //var scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(1); << Este metodo me ayudo a aprenderlo.
        var buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        Debug.Log("BuildIndex: " + buildIndex);
        var clientTask = InitializeSession(_currentRunner,GameMode.Host, sessionName,
            buildIndex);
    }
    public void JoinSession(SessionInfo sessionInfo)
    {
        //Debug.Log("SESSION INFO: " + sessionInfo.Name);
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("BuildIndex JOIN: " + buildIndex);
        var clientTask = InitializeSession(_currentRunner, GameMode.Client, sessionInfo.Name,
            buildIndex);
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
            SceneManager = sceneManager,
            PlayerCount = 2
        });
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //Debug.Log("<< SESSION LIST UPDATED >>");
        //Debug.Log("HAS SESSIONS TO SHOW? " + sessionList.Count);
        OnSessionListUpdate?.Invoke(sessionList);
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //Debug.Log("ON INPUT - MAIN MENU SCENE");
        if (!PlayerModel.Local) return;
        if (!_characterInputHandler) _characterInputHandler = PlayerModel.Local.GetComponent<CharacterInputHandler>();
        else input.Set(_characterInputHandler.GetInputData());
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //Debug.Log("PLAYER ID: " + player.PlayerId);
        if (runner.IsServer)
        {
            if (player.PlayerId == 1)
            {
                //Debug.Log("ON PLAYER JOINED - MAIN MENU SCENE - " + player.PlayerId);
                GameManager.Instance.Player1Joined();
                GameManager.Instance.RPC_SetNicknamePlayer1(Nick);
                runner.Spawn(_catPlayerPrefab, initialPos, Quaternion.identity, player).gameObject.SetActive(false);
            }
            else if (player.PlayerId == 0)
            {
                //Debug.Log("ON PLAYER JOINED - MAIN MENU SCENE - " + player.PlayerId);
                GameManager.Instance.Player2Joined();
                GameManager.Instance.RPC_SetNicknamePlayer2(Nick);
                runner.Spawn(_mousePlayerPrefab, initialPos, Quaternion.identity, player).gameObject.SetActive(false);
            }

            Debug.Log("[Custom Message] Player Joined - I'm THE LAW!!");
        }
        else
        {
            if (GameManager.Instance && GameManager.Instance.GameHUDCanvas)
                GameManager.Instance.GameHUDCanvas.GetComponentsInChildren<NicknameText>()
                    .FirstOrDefault(x => x.gameObject.name.Contains("Mouse")).UpdateNickname(Nick);
            ShowLobbyModels();
            Debug.Log("[Custom Message] Player Joined - I'm NOT the Server");
        }
    }
    public void ShowLobbyModels() 
    {
        GameManager.Instance.CatLobbyModel.SetActive(true);
        GameManager.Instance.MouseLobbyModel.SetActive(true);
    }
    #region Unused Callbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

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

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
    #endregion
}
