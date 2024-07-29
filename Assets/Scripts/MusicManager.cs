using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource backgroundMusicSource;

    public List<AudioClip> backgroundMusics;

    private int currentBackgroundTrack=-1;
    // Start is called before the first frame update
    void Start()
    {
        backgroundMusicSource = GetComponent<AudioSource>();
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void PlayBackgroundMusic(int track){
        if (backgroundMusicSource != null && backgroundMusicSource.clip != null && track != currentBackgroundTrack){
            currentBackgroundTrack = track;
            backgroundMusicSource.Stop();
            backgroundMusicSource.clip = backgroundMusics[track];
            backgroundMusicSource.Play();
        }
    }

    public void PlayTrack(AudioClip audioClip){
        Debug.Log("Playing new track"+audioClip.ToString());
        backgroundMusicSource.Stop();
        backgroundMusicSource.clip = audioClip;
        backgroundMusicSource.Play();
    }

    public bool IsCurrentTrack(AudioClip audioClip){
        return (audioClip == backgroundMusicSource.clip);
    }

}
