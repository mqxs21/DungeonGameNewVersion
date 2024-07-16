using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class SwordSwinger : MonoBehaviour
{
    public Animator sAnim;
    public Animator dummyAnim;
    public GameObject particleWood;
    public GameObject particleBlack;
    public Animator imsAnim;
    public bool isSwinging = false;

    public bool isBlocking = false;
    private int now;

    public string attackType;
    public bool canClickAgain;
    
    void Update()
    {
        if (Input.GetMouseButton(0) && !isSwinging && !imsAnim.GetBool("swordBa") && !Input.GetKey("e") && !Input.GetKey(KeyCode.LeftControl))
        {
            
            isSwinging = true;
            sAnim.SetBool("swordSwinging", true);
    
            attackType = "normal";
        }else  if (Input.GetMouseButton(1) && !imsAnim.GetBool("heavySwordSwinging") && !Input.GetKey(KeyCode.LeftControl)){
            isSwinging = true;
            Debug.Log("heav");
            sAnim.SetBool("heavySwordSwinging", true);
            attackType = "heavy";
        }
        /*if (Input.GetMouseButtonDown(1))
        {
            // Only set the parameter if the character is not already blocking
            sAnim.SetBool("swordAgain", false);
            if (!sAnim.GetBool("swordIsBlocking"))
            {
                Debug.Log("block");
                sAnim.SetBool("swordIsBlocking", true);
                
            }
        }
        
        // Check if the right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            
            Debug.Log("block down");
            sAnim.SetBool("swordIsBlocking", false);

        }*/
        
    }
     
    public void ActionsAfterDetectHittingEnemy(string objName, int hp)
{
    
    if (isSwinging)
    {
        Debug.Log("hit " + objName);
        Debug.Log(hp);
        GameObject obj = GameObject.Find(objName);
        

        // Ensure obj is not null before accessing its components or properties
        if (obj != null)
        {
            // Attempt to get the HitBySword component
            HitBySword hitBySword = obj.GetComponent<HitBySword>();

            // Check if the component was found
            if (hitBySword != null)
            {
                if (objName == "training_dummy" && attackType == "normal")
                {
                    dummyAnim.SetBool("isKnocked", true);
                    obj.GetComponent<Collider>().enabled = false;
                }
                else if (attackType == "heavy" && objName == "training_dummy")
                {
                    Destroy(obj);
                    particleBlack.SetActive(true);
                    particleWood.SetActive(true);
                }
                else if (obj.CompareTag("Enemy"))
                {
                    if (attackType == "heavy")
                    {
                        Destroy(obj);

                    }
                    else if (attackType == "normal")
                    {
                        if (hitBySword.hp > 0)
                        {   
                            hitBySword.hp--;
                        }
                        else
                        {
                        Destroy(obj);
                        }

                    }
                }
              if (hp<=0)
              {
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
    

}
