using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class SwordSwinger : MonoBehaviour
{
    public Animator sAnim;
    public Animator dummyAnim;
    public GameObject particleWood;
    public GameObject particleBlack;
    public GameObject skeletonDeathParticle;
    public Animator imsAnim;

    public bool cAttack = true;
    public bool isSwinging = false;
    public bool isBlocking = false;
    private int now;
    public bool isSwordSwingingSound = false;
    public GameObject skeletonHit;
    public string attackType;
    public bool canClickAgain;
    
    void Update()
    {
        skeletonDeathParticle = GameObject.Find("skeletonDeath");
        skeletonHit = GameObject.Find("skeletonGetHit");
        if (Input.GetMouseButton(0) && !isSwinging && !imsAnim.GetBool("swordBa") && !Input.GetKey("e") && !Input.GetKey(KeyCode.LeftControl) && cAttack)
        {
            isSwinging = true;
            if (!isSwordSwingingSound)
            {
                //GetComponent<AudioSource>().Play();
                isSwordSwingingSound = true;
                StartCoroutine(waitBetweenAttackSounds(0.3f));
            }
            sAnim.SetBool("swordSwinging", true);
            attackType = "normal";
        }
        else if (Input.GetMouseButton(1) && !imsAnim.GetBool("heavySwordSwinging") && !Input.GetKey(KeyCode.LeftControl))
        {
            isSwinging = true;
            //GameObject.Find("Joint").GetComponent<AudioSource>().Play();
            sAnim.SetBool("heavySwordSwinging", true);
            attackType = "heavy";
        }
    }

    public void ActionsAfterDetectHittingEnemy(string objName, int hp)
    {
        if (isSwinging)
        {
            GameObject obj = GameObject.Find(objName);
            
            if (obj != null)
            {

                Vector3 spawnEffectPos = obj.transform.position;
                spawnEffectPos.y = spawnEffectPos.y + 5;
                HitBySword hitBySword = obj.GetComponent<HitBySword>();
                if (hitBySword != null)
                {
                   StartCoroutine(attackFreeze(objName,hitBySword,spawnEffectPos,obj,hp));
                    
                }
                else
                {
                    Debug.LogError("HitBySword component not found on object: " + obj.name);
                }
            }
            else
            {
                Debug.LogError("GameObject with name " + objName + " not found.");
            }
        }
        else
        {
            Debug.Log("Sword is not swinging.");
        }
    }
    IEnumerator attackFreeze(string objName, HitBySword hitBySword, Vector3 spawnEffectPos, GameObject obj, int hp){
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1;
         if (objName == "training_dummy" && attackType == "normal")
                    {
                        dummyAnim.SetBool("isKnocked", true);
                        obj.GetComponent<Collider>().enabled = false;
                    }
                    else if (attackType == "heavy" && objName == "training_dummy")
                    {
                        dummyAnim.SetBool("isKnocked", true);
                        particleBlack.SetActive(true);
                        particleWood.SetActive(true);
                    }
                    else if (obj.CompareTag("Enemy"))
                    {
                        if (attackType == "heavy" && objName != "training_dummy")
                        {
                            Instantiate(skeletonDeathParticle, spawnEffectPos, Quaternion.identity);
                            Instantiate(GameObject.Find("skeletonDrop"),spawnEffectPos,Quaternion.identity);
                            Destroy(obj);
                        }
                        else if (attackType == "normal")
                        {
                            if (hitBySword.hp > 0)
                            {
                                hitBySword.hp--;
                                Instantiate(skeletonHit, spawnEffectPos, Quaternion.identity);
                            }
                            else if (objName != "training_dummy")
                            {
                                Instantiate(skeletonDeathParticle, spawnEffectPos, Quaternion.identity);
                                
                                Destroy(obj);
                            }
                        }
                    }
                    if (hp <= 0 && objName != "training_dummy")
                    {
                        Instantiate(skeletonDeathParticle, spawnEffectPos, Quaternion.identity);
                        Instantiate(GameObject.Find("skeletonDrop"),spawnEffectPos,Quaternion.identity);
                        Destroy(obj);
                    }

    }
    private IEnumerator waitBetweenAttackSounds(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSwordSwingingSound = false;
    }
}
