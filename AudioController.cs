using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioController : MonoBehaviour
{
	public float currentMasterVol;
	public float currentMusicVol;
	public float currentSoundeffectVol;
	public bool Decr;
	
	public float adjust;
	public AudioClip currentMusic;
	public AudioSource audioSource;

	// Sort all background musics by scene id
	public List<AudioClip> audioClips;

	public static AudioController Instance;

	public void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void findMusic(int musicNumber, float delay)
	{
		currentMusic = audioClips[musicNumber];	

		if (delay > 0)
			Invoke("changeMusic", delay);
        else
        {
			audioSource.clip = currentMusic;
			audioSource.Play();
		}
	}

	public void fadeMainOut()
	{
		while (audioSource.volume > 0)
		{
			if (adjust == 0)
				adjust = 1;
			audioSource.volume -= Time.deltaTime * adjust;
		}
	}

	public void fadeMusicOut()
	{
		Decr = true;
	}

	public void changeMusic()
	{
		currentMusicVol = PlayerPrefs.GetFloat("MusicVolumePreference");
		audioSource.volume = PlayerPrefs.GetFloat("MainVolumePreference");
		audioSource.clip = currentMusic;
		audioSource.Play();
	}

    public void FixedUpdate()
    {
        if (Decr && audioSource.volume > 0)
        {
			if (adjust == 0)
				adjust = 1;
			audioSource.volume -= Time.deltaTime * adjust;
		} else {
			Decr = false;
		}
    }
}

// When entering a boss fight, call fadeMusicOut as soon as possible, and when the music is gone, call findMusic.
// When boss fight is finished, call fadeMusicOut, wait for the bosses death animation to finish, wait 1 second and call findMusic.