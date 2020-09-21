using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene01Sound : MonoBehaviour
{
    public AudioClip[] step;
    public AudioSource audioSStep;

    public AudioClip[] cateyes;
    public AudioSource audioSCateyes;

    public AudioClip[] claws;
    public AudioSource audioSClaws;

    public AudioClip[] squeal;
    public AudioSource audioSSqueal;

    public AudioClip[] footsteps;
    public AudioSource audioSFootsteps;

    public AudioClip[] stomp;
    public AudioSource audioSStomp;

    public AudioClip[] rockCrack;
    public AudioSource audioSRockCrack;

    public AudioClip[] rockslide;
    public AudioSource audioSRockslide;

    public AudioClip[] falling;
    public AudioSource audioSFalling;

    public AudioClip[] screaming;
    public AudioSource audioSScreaming;

    public AudioClip[] conversation01;
    public AudioSource audioSConversation01;

    public AudioClip[] conversation02;
    public AudioSource audioSConversation02;

    public AudioClip[] conversation03;
    public AudioSource audioSConversation03;

    public AudioClip[] conversation04;
    public AudioSource audioSConversation04;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Step()
    {
        audioSStep.PlayOneShot(step[Random.Range(0, step.Length)]);
    }

    void Cateyes()
    {
        audioSCateyes.PlayOneShot(cateyes[Random.Range(0, cateyes.Length)]);
    }

    void Claws()
    {
        audioSClaws.PlayOneShot(claws[Random.Range(0, claws.Length)]);
    }

    void Squeal()
    {
        audioSSqueal.PlayOneShot(squeal[Random.Range(0, squeal.Length)]);
    }

    void Footsteps()
    {
        audioSFootsteps.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)]);
    }

    void Stomp()
    {
        audioSStomp.PlayOneShot(stomp[Random.Range(0, stomp.Length)]);
    }

    void RockCrack()
    {
        audioSRockCrack.PlayOneShot(rockCrack[Random.Range(0, rockCrack.Length)]);
    }

    void Rockslide()
    {
        audioSRockslide.PlayOneShot(rockslide[Random.Range(0, rockslide.Length)]);
    }

    void Falling()
    {
        audioSFalling.PlayOneShot(falling[Random.Range(0, falling.Length)]);
    }

    void Screaming()
    {
        audioSScreaming.PlayOneShot(screaming[Random.Range(0, screaming.Length)]);
    }

    void Conversation01()
    {
        audioSConversation01.PlayOneShot(conversation01[Random.Range(0, conversation01.Length)]);
    }

    void Conversation02()
    {
        audioSConversation02.PlayOneShot(conversation02[Random.Range(0, conversation02.Length)]);
    }

    void Conversation03()
    {
        audioSConversation03.PlayOneShot(conversation03[Random.Range(0, conversation03.Length)]);
    }

    void Conversation04()
    {
        audioSConversation04.PlayOneShot(conversation04[Random.Range(0, conversation04.Length)]);
    }
}
