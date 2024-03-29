using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject GameHUDCanvas;
    public GameObject CatSpawner;
    public GameObject MouseSpawner;
    public GameObject CatLobbyModel;
    public GameObject MouseLobbyModel;
    public Button BtnRestart;
    public bool IsPlayer1Ready { get; set; }
    public bool IsPlayer2Ready { get; set; }
    public bool IsMouseDead { get; set; }
    public bool HasMouseReachedGoal { get; set; }
    public List<AudioClip> AudioClips;
    public Dictionary<string, AudioClip> SoundLibrary;
    private AudioClip clip;
    [Header("Ready Buttons")]
    [SerializeField] Button BTN_Player1Ready;
    [SerializeField] Button BTN_Player2Ready;

    [Header("Ready Images")]
    [SerializeField] Sprite CatReady;
    [SerializeField] Sprite MouseReady;
    public string NicknamePlayer1;
    public string NicknamePlayer2;
    public override void Spawned()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
        SoundLibrary = new Dictionary<string, AudioClip>();
        AudioClips = Resources.LoadAll<AudioClip>("Sounds").ToList();
        AudioClips.ForEach(x => SoundLibrary.Add(x.name, x));
        if (Runner.CurrentScene == 2)
        {
            CatSpawner = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("CatSpawner")).FirstOrDefault();
            MouseSpawner = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("MouseSpawner")).FirstOrDefault();
            GameHUDCanvas = FindObjectsOfType<GameObject>(true).Where(x => x.name.Equals("GameHUDCanvas")).FirstOrDefault();
            BtnRestart = GameHUDCanvas.GetComponentsInChildren<Button>(true).Where(x => x.gameObject.name.Equals("BtnRestart")).FirstOrDefault();
            BtnRestart.onClick.AddListener(Restart);
        }
        else if (Runner.CurrentScene == 1)
        {
            CatReady = Resources.Load<Sprite>("UI/BTN_CatReady_Pressed");
            MouseReady = Resources.Load<Sprite>("UI/BTN_MouseReady_Pressed");
            CatLobbyModel = FindObjectsOfType<GameObject>(true).Where(x => x.name.Equals("CatLobbyModel")).FirstOrDefault();
            MouseLobbyModel = FindObjectsOfType<GameObject>(true).Where(x => x.name.Equals("MouseLobbyModel")).FirstOrDefault();
            BTN_Player1Ready = FindObjectsOfType<Button>(true).Where(x => x.name.Equals("BtnReadyPlayer1")).FirstOrDefault();
            BTN_Player2Ready = FindObjectsOfType<Button>(true).Where(x => x.name.Equals("BtnReadyPlayer2")).FirstOrDefault();
            BTN_Player1Ready.onClick.AddListener(Player1Ready);
            BTN_Player2Ready.onClick.AddListener(Player2Ready);

            if (Runner.LocalPlayer == 1)
            {
                BTN_Player2Ready.interactable = false;
            }
            else 
            {
                BTN_Player1Ready.interactable = false;
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        //Debug.Log("<< CHECK BEFORE PLAYERS READY >>");
        if (IsPlayer1Ready && IsPlayer2Ready)
        {
            Debug.Log("<< ESTAN LOS DOS JUGADORES >>");
            //RPC_ShowHideWaitingForPlayers(false);
            Debug.Log("<< SE EMPIEZA EL JUEGO - SE ACTIVAN LOS JUGADORES >>");
            RPC_InitializeGame();
            RPC_ActivatePlayers();
        }
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetNicknamePlayer1(string nick) 
    {
        NicknamePlayer1 = nick;
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SetNicknamePlayer2(string nick)
    {
        NicknamePlayer2 = nick;
    }
    public void Restart() 
    {
        RPC_RepositioningAndActivatePlayers();
    }
    /*[Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendNickname(PlayerModel model, string nick) 
    {
        model.Nickname = nick;
    }*/
    public void Player1Joined() 
    {
        RPC_Player1Joined();
    }
    public void Player2Joined()
    {
        RPC_Player2Joined();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player1Joined() 
    {
        CatLobbyModel.SetActive(true);
        PlaySoundAtPoint("catMeow", CatLobbyModel.transform.position, 0.25f);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player2Joined()
    {
        PlaySoundAtPoint("catHiss", CatLobbyModel.transform.position, 0.25f);
        MouseLobbyModel.SetActive(true);
        PlaySoundAtPoint("mouseStartSqueaks", MouseLobbyModel.transform.position, 0.25f);
    }
    public void Player1Ready() 
    {
        RPC_Player1Ready();
    }
    public void Player2Ready()
    {
        RPC_Player2Ready();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InitializeGame() 
    {
         Runner.SetActiveScene(2);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player1Ready() 
    {
        IsPlayer1Ready = true;
        BTN_Player1Ready.image.sprite = CatReady;
        BTN_Player1Ready.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 30f);
        PlaySoundAtPoint("mouseTrapButtons", CatLobbyModel.transform.position, 0.45f);
        Debug.Log("PLAYER 1 READY: " + IsPlayer1Ready);
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_Player2Ready()
    {
        IsPlayer2Ready = true;
        BTN_Player2Ready.image.sprite = MouseReady;
        BTN_Player2Ready.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(290f, 30f);
        PlaySoundAtPoint("mouseTrapButtons", MouseLobbyModel.transform.position, 0.45f);
        Debug.Log("PLAYER 2 READY: " + IsPlayer2Ready);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_DeactivateBlockedGoal(string name) 
    {
        FindObjectsOfType<MouseHoleGoal>(true)
            .Where(x => x.gameObject.name.Equals(name))
            .FirstOrDefault().gameObject.SetActive(false);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ActivateGameHUD(bool enabled, string name) 
    {
        GameHUDCanvas.GetComponentsInChildren<RectTransform>(true)
                                   .Where(x => x.gameObject.name.Equals("GameHUDPanel"))
                                   .FirstOrDefault().gameObject.SetActive(enabled);
        GameHUDCanvas.GetComponentsInChildren<RectTransform>(true)
                           .Where(x => x.gameObject.name.Equals("GameHUDPanel"))
                           .FirstOrDefault()
                           .GetChild(0).gameObject.SetActive(name.Contains("Cat") ? enabled : !enabled);
    }
    /*public void RPC_ShowHideWaitingForPlayers(bool activate)
    {
        GameHUDCanvas.GetComponentsInChildren<RectTransform>()
                    .Where(x => x.gameObject.name.Equals("GameHUDWaitingForPlayers"))
                    .FirstOrDefault().gameObject.SetActive(activate);
    }*/
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ActivatePlayers()
    {
        FindObjectsOfType<PlayerModel>(true).ToList().ForEach(x => x.gameObject.SetActive(true));
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RepositioningAndActivatePlayers()
    {
        RPC_MouseHasReachedGoalFalse();
        RPC_IsMouseDeadFalse();
        RPC_HideAllWinLosePopups();
        FindObjectsOfType<PlayerModel>(true).ToList().ForEach(x => {
            if (x.gameObject.name.Contains("Cat"))
            {
                x.gameObject.transform.position = CatSpawner.transform.position;
            }
            else 
            {
                x.gameObject.transform.position = MouseSpawner.transform.position;
                x.ResetLife();
            }    
            x.gameObject.SetActive(true); 
        });
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_HideAllWinLosePopups() 
    {
        FindObjectsOfType<RectTransform>(true)
                .Where(x => x.gameObject.name.Equals("GameWinLosePanel"))
                .First().GetComponentsInChildren<RectTransform>().Skip(1).ToList()
                .ForEach(x => 
                {
                    //Debug.Log(x.gameObject.name);
                    if(!x.gameObject.name.Contains("TXT"))
                        x.gameObject.SetActive(false);
                });
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
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

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_IsMouseDeadFalse()
    {
        IsMouseDead = false;
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

    /*private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Level")) 
        {
            if (Object.HasInputAuthority && Object.HasStateAuthority) 
            {
                Spawned();
            }
        }
    }*/
}
