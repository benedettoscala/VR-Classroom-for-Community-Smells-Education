
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CloudThoughts : UdonSharpBehaviour
{
    public RawImage angryFace;
    public RawImage happyFace;
    public RawImage sadFace;
    public RawImage thinkingFace;

    public RawImage thinkingCloud;

    void Start()
    {
        
    }

    void Update()
    {

    }

    //make a thought
    public void AngryThought()
    {
        angryFace.gameObject.SetActive(true);
        happyFace.gameObject.SetActive(false);
        sadFace.gameObject.SetActive(false);
        thinkingFace.gameObject.SetActive(false);

        thinkingCloud.gameObject.SetActive(true);
    }

    public void HappyThought()
    {
        angryFace.gameObject.SetActive(false);
        happyFace.gameObject.SetActive(true);
        sadFace.gameObject.SetActive(false);
        thinkingFace.gameObject.SetActive(false);

        thinkingCloud.gameObject.SetActive(true);
    }

    public void SadThought()
    {
        angryFace.gameObject.SetActive(false);
        happyFace.gameObject.SetActive(false);
        sadFace.gameObject.SetActive(true);
        thinkingFace.gameObject.SetActive(false);

        thinkingCloud.gameObject.SetActive(true);
    }

    public void ThinkingThought()
    {
        angryFace.gameObject.SetActive(false);
        happyFace.gameObject.SetActive(false);
        sadFace.gameObject.SetActive(false);
        thinkingFace.gameObject.SetActive(true);

        thinkingCloud.gameObject.SetActive(true);
    }

    public void NoThought()
    {
        angryFace.gameObject.SetActive(false);
        happyFace.gameObject.SetActive(false);
        sadFace.gameObject.SetActive(false);
        thinkingFace.gameObject.SetActive(false);
        
        thinkingCloud.gameObject.SetActive(false);
    }

}
