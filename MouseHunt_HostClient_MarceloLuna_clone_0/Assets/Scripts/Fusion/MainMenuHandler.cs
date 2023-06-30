using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;

    [Header("Panels")]
    [SerializeField] GameObject _initialPanel;
    [SerializeField] GameObject _nicknamePanel;
    [SerializeField] GameObject _statusPanel;
    [SerializeField] GameObject _sessionBrowserPanel;
    [SerializeField] GameObject _hostGamePanel;

    [Header("Buttons")]
    [SerializeField] Button BTN_JoinLobby;
    [SerializeField] Button BTN_Quit;
    [SerializeField] Button BTN_OpenHostPanel;
    [SerializeField] Button BTN_HostGame;
    [SerializeField] Button BTN_SaveNickname;
    [SerializeField] Button BTN_NicknamePanel;

    [Header("InputFields")]
    [SerializeField] InputField IF_HostSessionName;
    [SerializeField] InputField IF_SetNickname;

    // Start is called before the first frame update
    void Start()
    {
        BTN_JoinLobby.onClick.AddListener(JoinLobby);
        BTN_Quit.onClick.AddListener(QuitGame);
        BTN_OpenHostPanel.onClick.AddListener(ShowHostPanel);
        BTN_HostGame.onClick.AddListener(CreateGameSession);
        BTN_SaveNickname.onClick.AddListener(SetNickname);
        BTN_NicknamePanel.onClick.AddListener(ShowNicknamePanel);

        _networkRunner.OnJoinedLobby += () =>
        {
            _sessionBrowserPanel.SetActive(true);
            _statusPanel.SetActive(false);
        };
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
    private void QuitGame()
    {
        Application.Quit();
    }

    void JoinLobby() 
    {
        _networkRunner.JoinLobby();

        _initialPanel.SetActive(false);
        _statusPanel.SetActive(true);
    }

    void ShowHostPanel() 
    {
        _sessionBrowserPanel.SetActive(false);

        _hostGamePanel.SetActive(true);
    }

    void CreateGameSession() 
    {
        _networkRunner.CreateSession(IF_HostSessionName.text, "Level");
    }

    void ShowNicknamePanel() 
    {
        _nicknamePanel.SetActive(true);
        _sessionBrowserPanel.SetActive(false);
    }

    void SetNickname() 
    {
        _networkRunner.Nick = IF_SetNickname.text;
        _sessionBrowserPanel.SetActive(true);
        _nicknamePanel.SetActive(false);
    }
}
