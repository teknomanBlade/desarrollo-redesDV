using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoleGoal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (!Object || !Object.HasStateAuthority) return;

        if (other.gameObject.layer == 7)
        {
            if (gameObject.name.Equals("MouseGoal1"))
            {
                GameManager.Instance.RPC_DeactivateBlockedGoal(gameObject.name);
                Debug.Log("<< GOAL 1 BLOCKED >>");
            }
            else 
            {
                GameManager.Instance.RPC_DeactivateBlockedGoal(gameObject.name);
                Debug.Log("<< GOAL 2 BLOCKED >>");
            }
            
        }
    }
}
