
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


//TODO: SINCRONIZZARE LA CLASSE PER I VARI CLIENT(sarà una gran rottura)
public class BlackCloudEffect : UdonSharpBehaviour
{

    public Slider slider;
    bool action1Slider = false;
    bool action2Slider = false;
    bool action3Slider = false;
    public float velocity = 0.05f;
    public GameObject setting;

    public GameObject action2;

    public GameObject particleSystemCloud;

    public Animator papersAnimator;

    public Telefono telefono;

    void Start()
    {
        setting.SetActive(false);
        slider.value = 0; //set slider value to 0
        //deactivate slider
        slider.gameObject.SetActive(false);
        action2.SetActive(false);
        telefono.gameObject.SetActive(false);
    }

    void Update()
    {
        //change slider value gradually to 0.5, use time delta time to sync with frames
        if (action1Slider)
        {
            if (slider.value < 0.75)
            {
                slider.value += Time.deltaTime * velocity;
            }
            else
            {
                action1Slider = false;
            }
        }

        if (action2Slider)
        {
            if (slider.value < 0.80)
            {
                slider.value += Time.deltaTime * velocity;
            }
            else
            {
                action2Slider = false;
            }
        }

        if (action3Slider)
        {
            if (slider.value < 0.85)
            {
                slider.value += Time.deltaTime * velocity;
            }
            else
            {
                action3Slider = false;
            }
        }
        
        
    }

    public void activateSetting()
    {
        setting.SetActive(true);
        changeAlphaValueCloud(0f);
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
        changeAlphaValueCloud(0.1f);
        slider.gameObject.SetActive(true);
        action1Slider = true;  
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
        action2Slider = true;
        changeAlphaValueCloud(0.5f);
        slider.gameObject.SetActive(true);
        action2.SetActive(true);
    }

    public void activateAction3()
    {  
        /***
        Azione #3
        Iniziano a cadere e ad accumularsi una pila di documenti alle scrivanie dei team member.
        L’insegnante spiega che questo rappresenta l’accumularsi
        di informazioni che non viaggiano tra i membri del team.
        ***/

        action3Slider = true;
        changeAlphaValueCloud(0.6f);
        papersAnimator.SetInteger("papers", 1);
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

        changeAlphaValueCloud(1f);
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
        slider.gameObject.SetActive(true);
        telefono.gameObject.SetActive(true);
        telefono.activateSmartphoneAnimation();
        //change fill color of the slider to red
        slider.fillRect.GetComponent<Image>().color = Color.red;
    }
}
