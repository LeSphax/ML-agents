using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance = null;

    public AudioSource musicSource;
    public AudioSource catapultOneSource;
    public AudioSource catapultTwoSource;
    public AudioSource corpseSource;

    private AudioClip music;
    private AudioClip charge;
    private AudioClip shoot;
    private AudioClip turn;
    private AudioClip reload;
    private AudioClip[] impact;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadAudioClips();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlayBackgroundMusic();
        }    
    }

    private void LoadAudioClips()
    {
        music   = Resources.Load<AudioClip>("Audio/Background Music/Music");
        charge  = Resources.Load<AudioClip>("Audio/Catapult/Charge/Charge");
        shoot   = Resources.Load<AudioClip>("Audio/Catapult/Shoot/Shoot");
        turn    = Resources.Load<AudioClip>("Audio/Catapult/Turn/Turn");
        impact  = Resources.LoadAll<AudioClip>("Audio/Corpse/Impact/");
        reload  = Resources.Load<AudioClip>("Audio/Catapult/Reload/Reload");
    }

    public void PlayReloadSound(int catapultNumber)
    {
        switch (catapultNumber)
        {
            case 1:
                catapultOneSource.PlayOneShot(reload);
                break;

            case 2:
                catapultTwoSource.PlayOneShot(reload);
                break;

            default:
                break;
        }
    }

    public void PlayBackgroundMusic()
    {
        musicSource.PlayOneShot(music);
    }

    public void PlayChargeSound(int catapultNumber)
    {
        switch (catapultNumber)
        {
            case 1:
                catapultOneSource.PlayOneShot(charge);
                break;

            case 2:
                catapultTwoSource.PlayOneShot(charge);
                break;

            default:
                break;
        }
    }

    public void StopChargeSound(int catapultNumber)
    {
        switch (catapultNumber)
        {
            case 1:
                catapultOneSource.Stop();
                break;

            case 2:
                catapultTwoSource.Stop();
                break;

            default:
                break;
        }
    }

    public void PlayShootSound(int catapultNumber)
    {
        switch (catapultNumber)
        {
            case 1:
                catapultOneSource.PlayOneShot(shoot);
                break;

            case 2:
                catapultTwoSource.PlayOneShot(shoot);
                break;

            default:
                break;
        }
    }

    public void PlayTurnSound(int catapultNumber)
    {
        switch (catapultNumber)
        {
            case 1:
                if (!catapultOneSource.isPlaying)
                {
                    catapultOneSource.PlayOneShot(turn);
                }
                break;

            case 2:
                if (!catapultTwoSource.isPlaying)
                {
                    catapultTwoSource.PlayOneShot(turn);
                }
                break;

            default:
                break;
        }
    }

    public void PlayImpactSound()
    {
        AudioClip currentImpact;

        currentImpact = impact[Random.Range(0, impact.Length)];

        corpseSource.PlayOneShot(currentImpact, Random.Range(0.9f, 1.0f));
    }
}
