using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SmartphoneCanvas : UdonSharpBehaviour
{
    // Parent con all'interno una serie di raw images
    public GameObject notifications;

    bool showNotifications = false;

    public AudioSource notificationAudio;


    
    private float activationTimer = 0f;
    private float activationInterval = 2f; // Intervallo iniziale di 2 secondi
    private int activatedCount = 0;

    private RawImage[] rawImages;

    public RawImage attentionImage;
    private float pulseTimer = 0f;
    private float pulseInterval = 0.5f; // Intervallo di pulsazione di 0.5 secondi
    private bool isAttentionImageActive = false;

    void Start()
    {
        notificationAudio.playOnAwake = false;
        attentionImage.gameObject.SetActive(false);
        // Recupero le raw images
        rawImages = notifications.GetComponentsInChildren<RawImage>();
        // Le disattivo tutte 
        foreach (RawImage rawImage in rawImages)
        {
            rawImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Attivo ogni rawImage gradualmente
        if (showNotifications)
        {
            activationTimer += Time.deltaTime;
            
            if (activationTimer >= activationInterval && activatedCount < rawImages.Length)
            {
                if(activatedCount >= 4) 
                {
                    attentionImage.gameObject.SetActive(true);
                }
                // Attiva la prossima rawImage
                rawImages[activatedCount].gameObject.SetActive(true);
                activatedCount++;
                notificationAudio.Play(); // Suona la notifica

                // Riduci l'intervallo di attivazione per la prossima notifica
                activationInterval = Mathf.Max(0.1f, activationInterval * 0.9f); // Non scendere sotto 0.1 secondi
                activationTimer = 0f; // Resetta il timer
            }
        }

        

        // Gestione della pulsazione dell'attentionImage
        if (attentionImage.gameObject.activeSelf)
        {
            pulseTimer += Time.deltaTime;

            if (pulseTimer >= pulseInterval)
            {
                isAttentionImageActive = !isAttentionImageActive;
                attentionImage.gameObject.SetActive(isAttentionImageActive);
                pulseTimer = 0f; // Resetta il timer di pulsazione
            }
        }

        if(activatedCount == rawImages.Length)
        {
            attentionImage.gameObject.SetActive(true);
        }
    }

    public void activateSmartphoneAnimation()
    {
        showNotifications = true;
    }

    public void deactivateSmartphoneAnimation()
    {
        showNotifications = false;
        //dacitvate all raw images
        foreach (RawImage rawImage in rawImages)
        {
            rawImage.gameObject.SetActive(false);
        }

        //resets the activation timer
        activationTimer = 0f;
        activationInterval = 2f;
        activatedCount = 0;
        pulseInterval = 0.5f;
        pulseTimer = 0f;
        attentionImage.gameObject.SetActive(false);
    }
}
