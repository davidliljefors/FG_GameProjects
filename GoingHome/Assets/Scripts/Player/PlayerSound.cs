using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{

    public AudioClip[] footstepHands;
    public AudioSource audioSHands;

    public AudioClip[] footstepFeet;
    public AudioSource audioSFeet;

    public AudioClip[] runRise;
    public AudioSource audioSRise;

    public AudioClip[] deathScream;
    public AudioSource audioSDScream;

    public AudioClip[] deathFall;
    public AudioSource audioSDFall;

    public AudioClip[] takeDamage;
    public AudioSource audioSTakeDamage;

    public AudioClip[] attack;
    public AudioSource audioSattack;

    void FootstepHands()
    {
        audioSFeet.PlayOneShot(footstepHands[Random.Range(0, footstepHands.Length)]);
    }

    void FootstepLegs()
    {
        audioSFeet.PlayOneShot(footstepFeet[Random.Range(0, footstepFeet.Length)]);
    }

    void RunRise()
    {
        audioSRise.PlayOneShot(runRise[Random.Range(0, runRise.Length)]);
    }

    void DeathScream()
    {
        audioSDScream.PlayOneShot(deathScream[Random.Range(0, deathScream.Length)]);
    }

    void DeathFall()
    {
        audioSDFall.PlayOneShot(deathFall[Random.Range(0, deathFall.Length)]);
    }

    void TakeDamage()
    {
        audioSTakeDamage.PlayOneShot(takeDamage[Random.Range(0, takeDamage.Length)]);
    }

    void Attack()
    {
        audioSattack.PlayOneShot(attack[Random.Range(0, attack.Length)]);
    }
}
