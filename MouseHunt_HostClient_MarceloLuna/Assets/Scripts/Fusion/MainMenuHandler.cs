using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;

    [Header("Panels")]
    [SerializeField] GameObject _initialPanel;
    [SerializeField] GameObject _statusPanel;
    [SerializeField] GameObject _sessionBrowserPanel;
    [SerializeField] GameObject _hostGamePanel;

    [Header("Buttons")]
    [SerializeField] Button BTN_JoinLobby;
    [SerializeField] Button BTN_OpenHostPanel;
    [SerializeField] Button BTN_HostGame;

    [Header("InputFields")]
    [SerializeField] InputField IF_HostSessionName;

    [Header("Text")]
    [SerializeField] Text TXT_Status;

    // Start is called before the first frame update
    void Start()
    {
        BTN_JoinLobby.onClick.AddListener(JoinLobby);
        BTN_OpenHostPanel.onClick.AddListener(ShowHostPanel);
        BTN_HostGame.onClick.AddListener(CreateGameSession);

        _networkRunner.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            _sessionBrowserPanel.SetActive(true);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void JoinLobby() 
    {
        _networkRunner.JoinLobby();

        _initialPanel.SetActive(false);
        _statusPanel.SetActive(true);

        TXT_Status.text = "Joining Lobby...";
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
}
