
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AnimationPapersAudio : UdonSharpBehaviour
{

    public AudioSource audioSource;
    void Start()
    {
        
        audioSource.playOnAwake = false;
    }

    public void PlayAnimationPapersAudio()
    {
        audioSource.Play();
    }

}
