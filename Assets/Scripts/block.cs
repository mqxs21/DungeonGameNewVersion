using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swblc : MonoBehaviour
{
    public bool isBlockingalr;
    public Animator sAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b") && !isBlockingalr){
            isBlockingalr = true;
            sAnim.SetBool("swordBlock", true);
        }
    }
}
