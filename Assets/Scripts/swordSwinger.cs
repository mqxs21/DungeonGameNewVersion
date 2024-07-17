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
        if (Input.GetMouseButton(0) && !isSwinging && !imsAnim.GetBool("swordBa") && !Input.GetKey("e") && !Input.GetKey(KeyCode.LeftControl))
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
            Debug.Log("heav");
            //GameObject.Find("Joint").GetComponent<AudioSource>().Play();
            sAnim.SetBool("heavySwordSwinging", true);
            attackType = "heavy";
        }
    }

    public void ActionsAfterDetectHittingEnemy(string objName, int hp)
    {
        if (isSwinging)
        {
            Debug.Log("swordSwingSoundSffect");
            Debug.Log("hit " + objName);
            Debug.Log(hp);
            GameObject obj = GameObject.Find(objName);
            
            if (obj != null)
            {
                Vector3 spawnEffectPos = obj.transform.position;
                spawnEffectPos.y = spawnEffectPos.y + 5;
                HitBySword hitBySword = obj.GetComponent<HitBySword>();
                if (hitBySword != null)
                {
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
                                Instantiate(GameObject.Find("skeletonDrop"),spawnEffectPos,Quaternion.identity);
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

    private IEnumerator waitBetweenAttackSounds(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSwordSwingingSound = false;
    }
}
