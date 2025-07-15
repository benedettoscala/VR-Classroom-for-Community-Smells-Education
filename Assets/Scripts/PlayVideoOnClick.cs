using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.Video;
using UnityEngine;

public class PlayVideoOnClick : UdonSharpBehaviour
{
    public GameObject videoPlayer;
    public Button playButton;
    public RenderTexture renderTexture;

    public void onButtonClicked() {
        Debug.Log("Ciao sono qui 2");
        //videoPlayer.targetTexture = renderTexture;
        //videoPlayer.Play();
        videoPlayer.SetActive(true);
   
    }
}
