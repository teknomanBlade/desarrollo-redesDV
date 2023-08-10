using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<AudioClip> AudioClips;
    public Dictionary<string, AudioClip> SoundLibrary;
    private AudioClip clip;
    public AudioSource AudioSource;

    // Start is called before the first frame update
    void Start()
    {
        BTN_JoinLobby.onClick.AddListener(JoinLobby);
        BTN_Quit.onClick.AddListener(QuitGame);
        BTN_OpenHostPanel.onClick.AddListener(ShowHostPanel);
        BTN_HostGame.onClick.AddListener(CreateGameSession);
        BTN_SaveNickname.onClick.AddListener(SetNickname);
        BTN_NicknamePanel.onClick.AddListener(ShowNicknamePanel);
        AudioSource = GetComponent<AudioSource>();
        _networkRunner.OnJoinedLobby += () =>
        {
            _sessionBrowserPanel.SetActive(true);
            _statusPanel.SetActive(false);
        };
        SoundLibrary = new Dictionary<string, AudioClip>();
        AudioClips = Resources.LoadAll<AudioClip>("Sounds/UI").ToList();
        AudioClips.ForEach(x => SoundLibrary.Add(x.name, x));
        PlaySoundOnce(AudioSource, "mainMenuMusic", 0.75f, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void QuitGame()
    {
        PlaySoundOnce(_networkRunner.AudioSource, "mouseTrapButtons", 0.45f, false);
        Application.Quit();
    }

    void JoinLobby() 
    {
        PlaySoundOnce(_networkRunner.AudioSource, "mouseTrapButtons", 0.45f, false);
        _networkRunner.JoinLobby();

        _initialPanel.SetActive(false);
        _statusPanel.SetActive(true);
    }

    void ShowHostPanel() 
    {
        PlaySoundOnce(_networkRunner.AudioSource, "mouseTrapButtons", 0.45f, false);
        _sessionBrowserPanel.SetActive(false);

        _hostGamePanel.SetActive(true);
    }

    void CreateGameSession() 
    {
        PlaySoundOnce(_networkRunner.AudioSource, "mouseTrapButtons", 0.45f, false);
        _networkRunner.CreateSession(IF_HostSessionName.text, "Lobby");
    }

    void ShowNicknamePanel() 
    {
        PlaySoundOnce(_networkRunner.AudioSource, "mouseTrapButtons", 0.45f, false);
        _nicknamePanel.SetActive(true);
        _sessionBrowserPanel.SetActive(false);
    }

    void SetNickname() 
    {
        PlaySoundOnce(_networkRunner.AudioSource,"mouseTrapButtons", 0.45f, false);
        _networkRunner.Nick = IF_SetNickname.text;

        _sessionBrowserPanel.SetActive(true);
        _nicknamePanel.SetActive(false);
    }

    public void PlaySoundOnce(AudioSource sound, string clipName, float volume, bool loop)
    {
        if (SoundLibrary.TryGetValue(clipName, out clip))
        {
            sound.clip = clip;
            sound.volume = volume;
            sound.loop = loop;
            sound.spatialBlend = 0f;
            sound.PlayOneShot(clip);
        }
    }


    public void PlaySound(AudioSource sound, string clipName, float volume, bool loop, float spatialBlend)
    {
        if (SoundLibrary.TryGetValue(clipName, out clip))
        {
            sound.clip = clip;
            sound.volume = volume;
            sound.loop = loop;
            sound.minDistance = 2f;
            sound.maxDistance = 400f;
            sound.spatialBlend = spatialBlend;
            sound.Play();
        }
    }


    public void PlaySoundAtPoint(string clipName, Vector3 position, float volume)
    {
        if (SoundLibrary.TryGetValue(clipName, out clip))
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }
}
