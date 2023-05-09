using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnNetworkPlayer : MonoBehaviour, INetworkRunnerCallbacks
{
    public GameObject MainMenuCanvas { get; private set; }
    public GameObject GameHUDCanvas { get; private set; }
    public GameObject CatSpawner { get; private set; }
    public GameObject MouseSpawner { get; private set; }
    //Prefab del Player
    [SerializeField] CatPlayerModel _catPlayerPrefab;
    [SerializeField] MouseNPCModel _mousePlayerPrefab;
    CharacterInputHandler _characterInputHandler;

    // Start is called before the first frame update
    void Start()
    {
        MainMenuCanvas = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("MainMenuCanvas")).FirstOrDefault();
        GameHUDCanvas = FindObjectsOfType<GameObject>(true).Where(x => x.name.Equals("GameHUDCanvas")).FirstOrDefault();
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

        if (!_characterInputHandler) _characterInputHandler = PlayerModel.Local.GetComponent<CharacterInputHandler>();
        else input.Set(_characterInputHandler.GetInputData());

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        if (runner.Topology == SimulationConfig.Topologies.Shared) 
        {
            if (runner.LocalPlayer.PlayerId == 0)
            {
                runner.Spawn(_catPlayerPrefab, CatSpawner.transform.position, Quaternion.identity, runner.LocalPlayer);
                ShowHideGameCanvases(true, runner.LocalPlayer.PlayerId == 1);
                Debug.Log("[Custom Message] Connected to Server - Spawning " + _catPlayerPrefab.name);
            }
            else 
            {
                runner.Spawn(_mousePlayerPrefab, MouseSpawner.transform.position, Quaternion.identity, runner.LocalPlayer);
                ShowHideGameCanvases(true, runner.LocalPlayer.PlayerId == 1);
                Debug.Log("[Custom Message] Connected to Server - Spawning " + _mousePlayerPrefab.name);
            }
        }
    }
    public void ShowHideGameCanvases(bool enabled, bool isPlayerTwo) 
    {
        if (MainMenuCanvas)
            MainMenuCanvas.SetActive(!enabled);

        if (GameHUDCanvas) 
        {
            GameHUDCanvas.SetActive(enabled);
            GameHUDCanvas.GetComponentsInChildren<RectTransform>()
                .Where(x => x.gameObject.name.Equals("GameHUDPanel"))
                .FirstOrDefault().gameObject.SetActive(isPlayerTwo);
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
