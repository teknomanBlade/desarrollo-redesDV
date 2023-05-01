using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkHandlerMainMenu : MonoBehaviour
{
    NetworkRunner _runner;
    // Start is called before the first frame update
    void Start()
    {
        _runner = GetComponent<NetworkRunner>();
        DontDestroyOnLoad(this);
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 2) 
        {
            gameObject.AddComponent<SpawnNetworkPlayer>();
        }
    }*/

    Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, SceneRef scene) 
    {
        var sceneObject = runner.GetComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Scene = scene,
            SessionName = "GameSession_MouseHunt",
            SceneManager = sceneObject
        });
    }

    public void PlayGame() 
    {
        var clientTask = InitializeNetworkRunner(_runner, GameMode.Shared, 0);
    }
}
