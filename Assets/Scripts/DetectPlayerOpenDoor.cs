
using UnityEngine;

public class DetectPlayerOpenDoor : MonoBehaviour
{
    public Animator leftDoor;
    public Animator rightDoor;
    void OnTriggerEnter(Collider obj){
        if (obj.CompareTag("Player"))
        {
            leftDoor.SetBool("playerNear",true);
            rightDoor.SetBool("playerNearRight",true);
        }
        
    }
}
