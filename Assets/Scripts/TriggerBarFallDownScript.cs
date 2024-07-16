using System.Collections;
using UnityEngine;

public class TriggerBarFallDownScript : MonoBehaviour
{
    public Animator columnAnimator;
    public Animator cameraAnimator;
    public GameObject crouchTipPanel;

    public GameObject rockParticle;

    public bool passedCrouch;

    void Update(){
        if (passedCrouch)
        {
            crouchTipPanel.SetActive(false);
        }
    }
    void OnTriggerEnter()
    {
        Debug.Log("triggered");
        columnAnimator.SetBool("fall", true);
        cameraAnimator.enabled = true;
        cameraAnimator.SetBool("isShake", true);
        
        rockParticle.SetActive(true);
        StartCoroutine(DisableAnimatorAfterDelay(1f));

        
        
    }

    private IEnumerator DisableAnimatorAfterDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        cameraAnimator.enabled = false;
        
    }
    private IEnumerator FadeOutCrouchTipPanel(float delay){
        yield return new WaitForSeconds(delay);
        crouchTipPanel.GetComponent<Animator>().SetBool("fadeIn",true);
        yield return new WaitForSeconds(delay*1.5f);
        crouchTipPanel.GetComponent<Animator>().SetBool("fadeIn",false);
    }
}
