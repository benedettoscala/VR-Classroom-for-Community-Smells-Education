
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;


//TODO: SINCRONIZZARE LA CLASSE PER I VARI CLIENT(sarà una gran rottura)
public class BlackCloudEffect : UdonSharpBehaviour
{
    public CloudThoughts managerThoughtsCloud;
    public TextMeshProUGUI text;
    public Slider slider;
    public float velocity = 0.05f;
    public GameObject setting;

    public GameObject blackCloud;
    public GameObject[] actions;

    public GameObject particleSystemCloud;

    public Animator papersAnimator;

    public SmartphoneCanvas telefono;

    public SmartphoneCanvas[] computers;

    public Animator managerAnimator;
    public Animator[] teamMemberAnimators;

    public GameObject LSmartPhone;
    public GameObject RSmartPhone;

    public CloudThoughts[] teamMemberCloudThoughts;

    
    float valueSliderToReach = 0f;

    [UdonSynced, FieldChangeCallback(nameof(activateSettingVal))]
    bool _activateSettingVal = false;

    bool activateSettingVal
    {
        get => setting.activeSelf;
        set
        {   
            managerThoughtsCloud.NoThought();

            for(int i = 0; i < teamMemberCloudThoughts.Length; i++)
            {
                teamMemberCloudThoughts[i].NoThought();
            }

            _activateSettingVal = value;
            actions[0].SetActive(_activateSettingVal);
            setting.SetActive(_activateSettingVal);
            if(activateSettingVal)
            {
                changeAlphaValueCloud(0f);
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action1Active))]
    bool _action1Active = false;

