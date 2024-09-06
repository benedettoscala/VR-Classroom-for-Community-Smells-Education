using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class VoiceTextTrigger : UdonSharpBehaviour
{
    public float voiceAmplification = 1.5f;
    public TextMeshProUGUI followText;
    public BoxCollider triggerZone;
    public float textHeight = 2f;
    public float textDistance = 1f;
    public float textDisplayDuration = 5f;

    private float originalVoiceGain;
    private VRCPlayerApi localPlayer;
    private bool isInTrigger = false;
    private float textTimer;

    public GameObject sceneController;
    public bool isSceneController = false;

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        if (localPlayer != null)
        {
            originalVoiceGain = 15f;
        }

        if (followText != null)
        {
            followText.gameObject.SetActive(false);
        }

        if (sceneController != null)
        {
            sceneController.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == localPlayer)
        {
            isInTrigger = true;
            localPlayer.SetVoiceGain(voiceAmplification);
            if (followText != null)
            {
                followText.gameObject.SetActive(true);
                followText.text = "La tua voce è più forte, sei il presentatore!";
                textTimer = textDisplayDuration;
            }

            if(sceneController != null)
            {
                sceneController.gameObject.SetActive(true);
            }

        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == localPlayer)
        {
            isInTrigger = false;
            localPlayer.SetVoiceGain(originalVoiceGain);
            if (followText != null)
            {
                followText.gameObject.SetActive(false);
            }

            if(sceneController != null)
            {
                sceneController.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isInTrigger && followText != null && localPlayer != null)
        {
            if (textTimer > 0)
            {
                textTimer -= Time.deltaTime;
                if (textTimer <= 0)
                {
                    followText.gameObject.SetActive(false);
                }
            }

            if (followText.gameObject.activeSelf)
            {
                UpdateTextTransform();
            }
        }
    }

    private void UpdateTextTransform()
    {
        VRCPlayerApi.TrackingData headData = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
        Vector3 playerPosition = headData.position;
        Quaternion playerRotation = headData.rotation;

        // Calcola la posizione del testo
        Vector3 textPosition = playerPosition + (playerRotation * Vector3.forward * textDistance) + (Vector3.up * textHeight);
        followText.transform.position = textPosition;

        // Calcola la rotazione del testo
        Vector3 toPlayer = playerPosition - textPosition;
        toPlayer.y = 0; // Mantiene l'orientamento verticale
        Quaternion lookRotation = Quaternion.LookRotation(toPlayer);

        // Applica la rotazione inversa per far fronte al giocatore
        followText.transform.rotation = lookRotation * Quaternion.Euler(0, 180, 0);
    }
}