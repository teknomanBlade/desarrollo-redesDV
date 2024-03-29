using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlayerModel : NetworkBehaviour
{
    public static PlayerModel Local { get; private set; }
    protected Vector3 Dir;
    public Camera Camera;
    public PostProcessVolume volume;
    private Coroutine StunnedPlayerCoroutine;
    private Coroutine UnstunnedPlayerCoroutine;
    public StunnedPlayerPPSSettings postProcessStunnedPlayer;
    public event Action OnLeft = delegate { };
    private NicknameText _myNickname;
    public NicknameText MyNickname { get { return _myNickname; }  set { _myNickname = value; } }
    [Networked(OnChanged = nameof(OnNicknameChanged))] 
    public NetworkString<_16> Nickname { get; set; }

    protected IController _controller;
    public NetworkRigidbody NetworkRB { get; set; }
    public float Speed { get; set; }
    public float RunningSpeed { get; set; }
    public float RotateSpeed { get; set; }
    
    protected int _currentSignX;
    protected int _previousSignX;
    protected int _currentSignZ = 0;
    protected int _previousSignZ = 0;
    protected bool _isFirst;
    // Start is called before the first frame update
    void Start()
    {
        _isFirst = true;
        RotateSpeed = 720f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    protected void ManageRotation(Vector3 dir) 
    {
        if (dir != Vector3.zero) 
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                Quaternion.LookRotation(dir, Vector3.up), RotateSpeed * Runner.DeltaTime);
        }
    }
    public virtual CharacterInputHandler GetInputHandler() 
    {
        return GetComponent<CharacterInputHandler>();
    }
    public override void Spawned()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(SetNickname());
        SetInitialTexture();
        if (Object.HasInputAuthority)
        {
            Local = this;
            Camera = Camera.main;
            var ThirdPersonCamera = Camera.GetComponent<ThirdPersonCamera>();
            if(ThirdPersonCamera)
                ThirdPersonCamera.Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
            SetPostProcessVolume();
            Debug.Log("[Custom Message] Spawned own Player");
        }
        else
        {
            Debug.Log("[Custom Message] Spawned other (Proxy) Player");
        }

        if (Object.HasStateAuthority)
        {
            SetLife();
        }
    }
    public PlayerModel SetNickname(string nick)
    {
        RPC_SendNickname(nick);
        return this;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendNickname(string nick, RpcInfo info = default)
    {
        Nickname = nick;
    }
    
    public static void OnNicknameChanged(Changed<PlayerModel> changed)
    {
        changed.Behaviour.UpdateNickName(changed.Behaviour.Nickname.ToString());
    }

    void UpdateNickName(string nickname) 
    {
        if(gameObject.activeSelf)
            StartCoroutine(UpdateNick(nickname));
    }
    IEnumerator UpdateNick(string nickname) 
    {
        yield return new WaitUntil(() => MyNickname != null);
        MyNickname.UpdateNickname(nickname);
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
    public override void FixedUpdateNetwork()
    {
        if (GameManager.Instance && Runner.LocalPlayer.PlayerId == 0)
        {
            //Debug.Log("DEBUG EN EL ANTES PRIMER IF PARA VER SI SE VA ANTES...");
            if (GameManager.Instance.HasMouseReachedGoal)
            {
                GameManager.Instance.GameHUDCanvas.GetComponentsInChildren<RectTransform>(true)
                    .Where(x => x.gameObject.name.Equals("Player1Lose"))
                    .FirstOrDefault().gameObject.SetActive(true);
                GameManager.Instance.GameHUDCanvas.GetComponentsInChildren<RectTransform>(true)
                    .Where(x => x.gameObject.name.Equals("BtnRestart"))
                    .FirstOrDefault().gameObject.SetActive(true);

                DeactivateMouse();
                //Debug.Log("EL RATON SE HA ESCAPADO!!");
            }

            //Debug.Log("Mouse Dead: " + GameManager.Instance.IsMouseDead);
            if (GameManager.Instance.IsMouseDead)
            {
                //Debug.Log("INSIDE GAMEMANAGER CALL...");
                GameManager.Instance.GameHUDCanvas.GetComponentsInChildren<RectTransform>(true)
                    .Where(x => x.gameObject.name.Equals("Player1Win"))
                    .FirstOrDefault().gameObject.SetActive(true);
                GameManager.Instance.GameHUDCanvas.GetComponentsInChildren<RectTransform>(true)
                    .Where(x => x.gameObject.name.Equals("BtnRestart"))
                    .FirstOrDefault().gameObject.SetActive(true);
                //Debug.Log("EL RATON HIZO KAPUTT...");
            }
            //Debug.Log("DEBUG EN EL MEDIO PARA VER SI SE VA ANTES...");

            //Debug.Log("DEBUG EN EL DESPUES SEGUNDO IF PARA VER SI SE VA ANTES...");
        }
    }

    public void ActiveStunnedEffect(Action actionStaggeredOff = null)
    {
        if (Object.HasInputAuthority)
        {
            if (volume.profile.TryGetSettings(out postProcessStunnedPlayer))
            {
                if (StunnedPlayerCoroutine != null) StopCoroutine(StunnedPlayerCoroutine);
                StunnedPlayerCoroutine = StartCoroutine(LerpStunnedEffect(2f, actionStaggeredOff));
            }
        }
    }

    public void ActiveUnstunnedEffect(Action actionStaggeredOff = null)
    {
        if (Object.HasInputAuthority)
        {
            if (volume.profile.TryGetSettings(out postProcessStunnedPlayer))
            {
                if (UnstunnedPlayerCoroutine != null) StopCoroutine(UnstunnedPlayerCoroutine);
                UnstunnedPlayerCoroutine = StartCoroutine(LerpUnstunnedEffect(2f, actionStaggeredOff));
            }
        }
    }

    IEnumerator LerpStunnedEffect(float duration, Action actionStaggeredOff = null)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            postProcessStunnedPlayer._Intensity.value = Mathf.Clamp01(time / duration);
            yield return null;
        }
        ActiveUnstunnedEffect(actionStaggeredOff);
    }

    IEnumerator LerpUnstunnedEffect(float duration, Action actionStaggeredOff = null)
    {
        float time = 0.98f;

        while (time > 0 && time < duration)
        {
            time -= Time.deltaTime;

            postProcessStunnedPlayer._Intensity.value = Mathf.Clamp01(time / duration);
            yield return null;
        }
        actionStaggeredOff();
    }
    public virtual void ResetLife()
    {

    }

    public virtual void SetLife() 
    {
    
    }
    public virtual void SetInitialTexture()
    {

    }

    public virtual void DeactivateMouse() 
    {
    
    }

    public virtual void SetPlayerInSpawner() 
    {
    
    }
    public virtual void SetPlayerNick()
    {

    }

    public virtual void SetPostProcessVolume() 
    {
    
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Level"))
        {
            Spawned();
            StartCoroutine(SetPlayersToSpawner());
        }
    }
    IEnumerator SetPlayersToSpawner()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        SetPlayerInSpawner();
    }
    IEnumerator SetNickname() 
    {
        yield return new WaitUntil(() => NicknamesHandler.Instance != null);
        SetPlayerNick();
    }
}
