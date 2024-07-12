using UnityEngine;
using UnityEngine.UIElements;

public class HitBySword : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("Sword"))
        {
            

            SwordSwinger swordSwinger = collision.collider.GetComponentInParent<SwordSwinger>();

            if (swordSwinger != null)
            {
                swordSwinger.ActionsAfterDetectHittingEnemy(name);
            }
            else
            {
                Debug.LogError("Can't find SwordSwinger script");
            }
        }
    }
}
