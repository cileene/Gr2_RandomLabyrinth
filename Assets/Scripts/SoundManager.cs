using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        var sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();
        Destroy(sfxSource, clip.length);
    }

    public void PlayMusic(AudioClip clip, float volume = 0.5f)
    {
        if (musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}


// // Music in menu
//SoundManager.Instance.PlayMusic(menuMusicClip);

// game music
//SoundManager.Instance.PlayMusic(gameMusicClip);

// stop music
//SoundManager.Instance.StopMusic();

// sfx examples 
//SoundManager.Instance.PlaySfx(iceSoundClip);
//SoundManager.Instance.PlaySfx(fireSoundClip, 0.8f);
//SoundManager.Instance.PlaySfx(fallSoundClip); 
