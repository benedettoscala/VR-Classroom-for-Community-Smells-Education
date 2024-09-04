using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RadioSilence : UdonSharpBehaviour
{
    public CloudThoughts[] teamCloudThoughts;
    public CloudThoughts managerCloudThoughts;
    public CloudThoughts secondManagerCloudThoughts;

    public Animator managerAnimator;
    public Animator secondManagerAnimator;

    public Animator[] teamMembersAnimators;
    
    public CloudThoughts giuliaCloudThoughts;
    public Animator giuliaAnimator;

    public Walking walkingGiuliaScript;
    public Walking walkingSecondManagerScript;

    public MultiEmailExchangeSystem multiEmailExchangeSystem;

    public SpiralMovement spiralMovement;

    public TimelineController timelineController;

    public Transform giuliaStartPoint;

    public Transform secondManagerStartPoint;

    public Transform giuliaEndPoint;

    public Transform secondManagerEndPoint;

    public GameObject testoGiganteDiFineSmell;

    public AppearDisappearBehaviour appearDisappearBehaviour;

    void Start()
    {
        timelineController.SetSliderVisibility(true);

        // Assegna questo script come eventReceiver per gli script Walking
        if (walkingGiuliaScript != null)
        {
            walkingGiuliaScript.eventReceiver = this;
        }
        if (walkingSecondManagerScript != null)
        {
            walkingSecondManagerScript.eventReceiver = this;
        }

        testoGiganteDiFineSmell.gameObject.SetActive(false);
        appearDisappearBehaviour = GetComponent<AppearDisappearBehaviour>();
    }

    private int _synchronizedVariable = 0;

    private int synchronizedVariable
    {
        get => _synchronizedVariable;
        set
        {
            _synchronizedVariable = value;
            HandleStateChange(value);
        }
    }

    private void HandleStateChange(int state)
    {
        switch (state)
        {
            case 1:
                HandleStartState();
                break;
            case 2:
                PointToGiuliaState();
                break;
            case 3:
                ExchangeMailState();
                break;
            case 4:
                CommunicationCaosState();
                break;
            case 5:
                EndState();
                break;
        }
    }

    private void SetAllTeamAnimations(int value)
    {
        foreach (var animator in teamMembersAnimators)
        {
            animator.SetInteger("animVal", value);
        }
    }

    private void HandleStartState()
    {
        appearDisappearBehaviour.Appear();
        spiralMovement.StopSpiral();
        multiEmailExchangeSystem.StopEmailExchange();

        giuliaAnimator.transform.position = giuliaStartPoint.position;
        secondManagerAnimator.transform.position = secondManagerStartPoint.position;
        
        managerCloudThoughts.JollyCooperationThought();
        secondManagerCloudThoughts.JollyCooperationThought();
        giuliaCloudThoughts.StopCountNotification();
        secondManagerCloudThoughts.activateSendingEmail(false);

        managerAnimator.SetInteger("animVal", 0);
        secondManagerAnimator.SetInteger("animVal", -1);
        SetAllTeamAnimations(0);
        giuliaAnimator.SetInteger("animVal", 0);

        for (int i = 0; i < teamCloudThoughts.Length; i++)
        {
            teamCloudThoughts[i].HappyThought();
            teamMembersAnimators[i].SetInteger("animVal", -1);
            teamCloudThoughts[i].activateSendingEmail(false);
        }

        giuliaCloudThoughts.NoThought();

        timelineController.SetSliderVisibility(true);
        timelineController.SetSliderTarget(0.5f);
        timelineController.SetText("Mancano 7 giorni alla scadenza");
        testoGiganteDiFineSmell.gameObject.SetActive(false);
    }

    private void PointToGiuliaState()
    {
        spiralMovement.StopSpiral();
        multiEmailExchangeSystem.StopEmailExchange();
        giuliaCloudThoughts.StopCountNotification();
        
        walkingGiuliaScript.StartWalking(1);
        walkingSecondManagerScript.StartWalking(1);

        for (int i = 0; i < teamCloudThoughts.Length; i++)
        {
            teamCloudThoughts[i].HappyThought();
            teamMembersAnimators[i].SetInteger("animVal", -1);
            teamCloudThoughts[i].activateSendingEmail(false);
        }
        giuliaCloudThoughts.activateSendingEmail(false);
        secondManagerCloudThoughts.activateReceivedEmail(false);

        giuliaCloudThoughts.HappyThought();
        secondManagerCloudThoughts.HappyThought();
        managerCloudThoughts.HappyThought();

        giuliaAnimator.SetInteger("animVal", 1);
        secondManagerAnimator.SetInteger("animVal", 1);
        managerAnimator.SetInteger("animVal", 1);

        SetAllTeamAnimations(0);
        timelineController.SetSliderTarget(0.6f);
        timelineController.SetText("Mancano 4 giorni alla scadenza");
        testoGiganteDiFineSmell.gameObject.SetActive(false);
    }

    private void ExchangeMailState()
    {
        giuliaAnimator.transform.position = giuliaEndPoint.position;
        secondManagerAnimator.transform.position = secondManagerEndPoint.position;

        spiralMovement.StopSpiral();
        multiEmailExchangeSystem.StartEmailExchange();
        giuliaAnimator.SetInteger("animVal", 2);
        secondManagerAnimator.SetInteger("animVal", 0);
        managerAnimator.SetInteger("animVal", 4);
 
        SetAllTeamAnimations(0);
        giuliaCloudThoughts.StartCountNotification();
        giuliaCloudThoughts.HappyThought();
        for (int i = 0; i < teamCloudThoughts.Length; i++)
        {
            teamCloudThoughts[i].HappyThought();
            teamMembersAnimators[i].SetInteger("animVal", -1);
            teamCloudThoughts[i].activateSendingEmail(true);
        }
        secondManagerCloudThoughts.HappyThought();
        managerCloudThoughts.HappyThought();


        secondManagerCloudThoughts.activateSendingEmail(true);
        secondManagerCloudThoughts.HappyThought();

        
        timelineController.SetSliderTarget(0.7f);
        timelineController.SetText("Manca 1 giorno alla scadenza");
        testoGiganteDiFineSmell.gameObject.SetActive(false);
    }

    private void CommunicationCaosState()
    {
        giuliaAnimator.transform.position = giuliaEndPoint.position;
        secondManagerAnimator.transform.position = secondManagerEndPoint.position;

        multiEmailExchangeSystem.StartEmailExchange();
        spiralMovement.StartSpiral();
        giuliaCloudThoughts.StartCountNotification();
        //animators
        SetAllTeamAnimations(0);
        giuliaAnimator.SetInteger("animVal", 3);
        secondManagerAnimator.SetInteger("animVal", 2);
        managerAnimator.SetInteger("animVal", 2);

        //cloud thoughts
        
        giuliaCloudThoughts.AngryThought();
        secondManagerCloudThoughts.AngryThought();
        managerCloudThoughts.AngryThought();
        for (int i = 0; i < teamCloudThoughts.Length; i++)
        {
            teamCloudThoughts[i].AngryThought();
            teamMembersAnimators[i].SetInteger("animVal", i%2);
            teamCloudThoughts[i].activateSendingEmail(true);
        }
        
        secondManagerCloudThoughts.activateSendingEmail(true);
        giuliaCloudThoughts.activateReceivedEmail(true);

        timelineController.SetSliderTarget(1f);
        timelineController.SetText("Scaduto il termine");

        testoGiganteDiFineSmell.gameObject.SetActive(false);
    }

    public void EndState()
    {
        CommunicationCaosState();
        testoGiganteDiFineSmell.gameObject.SetActive(true);
    }

    public void metodo1()
    {
        synchronizedVariable = 1;
    }

    public void metodo2()
    {
        synchronizedVariable = 2;
    }

    public void metodo3()
    {
        synchronizedVariable = 3;
    }
    
    public void metodo4()
    {
        synchronizedVariable = 4;
    }

    public void metodo5()
    {
        synchronizedVariable = 5;
    }

    // Metodi per gestire gli eventi dello script Walking per Giulia
    public void OnStartWalking()
    {
        Debug.Log("Un personaggio ha iniziato a camminare");
        // Determinare quale personaggio ha iniziato a camminare
        if (walkingGiuliaScript.isWalking)
        {
            OnGiuliaWalking();
        }
        else if (walkingSecondManagerScript.isWalking)
        {
            OnSecondManagerWalking();
        }
    }

    public void OnStopWalking()
    {
            Debug.Log("Un personaggio si è fermato");
            if (!walkingGiuliaScript.isWalking)
            {
                OnGiuliaWaiting();
            }
            else if (!walkingSecondManagerScript.isWalking)
            {
                OnSecondManagerWaiting();
            }
            // La logica per determinare quale personaggio si è fermato potrebbe essere più complessa
            // e potrebbe richiedere variabili aggiuntive per tracciare lo stato di ciascun personaggio
    }

    public void OnWaiting()
    {
        Debug.Log("Un personaggio sta aspettando a un punto di camminata");
        // Determinare quale personaggio sta aspettando
        if (!walkingGiuliaScript.isWalking)
        {
            OnGiuliaWaiting();
        }
        else if (!walkingSecondManagerScript.isWalking)
        {
            OnSecondManagerWaiting();
        }
    }

    public void OnWalkingComplete()
    {
        Debug.Log("Un personaggio ha completato il percorso");
        Debug.Log(!walkingSecondManagerScript.isWalking);
        // Determinare quale personaggio ha completato il percorso
        if (!walkingGiuliaScript.isWalking && walkingGiuliaScript.waitCounter <= 0)
        {
            OnGiuliaWalkingComplete();
        }
        // questa cosa se ci fosse più gente non funzionerebbe
        OnSecondManagerWalkingComplete();
    }

    // Metodi specifici per Giulia
    private void OnGiuliaWalkingComplete()
    {
        giuliaAnimator.SetInteger("animVal", 2);
    }

    private void OnGiuliaWaiting()
    {
        giuliaAnimator.SetInteger("animVal", 0);
    }

    private void OnGiuliaWalking()
    {
        giuliaAnimator.SetInteger("animVal", 1);
    }

    // Metodi specifici per SecondManager
    private void OnSecondManagerWalkingComplete()
    {
        Debug.Log("SecondManager ha completato il percorso");
        secondManagerAnimator.SetInteger("animVal", 0);
    }

    private void OnSecondManagerWaiting()
    {
        secondManagerAnimator.SetInteger("animVal", 0);
    }

    private void OnSecondManagerWalking()
    {
        secondManagerAnimator.SetInteger("animVal", 1);
    }
}