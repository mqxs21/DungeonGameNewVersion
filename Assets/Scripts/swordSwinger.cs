using Unity.VisualScripting;
using UnityEngine;

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
        }else  if (Input.GetMouseButton(0) && Input.GetKey("e")  && !imsAnim.GetBool("heavySwordSwinging") && !Input.GetKey(KeyCode.LeftControl)){
            isSwinging = true;
            Debug.Log("heav");
            sAnim.SetBool("heavySwordSwinging", true);
            attackType = "heavy";
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Only set the parameter if the character is not already blocking
            if (!sAnim.GetBool("swordIsBlocking"))
            {
                Debug.Log("block");
                sAnim.SetBool("swordIsBlocking", true);
            }
        }
        
        // Check if the right mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            // Set the parameter to false to transition back to idle
            Debug.Log("block down");
            sAnim.SetBool("swordIsBlocking", false);
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
                obj.GetComponent<Collider>().enabled = false;

            }else if (attackType == "heavy")
            {
                Destroy(obj);
                particleBlack.SetActive(true);
                particleWood.SetActive(true);
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
