
using UnityEngine;

public class collectCoin : MonoBehaviour
{
    public float speed;
    public bool canCollect = false;

    public Rigidbody rb;

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            GameObject.Find("coinTracker").GetComponent<coinTrackerScript>().coinAmount++;
            Instantiate(GameObject.Find("purpleEffects"),GameObject.Find("Joint").transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        Debug.Log("tig");
        Debug.Log(obj.name);
        rb = GetComponent<Rigidbody>();
        if (obj.gameObject.CompareTag("Player"))
        {
            canCollect = true;
        }
    }

    void Update()
    {
        // Ensure the rotation is always -90 degrees on the x-axis
        transform.rotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (canCollect)
        {
            rb.isKinematic = true;
            Debug.Log("comeTowardsPlayer");
            Vector3 goToPos = GameObject.Find("PlayerCamera").transform.position;
            goToPos.y = goToPos.y - 1;
            transform.position = Vector3.Lerp(transform.position, goToPos, Time.deltaTime * speed);
        }
    }
}
