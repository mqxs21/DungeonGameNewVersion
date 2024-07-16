using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hps : MonoBehaviour
{
    public int hpCount = 4;
    public GameObject canvasFadeOutGameOver;

    void Update(){
        if (hpCount==0)
        {
            Debug.Log("game over");
            canvasFadeOutGameOver.SetActive(true);
            canvasFadeOutGameOver.GetComponent<Animator>().SetBool("triggerGameOver",true);
            StartCoroutine(delayAnim(2f));
            
        }
    }
    private IEnumerator delayAnim(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        int nextScene = SceneManager.GetActiveScene().buildIndex+1;
        SceneManager.LoadScene(nextScene);
        
    }

}
