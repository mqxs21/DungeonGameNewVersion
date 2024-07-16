using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  


public class NextLevButton : MonoBehaviour
{
    public GameObject transPanel;

    private AudioSource buttonClickAudio;
    public void NextLevelFunc()
    {
        buttonClickAudio = GetComponent<AudioSource>();
        Debug.Log("Next Level Trigger");
        buttonClickAudio.Play();

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
