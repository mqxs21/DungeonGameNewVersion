using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitter : MonoBehaviour
{
    public GameObject heartCanvas;

    public GameObject enemy;
    public bool isSwingingUpSkeleton;
    void OnTriggerEnter(Collider obj){
       
        if (obj.CompareTag("Player") && enemy.GetComponent<enemyAi>().canAttack)
        {
            Debug.Log("hit player");
            GameObject.Find("heartCanvas").GetComponent<hps>().hpCount--;
            Debug.Log(GameObject.Find("heartCanvas").GetComponent<hps>().hpCount);
            Destroy( GameObject.Find("health ("+GameObject.Find("heartCanvas").GetComponent<hps>().hpCount.ToString()+")"));

            
            }
            }
            
        }
        
    


