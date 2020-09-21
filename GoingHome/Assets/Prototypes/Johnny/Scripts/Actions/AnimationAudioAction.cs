using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioAction : Actions
{
    [SerializeField] AudioClip[] audioClips = null;
    [SerializeField] bool isMusic = false;

    private AudioManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = AudioManager.Instance;
    }

    public override void Act()
    {
        if (!isMusic)
            manager.PlaySfx(audioClips[Random.Range(0, audioClips.Length)], transform);
        else
            manager.PlayMusic(audioClips[0]);
    }
}
