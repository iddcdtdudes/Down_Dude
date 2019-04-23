﻿using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                Play("Theme");
            }
        }
        else
        {
            Play("Theme");
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.Save();
            Debug.Log("No music key");
        }
        
	}
	
	public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Cannot find sound");
            return;
        }
        s.source.Play();
        
    }

    public void Play(string name, Transform sourceTransform, float rangeY)
    {
        if(Mathf.Abs(DudeController.instance.transform.position.y - sourceTransform.position.y) <= rangeY) {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null) {
                Debug.LogWarning("Cannot find sound");
                return;
            }
            s.source.Play();
        }
    }

    public void StopSound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Cannot find sound");
            return;
        }
        s.volume = 0f;
    }

    public void Music(bool OnOff)
    {
        string name = "Theme";
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Cannot find sound");
            return;
        }

        if (OnOff)
        {
            s.source.volume = 0.15f;
            PlayerPrefs.SetInt("Music", 1);
        } 
        else
        {
            s.source.volume = 0f;
            PlayerPrefs.SetInt("Music", 0);
        }

        PlayerPrefs.Save();
            
    }

    
}
