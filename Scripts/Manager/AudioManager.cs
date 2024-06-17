using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : GenericSingleton<AudioManager>
{
    public AudioClip mainSceneMusic;
    public AudioClip[] gameSceneMusicClips;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            PlayMainSceneMusic();
        }
        else if (scene.name == "GameScene")
        {
            StopMainSceneMusic();
            PlayGameSceneMusic();
        }
    }

    public void PlayMainSceneMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = mainSceneMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMainSceneMusic()
    {
        if (audioSource.isPlaying && audioSource.clip == mainSceneMusic)
        {
            audioSource.Stop();
        }
    }

    public void PlayGameSceneMusic()
    {
        StartCoroutine(PlayMusicClipsSequentially());
    }

    private IEnumerator PlayMusicClipsSequentially()
    {
        foreach (var clip in gameSceneMusicClips)
        {
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
        }
    }
}
