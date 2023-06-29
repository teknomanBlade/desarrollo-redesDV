using Fusion;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject GameHUDCanvas;
    public GameObject CatSpawner;
    public GameObject MouseSpawner;
    public bool IsMouseDead { get; set; }
    public bool HasMouseReachedGoal { get; set; }
    //public string Nickname;
    public override void Spawned()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;
        CatSpawner = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("CatSpawner")).FirstOrDefault();
        MouseSpawner = FindObjectsOfType<GameObject>().Where(x => x.name.Equals("MouseSpawner")).FirstOrDefault();
        GameHUDCanvas = FindObjectsOfType<GameObject>(true).Where(x => x.name.Equals("GameHUDCanvas")).FirstOrDefault();
        if (Runner.ActivePlayers.ToList().Count > 1) 
        {
            Debug.Log("<< ESTAN LOS DOS JUGADORES >>");
            RPC_ShowHideWaitingForPlayers(false);
            Debug.Log("<< SE EMPIEZA EL JUEGO - SE ACTIVAN LOS JUGADORES >>");
            RPC_ActivatePlayers();
        }
    }

    public override void FixedUpdateNetwork()
    {

    }

    /*[Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendNickname(PlayerModel model, string nick) 
    {
        model.Nickname = nick;
    }*/

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_DeactivateBlockedGoal(string name) 
    {
        FindObjectsOfType<MouseHoleGoal>(true)
            .Where(x => x.gameObject.name.Equals(name))
            .FirstOrDefault().gameObject.SetActive(false);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ShowHideWaitingForPlayers(bool activate)
    {
        GameHUDCanvas.GetComponentsInChildren<RectTransform>()
                    .Where(x => x.gameObject.name.Equals("GameHUDWaitingForPlayers"))
                    .FirstOrDefault().gameObject.SetActive(activate);
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ActivatePlayers()
    {
        FindObjectsOfType<PlayerModel>(true).ToList().ForEach(x => x.gameObject.SetActive(true));
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RepositioningAndActivatePlayers()
    {
        FindObjectsOfType<PlayerModel>(true).ToList().ForEach(x => {
            if (x.gameObject.name.Contains("Cat"))
            {
                x.gameObject.transform.position = CatSpawner.transform.position;
            }
            else 
            {
                x.gameObject.transform.position = MouseSpawner.transform.position;
            }    
            x.gameObject.SetActive(true); 
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
}
