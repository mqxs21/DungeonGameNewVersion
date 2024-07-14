using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  


public class NextLevButton : MonoBehaviour
{
    public GameObject transPanel;
    public void NextLevelFunc()
    {
        Debug.Log("Next Level Trigger");
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        Debug.Log("Waiting for 2 seconds...");
        transPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        Debug.Log("2 seconds passed");
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene("Tutorial");
    }
}