    bool action1Active
    {
        get => _action1Active;
        set
        {
            activateSince(1);
            slider.gameObject.SetActive(true);
            //change text to "Scadenza della consegna"
            text.text= "Mancano 6 giorni alla scadenza della consegna";
            //team members are thinking
            for (int i = 0; i < teamMemberCloudThoughts.Length; i++)
            {
                teamMemberCloudThoughts[i].HappyThought();
            }

            managerThoughtsCloud.HappyThought();
            //cambio animazione del manager
            Debug.Log(_action1Active);
            managerAnimator.Play("Pickup", 0, 0);
            for (int i = 0; i < teamMemberAnimators.Length; i++)
            {
                teamMemberAnimators[i].Play("Typing", 0, 0);
                teamMemberAnimators[i].SetInteger("transistion", 0);
            }

            managerAnimator.SetInteger("phone", 1);
            changeAlphaValueCloud(0.1f);
            valueSliderToReach = 0.75f;

            
            _action2Active = false;
            _action3Active = false;
            _action4Active = false;
            _action5Active = false;
            _action6Active = false;
            _action7Active = false;
            _action1Active = value;
            
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action2Active))]
    bool _action2Active = false;

    bool action2Active
    {
        get => _action2Active;
        set
        {
            
            activateSince(2);
            text.text = "Mancano 5 giorni alla scadenza della consegna";
            slider.gameObject.SetActive(value);

            
            

            managerAnimator.SetInteger("phone", 0);

            managerThoughtsCloud.ThinkingThought();
            for (int i = 0; i < teamMemberCloudThoughts.Length; i++)
            {
                teamMemberCloudThoughts[i].MuteThought();
            }
            changeAlphaValueCloud(0.3f);
            valueSliderToReach = 0.85f;
            actions[3].SetActive(false);

            Debug.Log(_action1Active);
            //se l'animazione è già stata avviata una volta
            if(_action2Active) {
                
                telefono.deactivateSmartphoneAnimation();
                telefono.gameObject.SetActive(false);
                for (int i = 0; i < teamMemberAnimators.Length; i++)
                {
                    teamMemberCloudThoughts[i].HappyThought();
                    computers[i].deactivateSmartphoneAnimation();
                    computers[i].gameObject.SetActive(false);
                    teamMemberAnimators[i].Play("Typing", 0, 0);
                    teamMemberAnimators[i].SetInteger("transistion", 0);
                }
                managerAnimator.Play("Walking", 0, 0);
                managerAnimator.SetInteger("phone", 0);
                managerAnimator.SetInteger("text", 0);
            }


            SetActionStatesSince(2);
            _action2Active = value;
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action3Active))]
    bool _action3Active = false;

    bool action3Active
    {
        get => _action3Active;
        set
        {
            
            activateSince(3);
            text.text = "Mancano 4 giorno alla scadenza della consegna";

            
            managerThoughtsCloud.ThinkingThought();
            for (int i = 0; i < teamMemberAnimators.Length; i++)
            {
                teamMemberCloudThoughts[i].DizzyThought();
                teamMemberAnimators[i].Play("Confused", 0, 0);
                teamMemberAnimators[i].SetInteger("transistion", 1);
            }
            papersAnimator.SetInteger("papers", value ? 1 : 0);
            changeAlphaValueCloud(0.6f);
            valueSliderToReach = 0.90f;
            
            if(_action3Active)
            {
                telefono.deactivateSmartphoneAnimation();
                telefono.gameObject.SetActive(false);
                for (int i = 0; i < teamMemberAnimators.Length; i++)
                {
                    computers[i].deactivateSmartphoneAnimation();
                    computers[i].gameObject.SetActive(false);
                }
                managerAnimator.Play("Walking", 0, 0);
                managerAnimator.SetInteger("phone", 0);
                managerAnimator.SetInteger("text", 0);
            }


            SetActionStatesSince(3);
            _action3Active = value;
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action4Active))]
    bool _action4Active = false;

    bool action4Active
    {
        get => _action4Active;
        set
        {
            
            text.text = "Mancano 3 giorni alla scadenza della consegna";
            activateSince(4);


            managerAnimator.SetInteger("text", 1);
            //force the current animation to stop
            changeAlphaValueCloud(0.7f);
            valueSliderToReach = 0.95f;
            
            //se l'animazione è già stata avviata
            if(_action4Active)
            {  
                telefono.deactivateSmartphoneAnimation();
                telefono.gameObject.SetActive(false);
                for (int i = 0; i < teamMemberAnimators.Length; i++)
                {
                    computers[i].deactivateSmartphoneAnimation();
                    computers[i].gameObject.SetActive(false);
                }
                managerAnimator.Play("Text", 0, 0);
                managerAnimator.SetInteger("phone", 0);
                managerAnimator.SetInteger("text", 1);
                
                
            }


            SetActionStatesSince(4);
            _action4Active = value;
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action5Active))]
    bool _action5Active = false;

    bool action5Active
    {
        get => _action5Active;
        set
        {
            
            activateSince(5);
            text.text = "Mancano 2 giorni alla scadenza della consegna";
            telefono.gameObject.SetActive(value);
            slider.gameObject.SetActive(value);

            managerThoughtsCloud.AngryThought();
            // resetto l'animazione
            managerAnimator.SetInteger("text", 2);
            telefono.activateSmartphoneAnimation();
            valueSliderToReach = 0.97f;

            changeAlphaValueCloud(0.7f);

            if(_action5Active)
            {
                telefono.deactivateSmartphoneAnimation();
                telefono.gameObject.SetActive(true);
                telefono.activateSmartphoneAnimation();
                
                for (int i = 0; i < teamMemberAnimators.Length; i++)
                {
                    teamMemberAnimators[i].Play("Typing", 0, 0);
                    teamMemberAnimators[i].SetInteger("transistion", 0);
                    computers[i].deactivateSmartphoneAnimation();
                    computers[i].gameObject.SetActive(false);
                }

                managerAnimator.Play("Text", 0, 0);
            }


            SetActionStatesSince(5);
            _action5Active = value;
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action6Active))]
    bool _action6Active = false;

    bool action6Active
    {
        get => _action6Active;
        set
        {
           
            text.text = "Mancano 1 giorno alla scadenza della consegna";
            activateSince(6);
            managerThoughtsCloud.ThinkingThought();
            managerAnimator.SetInteger("text", 1);
            int i = 0;

            changeAlphaValueCloud(0.7f);

            foreach(SmartphoneCanvas computer in computers)
            {
                if ( i % 2 == 0) {
                    teamMemberAnimators[i].SetInteger("transistion", 2);
                } else {
                    teamMemberAnimators[i].SetInteger("transistion", 3);
                }
                teamMemberCloudThoughts[i].AngryThought();
                i++;
                computer.gameObject.SetActive(true);
                computer.activateSmartphoneAnimation();

            } 

            if(_action6Active)
            {
                telefono.gameObject.SetActive(true);
                
                for(int j = 0; j < teamMemberCloudThoughts.Length; j++)
                {
                    //activate computers
                    computers[j].gameObject.SetActive(true);

                    if ( j % 2 == 0) {
                        teamMemberAnimators[j].Play("Rage1", 0, 0);
                        teamMemberAnimators[j].SetInteger("transistion", 2);
                    } else {
                        teamMemberAnimators[j].Play("Rage2", 0, 0);
                        teamMemberAnimators[j].SetInteger("transistion", 3);
                    }
                }
                managerAnimator.Play("Text", 0, 0);
                managerAnimator.SetInteger("text", 1); 
            }

            SetActionStatesSince(6);
            _action6Active = value;
        }
    }
    
    [UdonSynced, FieldChangeCallback(nameof(action7Active))]
    bool _action7Active = false;

    bool action7Active
    {
        get => _action7Active;
        set
        { 
            activateSince(7);
            //se lo slider è attivo
            text.text = "Scaduto il tempo per la consegna";  
            
            changeAlphaValueCloud(0.7f);

            managerThoughtsCloud.SadThought();
            for (int i = 0; i < teamMemberCloudThoughts.Length; i++)
            {
                teamMemberCloudThoughts[i].SadThought();
            }
            valueSliderToReach = 1f;

            if(_action7Active)
            {
                telefono.gameObject.SetActive(true);
                
                for(int i = 0; i < teamMemberCloudThoughts.Length; i++)
                {
                    //activate computers
                    computers[i].gameObject.SetActive(true);

                    if ( i % 2 == 0) {
                        teamMemberAnimators[i].Play("Rage1", 0, 0);
                        teamMemberAnimators[i].SetInteger("transistion", 2);
                    } else {
                        teamMemberAnimators[i].Play("Rage2", 0, 0);
                        teamMemberAnimators[i].SetInteger("transistion", 3);
                    }
                }

                managerAnimator.Play("Text", 0, 0);
                managerAnimator.SetInteger("text", 1); 
            }

            SetActionStatesSince(7); 

            _action7Active = value;  
        }
    }

    void Start()
    {   

        setting.SetActive(false);
        slider.value = 0; //set slider value to 0
        //deactivate slider
        slider.gameObject.SetActive(false);
        //prendi tutti i gameobject figli di blackcloud
        //activate all actions
        foreach(GameObject action in actions)
        {
            action.SetActive(false);
        }

        telefono.gameObject.SetActive(false);
        //in action 6 i children sono SmartPhoneCanvas
        computers = actions[6].GetComponentsInChildren<SmartphoneCanvas>();

        foreach(SmartphoneCanvas computer in computers)
        {
            computer.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //change slider value gradually to 0.5, use time delta time to sync with frames
        if(slider.value < valueSliderToReach) {
            slider.value += velocity * Time.deltaTime;
            //cambia anche il colore da verde a rosso
            slider.fillRect.GetComponent<Image>().color = Color.Lerp(Color.green, Color.red, slider.value);
        }

        if(slider.value >= valueSliderToReach) {
            slider.value = valueSliderToReach;
        }

        //se lo stato pickup con tag pickup is playing
        if(managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Pickup"))
        {
            //se è finito l'animazione
            LSmartPhone.SetActive(true);
        }

        if(managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Phone"))
        {
            //se è finito l'animazione
            LSmartPhone.SetActive(true);
        } else {
            LSmartPhone.SetActive(false);
        }
        
        //se lo stato Text con tag Text is playing
        if(managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Text"))
        {
            //se è finito l'animazione
            RSmartPhone.SetActive(true);
        } else {
            RSmartPhone.SetActive(false);
        }
    }

    public void activateSetting()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        activateSettingVal = true;
    }


    void changeAlphaValueCloud(float alphaValue)
    {
        ParticleSystem ps = particleSystemCloud.GetComponent<ParticleSystem>();
        //restart particle system
        var main = ps.main;
        //change only the alpha value of the color
        main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, alphaValue);
        ps.Clear();
        ps.Play();
    }

    public void activateAction1()
    {

        /***
            Azione #1
            Compare una timeline del progetto come elemento grafico sopra le scrivanie, che inizia ad avanzare.
            Non parte da zero, ma è già quasi a ¾ e recita “Scadenza della consegna”.
            L’insegnante spiega chi sono, cosa stanno facendo e qual è la situazione del progetto. 
        ***/
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        //faccio sto giochetto per aggiornare la variabile su tutti i client
        action1Active = !action1Active;
    }

    public void activateAction2()
    {
        /***
            Azione #2 ver.1 
            Compare un simbolo di muto sulle teste degli attori.
            Gli attori si mostrano ancora più frustrati nei loro movimenti alla scrivania.
            L’insegnante spiega la simbologia di ciò che sta accadendo al team.
            
            Azione #2 ver.2
            Compaiono dei muri tra ciascuna coppia di scrivanie,
            simboleggiando il loro isolamento e la loro incapacità di comunicare.
            Gli attori si mostrano ancora più frustrati nei loro movimenti alla scrivania.
            L’insegnante spiega la simbologia di ciò che sta accadendo al team.
        ***/

        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action2Active = true;
    }

    public void activateAction3()
    {  
        /***
        Azione #3
        Iniziano a cadere e ad accumularsi una pila di documenti alle scrivanie dei team member.
        L’insegnante spiega che questo rappresenta l’accumularsi
        di informazioni che non viaggiano tra i membri del team.
        ***/
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action3Active = true;
    }

    public void activateAction4()
    {
        /***
            Azione #4
            L’alone scuro è a questo punto ben visibile.
            Gli attori si mostrano in difficoltà all’interno della nuvola. 
            L’insegnante spiega che la nuvola indica la mancanza di conoscenza globale,
            e che riusciranno a comunicare solo con il loro manager. 
        ***/
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action4Active = true;
    }

    public void activateAction5()
    {
        
        /***
        Azione #5
        Il manager è visibilmente frustrato.
        Appare una finestra grafica all’interno della quale compaiono diverse scritte,
        rappresentanti i molti messaggi che il manager sta ricevendo da tutto il team.
        La barra di avanzamento è ora di colore rosso ed è quasi giunta alla fine. 
        ***/
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action5Active = true;
    }

    public void activateAction6()
    {
        /***
            Azione #6
            Una stessa finestra grafica appare sopra le scrivanie coperte dall’alone,
            all’interno della quale compaiono diverse scritte,
            rappresentanti i molti messaggi che il manager sta inviando al team oscurato.
            La barra giunge alla fine. 

        ***/
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action6Active = true;
    }

    public void activateAction7()
    {
        /***
            Azione #7
            Il manager è visibilmente frustrato e il team demoralizzato.
            L’insegnante spiega che il tempo a disposizione è finito
            e il progetto è fallito. Compare infine la scritta “Black Cloud”. 
        ***/
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        action7Active = true;
    }

    public void activateSince(int actionNumber) {
        for (int i = 1; i <= actionNumber; i++) {
            
            actions[i].SetActive(true);
        }
        for (int i = actionNumber + 1; i < actions.Length; i++) {
            actions[i].SetActive(false);
        }
    }
    

    public void SetActionStatesSince(int actionNumber)
    {

        // Array di stati per le azioni
        bool[] actionStates = new bool[7];
        //inizializza a false
        for (int i = 0; i < actionStates.Length; i++)
        {
            actionStates[i] = false;
        }


        for (int i = 0; i < actionNumber; i++)
        {
            actionStates[i] = true;
        }

        // Imposta gli stati delle azioni
        _action1Active = actionStates[0];
        _action2Active = actionStates[1];
        _action3Active = actionStates[2];
        _action4Active = actionStates[3];
        _action5Active = actionStates[4];
        _action6Active = actionStates[5];
        _action7Active = actionStates[6];

        //debug all the actions
        Debug.Log("Action 1: " + _action1Active);
        Debug.Log("Action 2: " + _action2Active);
        Debug.Log("Action 3: " + _action3Active);
        Debug.Log("Action 4: " + _action4Active);
        Debug.Log("Action 5: " + _action5Active);
        Debug.Log("Action 6: " + _action6Active);
        Debug.Log("Action 7: " + _action7Active);
    }
}
