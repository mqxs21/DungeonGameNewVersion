
using UnityEngine;
using UnityEngine.SceneManagement;
public class DetectPlayerOpenDoor : MonoBehaviour
{
    public Animator leftDoor;
    public Animator rightDoor;
    public Collider m_Collider;
    void OnTriggerEnter(Collider obj){
        
        if (obj.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().name == "World1")
            {
                m_Collider.enabled = false;
            }
             
            leftDoor.SetBool("playerNear",true);
            rightDoor.SetBool("playerNearRight",true);
        }
        
    }
}
