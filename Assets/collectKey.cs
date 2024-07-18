using UnityEngine;

public class collectKey : MonoBehaviour
{
    public bool haveKey = false;

    void Start()
    {
        GameObject keyGet = GameObject.Find("keyGet");
        if (keyGet != null)
        {
            keyGet.GetComponent<CanvasRenderer>().SetAlpha(0);
        }
    }

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            haveKey = true;

            GameObject keyGet = GameObject.Find("keyGet");
            if (keyGet != null)
            {
                keyGet.GetComponent<CanvasRenderer>().SetAlpha(1);
            }

            // Make keyRed invisible and disable its collider
            gameObject.GetComponent<Renderer>().enabled = false;
            Collider keyCollider = gameObject.GetComponent<Collider>();
            if (keyCollider != null)
            {
                keyCollider.enabled = false;
            }
        }
    }
}
