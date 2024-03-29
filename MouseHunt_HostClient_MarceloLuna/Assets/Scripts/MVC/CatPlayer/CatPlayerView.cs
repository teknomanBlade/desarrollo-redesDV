using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatPlayerView : MonoBehaviour
{
    public Animator Animator;
    public SkinnedMeshRenderer MeshRenderer;
    public List<Texture> textures;
    public AudioSource AudioSource;
    // Start is called before the first frame update
    //public LifeBar Life;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetInitialTexture() 
    {
        var textureSelected = textures[Random.Range(0, textures.Count)];
        Debug.Log("TEXTURE SELECTED: " + textureSelected);
        GetComponentInChildren<Renderer>().material.SetTexture("_MainTexture", textureSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayAttackSound()
    {
        GameManager.Instance.PlaySoundOnce(AudioSource, "catAttack", 0.45f, false);
        //StartCoroutine(AttackSoundCoroutine());
    }
    IEnumerator AttackSoundCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.8f);
        //GameManager.Instance.PlaySoundAtPoint("catAttack", transform.position, 0.45f);
    }
    public void PlayStartMeowSound()
    {
        GameManager.Instance.PlaySoundAtPoint("catStart", transform.position, 0.25f);
    }
    public void IdleAnimation()
    {
        Animator.SetBool("IsIdle", true);
    }
    public void IdleFalseAnimation()
    {
        Animator.SetBool("IsIdle", false);
    }
    public void RunningAnimation()
    {
        Animator.SetBool("IsRunning", true);
    }
    public void RunningFalseAnimation()
    {
        Animator.SetBool("IsRunning", false);
    }
    public void WalkingAnimation()
    {
        Animator.SetBool("IsWalking", true);
    }
    public void WalkingFalseAnimation()
    {
        Animator.SetBool("IsWalking", false);
    }
    public void AttackingAnimation()
    {
        StartCoroutine(AttackingAnim());
    }

    IEnumerator AttackingAnim() 
    {
        Animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.36f);
        Animator.SetBool("IsAttacking", false);
    }
    public void StunnedAnimation()
    {
        Animator.SetBool("IsStunned", true);
    }
    public void StunnedFalseAnimation()
    {
        Animator.SetBool("IsStunned", false);
    }
}
