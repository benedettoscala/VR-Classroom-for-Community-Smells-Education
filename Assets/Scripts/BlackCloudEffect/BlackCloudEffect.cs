
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
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


    float valueSliderToReach = 0f;

    [UdonSynced, FieldChangeCallback(nameof(activateSettingVal))]
    bool _activateSettingVal = false;

    bool activateSettingVal
    {
        get => setting.activeSelf;
        set
        {   
            managerThoughtsCloud.NoThought();
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
            _action1Active = value;
            slider.gameObject.SetActive(_action1Active);
            //change text to "Scadenza della consegna"
            text.text= "Mancano 6 giorni alla scadenza della consegna";
            actions[1].SetActive(_action1Active);
            if(_action1Active)
            {
                managerThoughtsCloud.ThinkingThought();
                //cambio animazione del manager
                managerAnimator.SetInteger("phone", 1);
                changeAlphaValueCloud(0.1f);
                valueSliderToReach = 0.75f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action2Active))]
    bool _action2Active = false;

    bool action2Active
    {
        get => _action2Active;
        set
        {
            _action2Active = value;
            actions[2].SetActive(_action2Active);
            text.text = "Mancano 5 giorni alla scadenza della consegna";
            slider.gameObject.SetActive(_action2Active);

            managerAnimator.SetInteger("phone", 0);
            if(_action2Active)
            {
                managerThoughtsCloud.HappyThought();
                changeAlphaValueCloud(0.3f);
                valueSliderToReach = 0.85f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action3Active))]
    bool _action3Active = false;

    bool action3Active
    {
        get => _action3Active;
        set
        {
            _action3Active = value;
            actions[3].SetActive(_action3Active);
            text.text = "Mancano 4 giorno alla scadenza della consegna";

            
            if(_action3Active)
            {
                managerThoughtsCloud.ThinkingThought();
                for (int i = 0; i < teamMemberAnimators.Length; i++)
                {
                    teamMemberAnimators[i].Play("Confused", 0, 0);
                    teamMemberAnimators[i].SetInteger("transistion", 1);
                }
                papersAnimator.SetInteger("papers", _action3Active ? 1 : 0);
                changeAlphaValueCloud(0.6f);
                valueSliderToReach = 0.90f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action4Active))]
    bool _action4Active = false;

    bool action4Active
    {
        get => _action4Active;
        set
        {
            _action4Active = value;
            text.text = "Mancano 3 giorni alla scadenza della consegna";
            actions[4].SetActive(_action4Active);

            
            if(_action4Active)
            {  
                
                
                managerAnimator.SetInteger("text", 1);
                //force the current animation to stop
                changeAlphaValueCloud(0.7f);
                valueSliderToReach = 0.95f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action5Active))]
    bool _action5Active = false;

    bool action5Active
    {
        get => _action5Active;
        set
        {
            _action5Active = value;
            actions[5].SetActive(_action5Active);
            text.text = "Mancano 2 giorni alla scadenza della consegna";
            telefono.gameObject.SetActive(_action5Active);
            slider.gameObject.SetActive(_action5Active);
            if(_action5Active)
            {
                managerThoughtsCloud.AngryThought();
                // resetto l'animazione
                managerAnimator.Play("Text", 0, 0);
                managerAnimator.SetInteger("text", 2);
                telefono.activateSmartphoneAnimation();
                valueSliderToReach = 0.97f;
            } else {
                telefono.deactivateSmartphoneAnimation();
            }

        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action6Active))]
    bool _action6Active = false;

    bool action6Active
    {
        get => _action6Active;
        set
        {
            _action6Active = value;
            text.text = "Mancano 1 giorno alla scadenza della consegna";
            actions[6].SetActive(_action6Active);
            if(_action6Active)
            {
                managerThoughtsCloud.ThinkingThought();
                managerAnimator.SetInteger("text", 1);
                int i = 0;
                foreach(SmartphoneCanvas computer in computers)
                {
                    if ( i % 2 == 0) {
                        teamMemberAnimators[i].SetInteger("transistion", 2);
                    } else {
                        teamMemberAnimators[i].SetInteger("transistion", 3);
                    }
                    i++;
                    computer.gameObject.SetActive(true);
                    computer.activateSmartphoneAnimation();
                }
            } else {
                foreach(SmartphoneCanvas computer in computers)
                {
                    computer.gameObject.SetActive(false);
                    computer.deactivateSmartphoneAnimation();
                }
            
            }
        }
    }
    
    [UdonSynced, FieldChangeCallback(nameof(action7Active))]
    bool _action7Active = false;

    bool action7Active
    {
        get => _action7Active;
        set
        {
            
            _action7Active = value;
            actions[7].SetActive(_action7Active);
            //se lo slider è attivo
            text.text = "Scaduto il tempo per la consegna";  
    
            if(_action7Active)
            {
                managerThoughtsCloud.SadThought();
                valueSliderToReach = 1f;
            }
            
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

        //se lo stato Hangup con tag Hangdup is playing
        if(managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Hangup"))
        {
            //se è finito l'animazione
            LSmartPhone.SetActive(false);
        }
        
        //la thoughtsCloud è sempre rivolta verso il local player
        managerThoughtsCloud.transform.LookAt(Networking.LocalPlayer.GetPosition());
        
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
        action2Active = !action2Active;
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
        action3Active = !action3Active;
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
        action4Active = !action4Active;
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
        action5Active = !action5Active;
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
        action6Active = !action6Active;
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
        action7Active = !action7Active;
    }


    
}
