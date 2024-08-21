using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class HitBySword : MonoBehaviour
{
    public int hp = 2;
    public Animator cameraAnimator;

    private AudioSource boneCrackAud;

    void Start(){
        boneCrackAud = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("Sword"))
        {
            

            SwordSwinger swordSwinger = collision.collider.GetComponentInParent<SwordSwinger>(); 
            boneCrackAud.Play();

            if (swordSwinger != null)
            {
                
                swordSwinger.ActionsAfterDetectHittingEnemy(name,hp);
            }
            else
            {
                Debug.LogError("Can't find SwordSwinger script");
            }
            /*cameraAnimator.enabled = true;
            cameraAnimator.SetBool("isShake", true);
            StartCoroutine(DisableAnimatorAfterDelay(1f));*/
        }
    }
    private IEnumerator DisableAnimatorAfterDelay(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        cameraAnimator.enabled = false;
        
    }
}
