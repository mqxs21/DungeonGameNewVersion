using Unity.VisualScripting;
using UnityEngine;

public class SwordSwinger : MonoBehaviour
{
    public Animator sAnim;
    public Animator dummyAnim;

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
        }else  if (Input.GetMouseButton(0) && Input.GetKey("e")  && !imsAnim.GetBool("heavySwordSwinging") && !Input.GetKey(KeyCode.LeftControl)){
            isSwinging = true;
            Debug.Log("heav");
            sAnim.SetBool("heavySwordSwinging", true);
            attackType = "heavy";
        }
        if (Input.GetMouseButtonDown(1)  && !imsAnim.GetBool("swordBlocking") && !Input.GetKey(KeyCode.LeftControl) && !isBlocking){
            
            Debug.Log("block");
            sAnim.SetBool("swordBlocking", true);
        }
        
    }

    public void ActionsAfterDetectHittingEnemy(string objName)
    {
        
        if (isSwinging)
        {
            Debug.Log("hit enemy");
            GameObject obj = GameObject.Find(objName);
            if (objName == "training_dummy" && attackType == "normal")
            {
                dummyAnim.SetBool("isKnocked",true);

            }else if (attackType == "heavy")
            {
                Destroy(obj);
            }
            {
                
            }
            
        }
        else
        {
            Debug.Log("Sword is not swinging.");
        }
    }
}
