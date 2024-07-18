using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void OnTriggerEnter(Collider obj){
        if (obj.gameObject.CompareTag("Player") && GameObject.Find("keyRed").GetComponent<collectKey>().haveKey == true)
        {
        GameObject.Find("Jail_Door_Left").GetComponent<Animator>().SetBool("bossRoomOpen",true);
        Destroy(GameObject.Find("Lock"));
        }
    }
}
