using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLobbyHandler : MonoBehaviour
{
    [Header("Ready Buttons")]
    [SerializeField] Button BTN_Player1Ready;
    [SerializeField] Button BTN_Player2Ready;

    [Header("Ready Images")]
    [SerializeField] Sprite CatReady;
    [SerializeField] Sprite MouseReady;

    // Start is called before the first frame update
    void Start()
    {
        CatReady = Resources.Load<Sprite>("UI/BTN_CatReady_Pressed");
        MouseReady = Resources.Load<Sprite>("UI/BTN_MouseReady_Pressed");
        StartCoroutine(Player1Ready());
        StartCoroutine(Player2Ready());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Player1Ready() 
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        
    }

    IEnumerator Player2Ready()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
       
    }

    public void Player1ReadyCall() 
    {
        BTN_Player1Ready.image.sprite = CatReady;
        BTN_Player1Ready.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(290f, 30f);
        GameManager.Instance.Player1Ready();
    }
    public void Player2ReadyCall()
    {
        BTN_Player2Ready.image.sprite = MouseReady;
        BTN_Player2Ready.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(290f, 30f);
        GameManager.Instance.Player2Ready();
    }
}
