using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance 
    {
        get
        {
            if(s_Instance == null)
            {
                Debug.LogError("AudioManager instance was null");
                return s_Instance;
            }
            return s_Instance;
        }
        private set => s_Instance = value; 
    }

    [SerializeField] AudioMixerGroup sfxGroup, musicGroup;
    [SerializeField] int audioSourceInstances = 5;

    private Queue<AudioSource> sfxLib = new Queue<AudioSource>();
    private AudioSource musicPlayer;

    [Header("Sound effects Gameplay")]
    public Sound[] sounds;
    private static AudioManager s_Instance;

    [HideInInspector] public bool isFadingIn;


    private void Awake()
    {

        Init();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        if (s_Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FadeInMusic()
    {
        isFadingIn = true;
        
        Debug.Log("music fade in");
        while (musicPlayer.volume < 1f && isFadingIn)
        {
            musicPlayer.volume += Time.deltaTime * 0.2f;
            yield return null;
        }

    }
    
    IEnumerator FadeOutMusic()
    {
        isFadingIn = false;

        Debug.Log("music fade out");
        while (musicPlayer.volume > 0.01f && !isFadingIn)
        {
            musicPlayer.volume -= Time.deltaTime * 1f;
            yield return null;
        }
        
        musicPlayer.volume = 0;
        musicPlayer.Stop();
    }


    void Init()
    {
        for (int i = 0; i < audioSourceInstances; i++)
        {
            sfxLib.Enqueue(AudioSourceInstantiate(sfxGroup, true, "AudioSource" + i.ToString("00")));
        }

        musicPlayer = AudioSourceInstantiate(musicGroup, false, "MusicSource");
    }

    AudioSource AudioSourceInstantiate(AudioMixerGroup group, bool sfx, string name = "AudioSource")
    {
        AudioSource audio = new GameObject(name).AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = group;
        audio.spatialBlend = sfx ? 1f : 0f;

        audio.loop = !sfx;

        audio.transform.SetParent(transform);

        return audio;
    }

    public void PlaySfx(AudioClip clip, Transform source = null)
    {
        AudioSource audio;

        if (sfxLib.Count == 0)
        {
            audio = AudioSourceInstantiate(sfxGroup, true, "AudioSource" + audioSourceInstances++);
        }
        else
        {
            audio = sfxLib.Dequeue();
        }

        audio.transform.position = source != null ? source.position : Vector3.zero;

        audio.clip = clip;
        audio.Play();

        audio.transform.SetAsLastSibling(); //for illustrate the dequeue/enqueue process

        sfxLib.Enqueue(audio);
    }

    public void PlayMusic(AudioClip music)
    {
        if (musicPlayer.clip == music)
            return;

        musicPlayer.clip = music;
        musicPlayer.volume = 0f;
        StartCoroutine("FadeInMusic");
        musicPlayer.Play();


    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.LogError("Could not play sound '" + name + "'.");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(FadeOutMusic());
        }
    }

    public void FadeOutTheMusic()
    {
        StartCoroutine("FadeOutMusic");
    }

}

