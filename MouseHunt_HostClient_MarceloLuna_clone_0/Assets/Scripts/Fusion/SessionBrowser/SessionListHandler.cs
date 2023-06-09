using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionListHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;
    [SerializeField] Text _statusText;
    [SerializeField] SessionInfoItem _sessionPrefab;
    [SerializeField] VerticalLayoutGroup _verticalLayout;

    private void OnEnable()
    {
        _networkRunner.OnSessionListUpdate += ReceiveSessionList;
    }
    
    private void OnDisable()
    {
        _networkRunner.OnSessionListUpdate -= ReceiveSessionList;
    }

    private void ClearList() 
    {
        foreach (Transform child in _verticalLayout.transform)
        {
            Destroy(child.gameObject);
        }

        _statusText.gameObject.SetActive(false);
    }

    private void ReceiveSessionList(List<SessionInfo> allSessions)
    {
        ClearList();

        if (allSessions.Count == 0)
        {
            NoSessionsFound();
        }
        else 
        {
            foreach (var session in allSessions)
            {
                AddSessionToList(session);
            }
        }
    }

    private void AddSessionToList(SessionInfo session)
    {
        var sessionItem = Instantiate(_sessionPrefab, _verticalLayout.transform);
        sessionItem.SetSessionInformation(session);
        sessionItem.OnJoinSession += (x) => _networkRunner.JoinSession(x);
    }

    private void NoSessionsFound()
    {
        _statusText.text = "No sessions found.";
        _statusText.gameObject.SetActive(true);
    }
}
