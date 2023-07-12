using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouseNPCView : MonoBehaviour
{
    public Image MouseLife;
    // Start is called before the first frame update
    void Awake()
    {
        var MouseLifeRect = FindObjectsOfType<RectTransform>(true)
                        .Where(x => x.gameObject.name.Equals("MouseLife"))
                        .FirstOrDefault();
        if(MouseLifeRect)
            MouseLife = MouseLifeRect.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeLife(float damage) 
    {
        MouseLife.fillAmount -= (damage / 100);
    }

}
