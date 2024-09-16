using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class VoiceTextTrigger : UdonSharpBehaviour
{
    public float voiceAmplification = 1000000f;
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

    public Canvas canvas;
    

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        if (localPlayer != null)
        {
            originalVoiceGain = 25f;
        }

        if (followText != null)
        {
            followText.gameObject.SetActive(false);
        }

        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player == localPlayer)
        {
            isInTrigger = true;
            DisableVoiceRolloff(player);
            if (followText != null)
            {
                followText.gameObject.SetActive(true);
                followText.text = "La tua voce si sentirà per tutta la stanza, sei il presentatore! (potrebbe non funzionare non l'ho testato)";
                textTimer = textDisplayDuration;
            }

            if (canvas != null)
            {
                canvas.enabled = true;
            }

        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player == localPlayer)
        {
            isInTrigger = false;
            EnableVoiceRolloff(player);
            if (followText != null)
            {
                followText.gameObject.SetActive(false);
            }

            if (canvas != null)
            {
                canvas.enabled = false;
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

    private void DisableVoiceRolloff(VRCPlayerApi player)
    {
        player.SetVoiceDistanceFar(1000000f);
        player.SetVoiceDistanceNear(0f);
        player.SetVoiceGain(15f);
        Debug.Log("Voice rolloff disabled for local player");
    }

    private void EnableVoiceRolloff(VRCPlayerApi player)
    {
        player.SetVoiceDistanceFar(25f);
        player.SetVoiceDistanceNear(0f);
        player.SetVoiceGain(15f);
        Debug.Log("Voice rolloff enabled for local player");
    }
}