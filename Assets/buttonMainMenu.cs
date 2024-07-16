
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class buttonMainMenu : MonoBehaviour
{
   public GameObject transPanel;

    private AudioSource buttonClickAudio;
    public void World1Func()
    {

        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        transPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
