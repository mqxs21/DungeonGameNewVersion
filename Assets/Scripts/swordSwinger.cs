using Unity.VisualScripting;
using UnityEngine;

public class SwordSwinger : MonoBehaviour
{
    public Animator sAnim;
    public Animator dummyAnim;
    public bool isSwinging = false;

    private int now;

    public bool canClickAgain;

    void Update()
    {
        if (Input.GetMouseButton(0) && !isSwinging)
        {
            isSwinging = true;
            sAnim.SetBool("swordSwinging", true);
        }else{
            Debug.Log("isSwinging is "+isSwinging);
            Debug.Log("canClickagain is "+ canClickAgain);
            Debug.Log("mouseButton is "+Input.GetMouseButton(0));
        }
    }

    public void ActionsAfterDetectHittingEnemy(string objName)
    {

        if (isSwinging)
        {
            Debug.Log("hit enemy");
            GameObject obj = GameObject.Find(objName);
            if (objName == "training_dummy")
            {
                dummyAnim.SetBool("isKnocked",true);
            }else
            {
                Destroy(obj);
            }
            
        }
        else
        {
            Debug.Log("Sword is not swinging.");
        }
    }
}
