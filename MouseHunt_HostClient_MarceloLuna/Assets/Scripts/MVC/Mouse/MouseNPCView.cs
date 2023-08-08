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
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
