using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;

public class BlackCloudEffect : UdonSharpBehaviour
{
    public CloudThoughts managerThoughtsCloud;
    public TextMeshProUGUI text;
    public TimelineController slider;
    public float velocity = 0.05f;
    public GameObject setting;
    public GameObject blackCloud;
    public GameObject[] actions;
    public GameObject particleSystemCloud;
    //public Animator papersAnimator;
    public SmartphoneCanvas telefono;
    public SmartphoneCanvas[] computers;
    public Animator managerAnimator;
    public Animator[] teamMemberAnimators;
    public GameObject LSmartPhone;
    public GameObject RSmartPhone;
    public CloudThoughts[] teamMemberCloudThoughts;

    public AudioSource keyboardSound;

    public AppearDisappearBehaviour appearDisappearBehaviour;

    [UdonSynced, FieldChangeCallback(nameof(synchronizedVariable))]
    private int _synchronizedVariable = -1;

    public SpiralMovement[] spiralMovements;

    private int synchronizedVariable
    {
        get => _synchronizedVariable;
        set
        {
            _synchronizedVariable = value;
            HandleStateChange(value);
        }
    }

    void Start()
    {   
        keyboardSound.Stop();
        setting.SetActive(false);
        slider.SetSliderVisibility(false);
        foreach(GameObject action in actions)
        {
            action.SetActive(false);
        }
        telefono.gameObject.SetActive(false);
        computers = actions[6].GetComponentsInChildren<SmartphoneCanvas>();
        foreach(SmartphoneCanvas computer in computers)
        {
            computer.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        HandleSmartPhoneVisibility();
    }

    private void HandleStateChange(int state)
    {
        switch (state)
        {
            case 0:
                HandleSettingState();
                break;
            case 1:
                HandleAction1State();
                break;
            case 2:
                HandleAction2State();
                break;
            case 3:
                HandleAction3State();
                break;
            case 4:
                HandleAction4State();
                break;
            case 5:
                HandleAction5State();
                break;
            case 6:
                HandleAction6State();
                break;
            case 7:
                HandleAction7State();
                break;
        }
    }

    private void HandleSettingState()
    {
        appearDisappearBehaviour.Appear();
        managerThoughtsCloud.NoThought();
        foreach(var cloudThought in teamMemberCloudThoughts)
        {
            cloudThought.NoThought();
        }
        startSpirals(false);
        actions[0].SetActive(true);
        setting.SetActive(true);
        changeAlphaValueCloud(0f);
    }

    private void HandleAction1State()
    {
        ActivateSince(1);
        slider.SetSliderVisibility(true);
        slider.SetSliderTarget(0.5f);
        text.text = "Mancano 6 giorni alla scadenza della consegna";
        foreach(var cloudThought in teamMemberCloudThoughts)
        {
            cloudThought.HappyThought();
        }
        startSpirals(false);
        managerThoughtsCloud.HappyThought();
        managerAnimator.Play("Pickup", 0, 0);
        SetTeamMemberAnimations("Typing", 0);
        keyboardSound.Play();
        managerAnimator.SetInteger("phone", 1);
        managerAnimator.SetInteger("text", 0);
        changeAlphaValueCloud(0.1f);
        
    }

    private void HandleAction2State()
    {
        ActivateSince(2);
        text.text = "Mancano 5 giorni alla scadenza della consegna";
        managerAnimator.SetInteger("phone", 0);
        managerThoughtsCloud.ThinkingThought();
        foreach(var cloudThought in teamMemberCloudThoughts)
        {
            cloudThought.MuteThought();
        }

        changeAlphaValueCloud(0.3f);
        slider.SetSliderVisibility(true);
        slider.SetSliderTarget(0.75f);
        actions[3].SetActive(false);
        startSpirals(false);
        telefono.deactivateSmartphoneAnimation();
        telefono.gameObject.SetActive(false);
        SetTeamMemberAnimations("Typing", 0);
        managerAnimator.Play("Walking", 0, 0);
        managerAnimator.SetInteger("phone", 0);
        managerAnimator.SetInteger("text", 0);
    }

    private void startSpirals(bool start)
    {
        foreach (var spiral in spiralMovements)
        {
            if (start)
            {
                spiral.StartSpiral();
            }
            else
            {
                spiral.StopSpiral();
            }
        }
    }

    private void HandleAction3State()
    {
        ActivateSince(3);
        text.text = "Mancano 4 giorni alla scadenza della consegna";
        managerThoughtsCloud.ThinkingThought();
        SetTeamMemberAnimations("Confused", 1);

        startSpirals(true);

        //papersAnimator.SetInteger("papers", 1);
        changeAlphaValueCloud(0.6f);
        slider.SetSliderVisibility(true);
        slider.SetSliderTarget(0.85f);
    }

    private void HandleAction4State()
    {
        ActivateSince(4);
        text.text = "Mancano 3 giorni alla scadenza della consegna";
        managerAnimator.SetInteger("text", 1);
        changeAlphaValueCloud(0.7f);
        slider.SetSliderVisibility(true);
        slider.SetSliderTarget(0.9f);
        
        startSpirals(true);
        
        telefono.deactivateSmartphoneAnimation();
        telefono.gameObject.SetActive(false);
        foreach(var computer in computers)
        {
            computer.deactivateSmartphoneAnimation();
            computer.gameObject.SetActive(false);
        }
        managerAnimator.Play("Text", 0, 0);
        managerAnimator.SetInteger("phone", 0);
        managerAnimator.SetInteger("text", 1);
    }

    private void HandleAction5State()
    {
        ActivateSince(5);
        text.text = "Mancano 2 giorni alla scadenza della consegna";
        telefono.gameObject.SetActive(true);
        slider.SetSliderVisibility(true);
        managerThoughtsCloud.AngryThought();
        managerAnimator.SetInteger("text", 2);
        telefono.deactivateSmartphoneAnimation();
        telefono.activateSmartphoneAnimation();
        slider.SetSliderTarget(0.95f);
        changeAlphaValueCloud(0.7f);

        startSpirals(true);
    }

    private void HandleAction6State()
    {
        ActivateSince(6);
        text.text = "Manca 1 giorno alla scadenza della consegna";
        managerThoughtsCloud.ThinkingThought();
        managerAnimator.SetInteger("text", 1);
        
        startSpirals(true);

        changeAlphaValueCloud(0.7f);
        
        for(int i = 0; i < teamMemberCloudThoughts.Length; i++)
        {
            teamMemberCloudThoughts[i].AngryThought();
            teamMemberAnimators[i].Play(i % 2 == 0 ? "Rage1" : "Rage2", 0, 0);
            teamMemberAnimators[i].SetInteger("transistion", i % 2 == 0 ? 2 : 3);
            computers[i].gameObject.SetActive(true);
            computers[i].activateSmartphoneAnimation();
        }
        
        managerAnimator.Play("Text", 0, 0);
        managerAnimator.SetInteger("text", 1);
    }

    private void HandleAction7State()
    {
        ActivateSince(7);

        startSpirals(true);

        text.text = "Scaduto il tempo per la consegna";
        changeAlphaValueCloud(0.7f);
        managerThoughtsCloud.SadThought();
        foreach(var cloudThought in teamMemberCloudThoughts)
        {
            cloudThought.SadThought();
        }
        slider.SetSliderVisibility(true);
        slider.SetSliderTarget(1f);
        keyboardSound.Stop();
    }

    private void ActivateSince(int actionNumber)
    {
        for (int i = 1; i <= actionNumber; i++)
        {
            actions[i].SetActive(true);
        }
        for (int i = actionNumber + 1; i < actions.Length; i++)
        {
            actions[i].SetActive(false);
        }
    }

    private void SetTeamMemberAnimations(string animationName, int transitionValue)
    {
        foreach(var animator in teamMemberAnimators)
        {
            animator.Play(animationName, 0, 0);
            animator.SetInteger("transistion", transitionValue);
        }
    }

    private void HandleSmartPhoneVisibility()
    {
        LSmartPhone.SetActive(managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Pickup") ||
                              managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Phone"));
        
        RSmartPhone.SetActive(managerAnimator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Text"));
    }

    public void activateSetting()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        synchronizedVariable = 0;
        RequestSerialization();
    }

    void changeAlphaValueCloud(float alphaValue)
    {
        ParticleSystem ps = particleSystemCloud.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, alphaValue);
        ps.Clear();
        ps.Play();
    }

    public void metodo0() { Networking.SetOwner(Networking.LocalPlayer, gameObject); synchronizedVariable = 0; RequestSerialization(); }

    public void metodo1() { synchronizedVariable = 1; RequestSerialization(); }
    public void metodo2() { synchronizedVariable = 2; RequestSerialization(); }
    public void metodo3() { synchronizedVariable = 3; RequestSerialization(); }
    public void metodo4() { synchronizedVariable = 4; RequestSerialization(); }
    public void metodo5() { synchronizedVariable = 5; RequestSerialization(); }
    public void metodo6() { synchronizedVariable = 6; RequestSerialization(); }
    public void metodo7() { synchronizedVariable = 7; RequestSerialization(); }
}