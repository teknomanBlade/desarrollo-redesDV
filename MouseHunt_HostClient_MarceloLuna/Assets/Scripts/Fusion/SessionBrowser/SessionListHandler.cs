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
    [SerializeField] GameObject _sessionPrefab;
    [SerializeField] VerticalLayoutGroup _verticalLayout;

    private void Awake()
    {
        _sessionPrefab = Resources.Load<GameObject>("UI/SessionItem");
    }

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
        //Debug.Log("HAS SESSIONS? - COUNT:" + allSessions.Count);
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
        //Debug.Log("SESSION IS LOADED? " + session.Name);
        var sessionItem = Instantiate(_sessionPrefab, _verticalLayout.transform);
        //Debug.Log("SESSION ITEM EXISTS? - " + (sessionItem == null));
        sessionItem.GetComponent<SessionInfoItem>().SetSessionInformation(session);
        sessionItem.GetComponent<SessionInfoItem>().OnJoinSession += (x) => 
        {
            //Debug.Log("<< BEFORE JOIN SESSION >>");
            _networkRunner.JoinSession(x);
            //Debug.Log("<< AFTER JOIN SESSION >>");
        };
    }

    private void NoSessionsFound()
    {
        _statusText.text = "No sessions found.";
        _statusText.gameObject.SetActive(true);
    }
}
