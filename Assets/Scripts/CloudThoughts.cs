using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;

public class CloudThoughts : UdonSharpBehaviour
{
    [Header("UI Elements")]
    public RawImage cloud;
    public TextMeshProUGUI text;
    public RawImage thinkingCloud;
    public Image questionMark;
    public Image dontCareMeme;
    public Image jollyCooperation;

    [Header("Emotion Faces")]
    public RawImage angryFace;
    public RawImage happyFace;
    public RawImage sadFace;
    public RawImage thinkingFace;
    public RawImage dizzyFace;
    public RawImage muteFace;

    [Header("Movement Settings")]
    public bool upAndDown = false;
    public float speed = 2.0f;
    public float height = 1.0f;

    private Vector3 startPos;
    private RawImage[] allFaces;

    void Start()
    {
        startPos = transform.position;
        allFaces = new RawImage[] { angryFace, happyFace, sadFace, thinkingFace, dizzyFace, muteFace };
        ResetAllElements();
    }

    void Update()
    {
        FaceLocalPlayer();
        if (upAndDown)
        {
            MoveUpAndDown();
        }
    }

    private void FaceLocalPlayer()
    {
        transform.LookAt(Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
    }

    private void MoveUpAndDown()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void ResetAllElements()
    {
        foreach (var face in allFaces)
        {
            face.gameObject.SetActive(false);
        }
        thinkingCloud.gameObject.SetActive(false);
        cloud.gameObject.SetActive(false);
        questionMark.gameObject.SetActive(false);
        dontCareMeme.gameObject.SetActive(false);
        jollyCooperation.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    private void SetEmotionFace(RawImage faceToActivate)
    {
        ResetAllElements();
        faceToActivate.gameObject.SetActive(true);
        thinkingCloud.gameObject.SetActive(true);
    }

    public void AngryThought() => SetEmotionFace(angryFace);
    public void HappyThought() => SetEmotionFace(happyFace);
    public void SadThought() => SetEmotionFace(sadFace);
    public void ThinkingThought() => SetEmotionFace(thinkingFace);
    public void DizzyThought() => SetEmotionFace(dizzyFace);
    public void MuteThought() => SetEmotionFace(muteFace);

    public void NoThought() => ResetAllElements();

    public void QuestionThought()
    {
        ResetAllElements();
        thinkingCloud.gameObject.SetActive(true);
        questionMark.gameObject.SetActive(true);
    }

    public void setDontCareMeme(bool active)
    {
        ResetAllElements();
        dontCareMeme.gameObject.SetActive(active);
    }

    public void setJollyCooperation(bool active)
    {
        ResetAllElements();
        jollyCooperation.gameObject.SetActive(active);
    }

    public void setText(string message)
    {
        text.gameObject.SetActive(true);
        text.text = message;
    }

    public void activateText() => text.gameObject.SetActive(true);
    public void deactivateText() => text.gameObject.SetActive(false);
}