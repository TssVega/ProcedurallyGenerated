using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string audioName;                                    // Name of the audio
    public AudioClip audioClip;                                 // Audio file

    [Range(0f, 1f)] public float volume = 1f;                   // Set the normal volume
    [SerializeField] private float currentVolume = 1f;
    [Range(0.2f, 1.8f)] public float pitch = 1f;                // Set the normal pitch
    [SerializeField] private float currentPitch = 1f;
    // public float soundEndingOffset = 0.3f;

    public bool looping = false;                                // Set true if the audio is supposed to loop

    public bool randomizeVolume = false;                        // Set true if you want randomized volume
    public bool randomizePitch = false;                         // Set true if you want randomized pitch
    [Range(0f, 0.5f)] public float volumeRandomness = 0.1f;     // Randomness of the volume
    [Range(0f, 0.5f)] public float pitchRandomness = 0.1f;      // Randomness of the pitch

    public AudioSource audioSource;

    // Set the audio source
    public void SetSource(AudioSource _source) {
        _source.loop = looping;
        audioSource = _source;
        audioSource.clip = audioClip;
    }

    public void ChangeVolume(float distance) {
        // If the current values are not the default values, set them
        if(currentVolume != volume) {
            currentVolume = volume;
        }
        // Randomize current value
        currentVolume =
            randomizeVolume ? currentVolume *= 1 +
            (Random.Range(-volumeRandomness / 2, volumeRandomness / 2)) : currentVolume;
        // These statemens are keeping the current volume between randomness offsets
        if(currentVolume > volume + volumeRandomness && randomizeVolume) {
            currentVolume = volume + volumeRandomness;
        }
        if(currentVolume < volume - volumeRandomness && randomizeVolume) {
            currentVolume = volume - volumeRandomness;
        }
        if(distance > 5) {
            audioSource.volume = currentVolume / distance * 0.2f;
        }
        else if(distance > 20) {
            audioSource.volume = 0f;
        }
        else {
            audioSource.volume = currentVolume;
        }
    }

    public void ChangePitch() {
        // If the current values are not the default values, set them
        if(currentPitch != pitch) {
            currentPitch = pitch;
        }
        // Randomize current value
        currentPitch =
            randomizePitch ? currentPitch *= 1 +
            (Random.Range(-pitchRandomness / 2, pitchRandomness / 2)) : currentPitch *= 1;
        // These statemens are keeping the current volume between randomness offsets
        if(currentPitch > pitch + pitchRandomness && randomizePitch) {
            currentPitch = pitch + pitchRandomness;
        }
        if(currentPitch < pitch - pitchRandomness && randomizePitch) {
            currentPitch = pitch - pitchRandomness;
        }
    }

    public void Play(float distance) {
        // Change the randomness
        ChangeVolume(distance);
        ChangePitch();
        // Set the values
        audioSource.volume = currentVolume;
        audioSource.pitch = currentPitch;
        // Play
        audioSource.Play();
    }

    public void Stop() {
        audioSource.Stop();
    }
}