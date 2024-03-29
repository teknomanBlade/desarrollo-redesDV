using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouseNPCView : MonoBehaviour
{
    public Animator Animator;
    public Image MouseLife;
    public ParticleSystem HitFeedback;
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMovementSqueaks() 
    {
        StartCoroutine(MovementSqueaksCoroutine());
    }
    IEnumerator MovementSqueaksCoroutine() 
    {
        yield return new WaitForSecondsRealtime(1.5f);
        GameManager.Instance.PlaySoundAtPoint("mouseMovementSqueaks", transform.position, 0.25f);
    }
    public void PlayHittedSound()
    {
        GameManager.Instance.PlaySoundAtPoint("mouseHitted", transform.position, 0.45f);
    }

    public void PlayStartSqueaks() 
    {
       GameManager.Instance.PlaySoundAtPoint("mouseStartSqueaks", transform.position, 0.25f);
    }
    public void SetSpriteLife() 
    {
        MouseLife = FindObjectsOfType<RectTransform>(true)
                        .Where(x => x.gameObject.name.Equals("MouseLife"))
                        .FirstOrDefault().GetComponent<Image>();
    }

    public void TakeLife(float damage) 
    {
        MouseLife.fillAmount -= (damage / 100);
        HitFeedback.Play();
    }
    public void ResetLifeSprite() 
    {
        MouseLife.fillAmount = 1f;
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

}
