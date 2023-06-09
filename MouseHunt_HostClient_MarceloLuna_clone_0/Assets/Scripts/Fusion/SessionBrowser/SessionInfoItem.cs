using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionInfoItem : MonoBehaviour
{
    [SerializeField] Text TXT_SessionName;
    [SerializeField] Text TXT_PlayerCount;
    [SerializeField] Button BTN_JoinSession;

    SessionInfo _sessionInfo;
    public event Action<SessionInfo> OnJoinSession;

    public void SetSessionInformation(SessionInfo session) 
    {
        _sessionInfo = session;

        TXT_SessionName.text = _sessionInfo.Name;
        TXT_PlayerCount.text = $"{_sessionInfo.PlayerCount}/{_sessionInfo.MaxPlayers}";
        BTN_JoinSession.enabled = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
    }

    public void OnClick() 
    {
        OnJoinSession?.Invoke(_sessionInfo);
    }
}
