using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class TriggeredAudio : MonoBehaviour {

    //if a sound is dependant on another game object(s)
    public AudioClip clip;
    AudioSource audio;
    bool played;

	// Use this for initialization
	void Start () {
        played = false;
        audio = gameObject.GetComponent<AudioSource>();
	}

    //This is for collider interactions 
    void OnTriggerEnter(Collider collision)
    {
        if(!played && collision.tag == "Player" && gameObject.name != "audio_trigger_4" && played == false)
        {
            print("sound trigger");
            played = true;
            audio.Play();   
        }
        else if(!played && collision.tag == "Player" && gameObject.name == "audio_trigger_4" && played == false)
        {
            print("sound trigger");
            played = true;
            audio.Play();
        }
    }
}
