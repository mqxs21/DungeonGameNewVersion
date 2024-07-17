
using UnityEngine;

public class collectCoin : MonoBehaviour
{
    void OnCollisionEnter(Collision obj){
        if (obj.gameObject.CompareTag("Player"))
        {
            GameObject.Find("coinTracker").GetComponent<coinTrackerScript>().coinAmount++;
            
            Destroy(gameObject);
            
        }
    }
}
