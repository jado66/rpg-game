using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicTrigger : MonoBehaviour
{
    GameObject player;
    MusicManager musicManager;
    
    public List<AudioClip> audioClips;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag == "Player" && collider.isTrigger){
            swapAudioTracks();
        }
    }

    void swapAudioTracks(){
        Debug.Log("Checking for swap");
        if (musicManager.IsCurrentTrack(audioClips[0])){
            Debug.Log("Music matches playing other track");
            musicManager.PlayTrack(audioClips[1]);
        }
        else
            musicManager.PlayTrack(audioClips[0]);
    }   
}
