using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    AudioSource audioSorce;
    [SerializeField] AudioClip Sound;
    [SerializeField] float SoundVolume = 0.35f;
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        audioSorce = GetComponent<AudioSource>();
        audioSorce.PlayOneShot(Sound, SoundVolume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
