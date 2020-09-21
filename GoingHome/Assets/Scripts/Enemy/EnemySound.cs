using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    public AudioClip[] footstepForward;
    public AudioSource audioSMoveForward;

    public AudioClip[] enemyDeathScream;
    public AudioSource audioSEnemyDeathScream;

    public AudioClip[] enemyDeathFall;
    public AudioSource audioSEnemyDeathFall;

    public AudioClip[] enemyDeathGrounded;
    public AudioSource audioSEnemyDeathGrounded;

    public AudioClip[] enemyTakeDamage;
    public AudioSource audioSEnemyTakeDamage;

    public AudioClip[] enemyAttack;
    public AudioSource audioSEnemyAttack;

    void FootstepForward()
    {
        audioSMoveForward.PlayOneShot(footstepForward[Random.Range(0, footstepForward.Length)]);
    }

    void EnemyDeathScream()
    {
        audioSEnemyDeathScream.PlayOneShot(enemyDeathScream[Random.Range(0, enemyDeathScream.Length)]);
    }

    void EnemyDeathFall()
    {
        audioSEnemyDeathFall.PlayOneShot(enemyDeathFall[Random.Range(0, enemyDeathFall.Length)]);
    }

    void EnemyDeathGrounded()
    {
        audioSEnemyDeathGrounded.PlayOneShot(enemyDeathGrounded[Random.Range(0, enemyDeathGrounded.Length)]);
    }

    void EnemyTakeDamage()
    {
        audioSEnemyTakeDamage.PlayOneShot(enemyTakeDamage[Random.Range(0, enemyTakeDamage.Length)]);
    }

    void EnemyAttack()
    {
        audioSEnemyAttack.PlayOneShot(enemyAttack[Random.Range(0, enemyAttack.Length)]);
    }
}
