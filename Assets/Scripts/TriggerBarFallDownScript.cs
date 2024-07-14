using System.Collections;
using UnityEngine;

public class TriggerBarFallDownScript : MonoBehaviour
{
    public Animator columnAnimator;
    public Animator cameraAnimator;
    public GameObject crouchTipPanel;
    void OnTriggerEnter()
    {
        Debug.Log("triggered");
        columnAnimator.SetBool("fall", true);
        cameraAnimator.enabled = true;
        cameraAnimator.SetBool("isShake", true);
        crouchTipPanel.GetComponent<Animator>().SetBool("fadeIn",true);
        StartCoroutine(DisableAnimatorAfterDelay(1.0f));
        
        StartCoroutine(FadeOutCrouchTipPanel(3.0f));
    }

    private IEnumerator DisableAnimatorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraAnimator.enabled = false;
    }
    private IEnumerator FadeOutCrouchTipPanel(float delay){
        yield return new WaitForSeconds(delay);
        crouchTipPanel.GetComponent<Animator>().SetBool("fadeIn",false);
    }
}
