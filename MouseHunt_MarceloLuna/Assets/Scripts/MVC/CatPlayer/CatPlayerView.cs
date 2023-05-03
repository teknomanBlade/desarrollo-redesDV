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
        var textureSelected = textures[Random.Range(0, textures.Count)];
        Debug.Log("TEXTURE SELECTED: " + textureSelected);
        MeshRenderer.material.mainTexture = textureSelected;
    }


    /*public CatPlayerView SetModel(CatPlayerModel model)
    {
        Model = model;
        textures = Resources.LoadAll<Texture>("Textures/CatPlayer").ToList();
        Animator = GetComponent<Animator>();
        MeshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        return this;
    }*/
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
        StartCoroutine(AttackingAnim());
    }

    IEnumerator AttackingAnim() 
    {
        Animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.35f);
        Animator.SetBool("IsAttacking", false);
    }
    public void StunnedAnimation()
    {
        Animator.SetBool("IsStunned", true);
    }
}
