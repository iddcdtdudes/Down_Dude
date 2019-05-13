using System;
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
        //if (PlayerPrefs.HasKey(PlayerDataManager.instance.m_playerPref_Music))
        //{
        //    //Play("Theme");
        //    if (PlayerPrefs.GetInt(PlayerDataManager.instance.m_playerPref_Music) == 1)
        //    {
        //        Music(true);
        //    }
        //    else
        //    {

        //        Music(false);
        //    }
        //}
        //else
        //{
        //    //Play("Theme");
        //    Music(true);
        //    PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Music, 1);
        //    PlayerPrefs.Save();
        //    Debug.Log("No music key");
        //}
        
	}
	
	public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Cannot find sound: " + name);
            return;
        }
        s.source.Play();
        
    }

    public void Play(string name, Transform sourceTransform, float rangeY)
    {
        if(Mathf.Abs(DudeController.instance.transform.position.y - sourceTransform.position.y) <= rangeY) {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null) {
                Debug.LogWarning("Cannot find sound: " + name);
                return;
            }
            s.source.Play();
        }
    }

    public void StopSound (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Cannot find sound: " + name);
            return;
        }
        s.volume = 0f;
        s.source.Stop();
    }

    public int GetTimeSamples(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {
            Debug.LogWarning("Cannot find sound: " + name);
            return 0;
        }
        return s.source.timeSamples;
    }

    public void SetTimeSamples(string name, int timeSamples)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null) {
            Debug.LogWarning("Cannot find sound: " + name);
            return;
        }
        s.source.timeSamples = timeSamples;
    }

    public int GetTimeSamplesLength(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Cannot find sound: " + name);
            return 0;
        }
        return s.source.clip.samples;
    }

    public void SynchronizeAudio(string prev, string next)
    {
        int prevTimeSamples = AudioManager.instance.GetTimeSamples(prev);
        prevTimeSamples = prevTimeSamples % AudioManager.instance.GetTimeSamplesLength(next);
        AudioManager.instance.SetTimeSamples(next, prevTimeSamples);
    }

    public void Music(bool OnOff)
    {
        string name = "BGM";
        string menu = "Menu";

        Sound s = Array.Find(sounds, sound => sound.name == name);
        Sound s1 = Array.Find(sounds, sound => sound.name == menu);
        if (s == null) {
            Debug.LogWarning("Cannot find sound: " + name);
            return;
        }

        if (OnOff)
        {
            s.source.volume = 0.15f;
            s1.source.volume = 0.15f;
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Music, 1);
            Debug.Log("Music True");
        } 
        else
        {
            s.source.volume = 0f;
            s1.source.volume = 0f;
            PlayerPrefs.SetInt(PlayerDataManager.instance.m_playerPref_Music, 0);
            Debug.Log("Music False");

        }

        PlayerPrefs.Save();
            
    }

    public void GameSound(bool OnOff)
    {
        string laser = "Laser";
        string jetpack = "Jetpack";
        string parachute = "Parachute";
        string timer = "Time Alert";

        Sound s = Array.Find(sounds, sound => sound.name == laser);
        Sound s1 = Array.Find(sounds, sound => sound.name == jetpack);
        Sound s2 = Array.Find(sounds, sound => sound.name == parachute);
        Sound s3 = Array.Find(sounds, sound => sound.name == timer);

        if (OnOff)
        {
            s.source.volume = 0.15f;
            s1.source.volume = 0.097f;
            s2.source.volume = 0.15f;
            s3.source.volume = 0.097f;
        }
        else
        {
            s.source.volume = 0f;
            s1.source.volume = 0f;
            s2.source.volume = 0f;
            s3.source.volume = 0f;
        }
    }

    
}
