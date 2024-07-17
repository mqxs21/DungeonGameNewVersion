using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyAudio : MonoBehaviour
{
    public bool game = false;
    public AudioSource audioSource; // Reference to the AudioSource component
    public float fadeDuration = 1.0f; // Duration of the fade effect

    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game Over" && audioSource.isPlaying)
        {
            StartCoroutine(FadeOut(audioSource, fadeDuration));
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "World1")
        {
            if (!audioSource.isPlaying)
            {
                StartCoroutine(FadeIn(audioSource, fadeDuration));
            }
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        Debug.Log("fade out");

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    IEnumerator FadeIn(AudioSource audioSource, float fadeDuration)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        float targetVolume = 1.0f;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
