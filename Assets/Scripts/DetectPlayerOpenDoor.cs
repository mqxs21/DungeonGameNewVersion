
using UnityEngine;

public class DetectPlayerOpenDoor : MonoBehaviour
{
    public Animator leftDoor;
    public Animator rightDoor;
    public Collider m_Collider;
    void OnTriggerEnter(Collider obj){
        
        if (obj.CompareTag("Player"))
        {
             m_Collider.enabled = false;
            leftDoor.SetBool("playerNear",true);
            rightDoor.SetBool("playerNearRight",true);
        }
        
    }
}
