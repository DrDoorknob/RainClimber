using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShuffleDeck : MonoBehaviour {

    AudioSource source;
    public AudioClip[] clips;
    public Queue<AudioClip> shuffledClips;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        Reshuffle();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Reshuffle()
    {
        AudioClip[] clipsCpy = new AudioClip[clips.Length];
        System.Array.Copy(clips, clipsCpy, clips.Length);
        for (var i = clips.Length - 1; i >= 0; i--)
        {
            var j = Mathf.FloorToInt(Random.Range(0, i + 1));
            var temp = clipsCpy[j];
            clipsCpy[j] = clipsCpy[i];
            clipsCpy[i] = temp;
        }
        shuffledClips = new Queue<AudioClip>(clipsCpy);
    }

    public void PlayOne()
    {
        source.Stop();
        source.clip = shuffledClips.Dequeue();
        source.Play();
        if (shuffledClips.Count == 0)
        {
            Reshuffle();
        }
    }
}
