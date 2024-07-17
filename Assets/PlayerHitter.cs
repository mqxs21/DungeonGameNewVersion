using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitter : MonoBehaviour
{
    public GameObject heartCanvas;

    public GameObject enemy;
    public bool isSwingingUpSkeleton;
    
    public Animator redScreen;

    public bool isAttack;

    public AudioSource playerGetHit;
    void OnTriggerEnter(Collider obj){
       
        if (obj.CompareTag("Player") && enemy.GetComponent<enemyAi>().canAttack)
        {
            //Debug.Log("hit player");
            GameObject.Find("heartCanvas").GetComponent<hps>().hpCount--;
            
            
            //Debug.Log(GameObject.Find("heartCanvas").GetComponent<hps>().hpCount);
            Destroy( GameObject.Find("health ("+GameObject.Find("heartCanvas").GetComponent<hps>().hpCount.ToString()+")"));
            GameObject.Find("getHitScreenRed").GetComponent<Animator>().SetBool("getHitSk",true);
            GetComponent<AudioSource>().Play();
            }
            
            }
            private IEnumerator Delay()
        {
            redScreen.SetBool("getHitSk",true);  
            yield return new WaitForSeconds(1f);
            redScreen.SetBool("getHitSk",false);
            Debug.Log("canRed");
            isAttack = true;
        }
        }
        
    


