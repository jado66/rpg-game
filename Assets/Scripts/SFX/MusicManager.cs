using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource backgroundMusicSource;

    public List<AudioClip> terraIndoors;
    public List<AudioClip> terraNight;
    public List<AudioClip> terraDay;

    public List<AudioClip> terraUnderground;


    private int currentBackgroundTrack = -1;
    private int lastPlayedTrackIndex = -1;

    public bool isDay;
    public bool isIndoor;

    public bool isGoingUnderground;

    private Coroutine transitionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusicSource = GetComponent<AudioSource>();
        backgroundMusicSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTrack(AudioClip audioClip, float transitionDuration = 1.0f)
    {
        // Debug.Log("Playing new track: " + audioClip.ToString());

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        
        transitionCoroutine = StartCoroutine(TransitionToNewTrack(audioClip, transitionDuration));
    }

    private IEnumerator TransitionToNewTrack(AudioClip newClip, float duration)
    {
        if (backgroundMusicSource.isPlaying)
        {
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                backgroundMusicSource.volume = 1 - (t / duration);
                yield return null;
            }
            backgroundMusicSource.Stop();
        }

        backgroundMusicSource.clip = newClip;
        backgroundMusicSource.Play();

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            backgroundMusicSource.volume = t / duration;
            yield return null;
        }
        
        backgroundMusicSource.volume = 1.0f;
    }

    public bool IsCurrentTrack(AudioClip audioClip)
    {
        return (audioClip == backgroundMusicSource.clip);
    }

    public void ChangeMusic(bool newIsDay, bool newIsIndoor, bool newIsGoingUnderground, float transitionDuration = 3.0f)
    {
        if (newIsIndoor != isIndoor || newIsDay != isDay || isGoingUnderground != newIsGoingUnderground)
        {
            isDay = newIsDay;
            isIndoor = newIsIndoor;
            isGoingUnderground = newIsGoingUnderground;

            List<AudioClip> selectedList = null;

            if (isIndoor)
            {
                selectedList = terraIndoors;
            }
            else if (isGoingUnderground)
            {
                selectedList = terraUnderground;
            }
            else
            {
                selectedList = isDay ? terraDay : terraNight;
            }

            if (selectedList != null && selectedList.Count > 0)
            {
                int newTrackIndex = GetRandomTrackIndex(selectedList.Count);
                while (selectedList.Count > 1 && newTrackIndex == lastPlayedTrackIndex)
                {
                    newTrackIndex = GetRandomTrackIndex(selectedList.Count);
                }

                lastPlayedTrackIndex = newTrackIndex;
                PlayTrack(selectedList[newTrackIndex], transitionDuration);
            }
        }
    }

    private int GetRandomTrackIndex(int listCount)
    {
        return Random.Range(0, listCount);
    }
}
