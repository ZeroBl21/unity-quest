using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip musicClip;
    public float fadeInDuration = 3f;

    private AudioSource audioSource;
    private float targetVolume;

    private static MusicPlayer instance;

    void Awake()
    {
        if (instance == null)
        {
            // Primera instancia, se guarda y no se destruye al cambiar escena
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // Si la música es distinta, reemplaza la anterior
            if (musicClip != null && instance.musicClip != musicClip)
            {
                // Reemplazar la instancia anterior
                Destroy(instance.gameObject);

                instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                // Si es la misma música, destruir esta nueva instancia
                Destroy(gameObject);
                return;
            }
        }

        audioSource = GetComponent<AudioSource>();
        targetVolume = audioSource.volume;
        audioSource.clip = musicClip;
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInDuration);
            audioSource.volume = t * targetVolume;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu")
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
