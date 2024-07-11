using UnityEngine.SceneManagement;
using UnityEngine;


public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider obj){
        if (obj.CompareTag("Player"))
        {
            Debug.Log("Next Level Trigger");
            int nextScene = SceneManager.GetActiveScene().buildIndex+1;
            SceneManager.LoadScene("World1");
        }
        
    }
}
