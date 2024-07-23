
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


//TODO: SINCRONIZZARE LA CLASSE PER I VARI CLIENT(sarà una gran rottura)
public class BlackCloudEffect : UdonSharpBehaviour
{

    public Slider slider;
    public float velocity = 0.05f;
    public GameObject setting;

    public GameObject blackCloud;
    public GameObject[] actions;

    public GameObject particleSystemCloud;

    public Animator papersAnimator;

    public SmartphoneCanvas telefono;

    public SmartphoneCanvas[] computers;

    float valueSliderToReach = 0f;

    [UdonSynced, FieldChangeCallback(nameof(activateSettingVal))]
    bool _activateSettingVal = false;

    bool activateSettingVal
    {
        get => setting.activeSelf;
        set
        {
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
        get => slider.gameObject.activeSelf;
        set
        {
            _action1Active = value;
            slider.gameObject.SetActive(_action1Active);
            actions[1].SetActive(_action1Active);
            if(_action1Active)
            {
                changeAlphaValueCloud(0.1f);
                valueSliderToReach = 0.75f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action2Active))]
    bool _action2Active = false;

    bool action2Active
    {
        get => actions[2].activeSelf;
        set
        {
            _action2Active = value;
            actions[2].SetActive(_action2Active);
            slider.gameObject.SetActive(_action2Active);
            if(_action2Active)
            {
                changeAlphaValueCloud(0.3f);
                valueSliderToReach = 0.85f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action3Active))]
    bool _action3Active = false;

    bool action3Active
    {
        get => papersAnimator.GetInteger("papers") == 1;
        set
        {
            _action3Active = value;
            actions[3].SetActive(_action3Active);
            if(_action3Active)
            {
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
        get => particleSystemCloud.GetComponent<ParticleSystem>().main.startColor.color.a == 1f;
        set
        {
            _action4Active = value;
            actions[4].SetActive(_action4Active);
            if(_action4Active)
            {
                changeAlphaValueCloud(1f);
                valueSliderToReach = 0.95f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action5Active))]
    bool _action5Active = false;

    bool action5Active
    {
        get => telefono.gameObject.activeSelf;
        set
        {
            _action5Active = value;
            actions[5].SetActive(_action5Active);
            telefono.gameObject.SetActive(_action5Active);
            slider.gameObject.SetActive(_action5Active);
            if(_action5Active)
            {
                telefono.activateSmartphoneAnimation();
                valueSliderToReach = 0.97f;
            }
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(action6Active))]
    bool _action6Active = false;

    bool action6Active
    {
        get => computers[0].gameObject.activeSelf;
        set
        {
            _action6Active = value;
            actions[6].SetActive(_action6Active);
            if(_action6Active)
            {
                foreach(SmartphoneCanvas computer in computers)
                {
                    computer.gameObject.SetActive(true);
                    computer.activateSmartphoneAnimation();
                }
            }
        }
    }
    
    [UdonSynced, FieldChangeCallback(nameof(action7Active))]
    bool _action7Active = false;

    bool action7Active
    {
        get => valueSliderToReach == 1f;
        set
        {
            _action7Active = value;
            if(_action7Active)
            {
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
        action1Active = true;
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
}
