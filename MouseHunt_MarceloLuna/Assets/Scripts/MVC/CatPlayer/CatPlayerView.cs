using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatPlayerView : MonoBehaviour
{
    public Animator Animator;
    public SkinnedMeshRenderer MeshRenderer;
    public List<Texture> textures;
    // Start is called before the first frame update
    //public LifeBar Life;

    public CatPlayerModel Model { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    /*public void LoadPlayerSkin()
    {
        PV.RPC("SetPlayerSkin", RpcTarget.Others);
    }

    [PunRPC]
    public void SetPlayerSkin()
    {
        MeshRenderer.material.mainTexture = textures.Where(x => x.name.Contains(transform.tag)).First();
    }*/

    public CatPlayerView SetModel(CatPlayerModel model)
    {
        Model = model;
        Animator = GetComponent<Animator>();
        textures = Resources.LoadAll<Texture>("Textures/CatPlayer").ToList();
        MeshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        //LoadPlayerSkin();
        //SpriteRenderer = GetComponent<SpriteRenderer>();
        //SetHumanConfig();
        //SetIdleAnimation();
        return this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void IdleAnimation()
    {
        Animator.SetBool("IsIdle", true);
    }
    public void RunningAnimation()
    {
        Animator.SetBool("IsRunning", true);
    }
    public void WalkingAnimation()
    {
        Animator.SetBool("IsWalking", true);
    }
    public void AttackingAnimation()
    {
        Animator.SetBool("IsAttacking", true);
    }
    public void StunnedAnimation()
    {
        Animator.SetBool("IsStunned", true);
    }
}
