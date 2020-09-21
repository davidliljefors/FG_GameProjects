using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAction : Actions
{
    [SerializeField] AudioClip[] audioClips = null;
    [SerializeField] bool isMusic = false;

    private AudioManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = AudioManager.Instance;

        if (!isMusic)
            manager.PlaySfx(audioClips[Random.Range(0, audioClips.Length)], transform);
        else
            manager.PlayMusic(audioClips[0]);

        // Play and loop "track 0"
        //manager.PlayMusic(audioClips[0]);

        // Play random track and then loop it
        // manager.PlayMusic(audioClips[Random.Range(0, audioClips.Length)]);
    }

    public override void Act()
    {
        if (!isMusic)
            manager.PlaySfx(audioClips[Random.Range(0, audioClips.Length)], transform);
        else
            manager.PlayMusic(audioClips[0]);
    }

 
}
