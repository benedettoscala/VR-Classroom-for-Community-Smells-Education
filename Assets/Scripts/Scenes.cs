using System.Collections.Generic;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Scenes : UdonSharpBehaviour
{
    //TODO sta classe è palesemente una god class, bisognerebbe rifattorizzarla
    public Material[] materialsFirstScene;
    public Material[] materialsSecondScene;

    
    public GameObject[] changeSceneObject;

    public Animator animFirstScene;
    public Animator animSecondScene;

    public GameObject firstScene;
    public GameObject secondScene;

    public GameObject smellsButton;
    public GameObject BCEAnimationButtons;
    public GameObject BCEDummy;
    public GameObject BackToSmellsButton;

    public GameObject blackCloudEffectScene;


    public GameObject ULFAnimationButtons;
    public GameObject UnmanagedLoneWolfScene;
    
    public GameObject RSAnimationButtons;
    public GameObject RadioSilenceScene;

    public AppearDisappearBehaviour avatarPedestals;


    [UdonSynced, FieldChangeCallback(nameof(BCEAnimation))]
    private int _BCEAnimation = 0;

    private int BCEAnimation
    {
        get => BCEDummy.GetComponentInChildren<Animator>().GetInteger("BCEAnimationValue");
        set
        {
            _BCEAnimation = value;
            SetAnimatorParameters(BCEDummy, "BCEAnimationValue", _BCEAnimation);
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(AnimationActive))]
    private bool _animationActive = false;

    private bool AnimationActive
    {
        get => animFirstScene.GetBool("CambioScena");
        set
        {
            _animationActive = value;
            //secondScene.SetActive(_animationActive);
            SetSceneAnimationState(value);
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(ChangeSceneVariable))]
    private bool _changeSceneVariable = false;

    private bool ChangeSceneVariable
    {
        get => _changeSceneVariable;
        set
        {
            _changeSceneVariable = value;
            ChangeSceneUtil();
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(BCEDummyActive))]
    private bool _BCEDummyActive = false;

    private bool BCEDummyActive
    {
        get => _BCEDummyActive;
        set
        {
            _BCEDummyActive = value;
            BCEDummy.SetActive(_BCEDummyActive);
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(BCESceneActive))]
    private bool _BCESceneActive = false;

    private bool BCESceneActive
    {
        get => _BCESceneActive;
        set
        {
            _BCESceneActive = value;
            blackCloudEffectScene.SetActive(_BCESceneActive);
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(ULWSceneActive))]
    private bool _ULWSceneActive = false;

    private bool ULWSceneActive
    {
        get => _ULWSceneActive;
        set
        {
            _ULWSceneActive = value;
            UnmanagedLoneWolfScene.SetActive(_ULWSceneActive);
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(RSSceneActive))]
    private bool _RSSceneActive = false;

    private bool RSSceneActive
    {
        get => _RSSceneActive;
        set
        {
            _RSSceneActive = value;
            RadioSilenceScene.SetActive(_RSSceneActive);
        }
    }

    [UdonSynced, FieldChangeCallback(nameof(AvatarPedestalsActive))]
    
    private bool _avatarPedestalsActive = false;

    private bool AvatarPedestalsActive
    {
        get => _avatarPedestalsActive;
        set
        {
            _avatarPedestalsActive = value;
            if (_avatarPedestalsActive)
            {
                avatarPedestals.Appear();
            }
            else
            {
                avatarPedestals.Disappear();
            }
        }
    }


    public void ChangeScene()
    {
        //setto l'owner a questo oggetto per poter sincronizzare le variabili
        //ed anche perchè senza di lui non funza nulla
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        ChangeSceneVariable = !ChangeSceneVariable;
        Debug.Log("ChangeSceneVariable: " + ChangeSceneVariable);
    }

    public void ActivateAndDeactivateAvatarPedestals()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        AvatarPedestalsActive = !AvatarPedestalsActive;
        Debug.Log("AvatarPedestalsActive: " + AvatarPedestalsActive);
    }

    private void ChangeSceneUtil()
    {
        Debug.Log("Cambio scena...");
        SetSceneRigidbodies(firstScene, ChangeSceneVariable);
        SetSceneRigidbodies(secondScene, !ChangeSceneVariable);

        // Cambia i materiali della prima scena in quelli della seconda scena, in 
        // change scene object ci sono gli oggetti che cambiano materiale, gli oggetti sono ordinati
        // in modo che il primo oggetto abbia il primo materiale, il secondo il secondo e così via
        for (int i = 0; i < changeSceneObject.Length; i++)
        {
            if(ChangeSceneVariable)
                changeSceneObject[i].GetComponent<Renderer>().material = materialsSecondScene[i];
            else
                changeSceneObject[i].GetComponent<Renderer>().material = materialsFirstScene[i];
        }


        AnimationActive = !AnimationActive;
    }

    private void SetAnimatorParameters(GameObject obj, string parameter, int value)
    {
        Animator[] animators = obj.GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.SetInteger(parameter, value);
        }
    }

    private void SetSceneAnimationState(bool state)
    {
        animFirstScene.SetBool("CambioScena", state);
        animSecondScene.SetBool("CambioScena", state);
    }

    private void SetSceneRigidbodies(GameObject scene, bool isKinematic)
    {
        Rigidbody[] rigidbodies = scene.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = isKinematic;
        }

        BoxCollider[] colliders = scene.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider bc in colliders)
        {
            bc.enabled = !isKinematic;
        }
    }

    public void BCEButtonSelected()
    {
        Debug.Log("BCE Button Selected");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        ToggleUIElements(false, true, true, false, false);
        //BCEDummyActive = true; // Attiva BCEDummy e sincronizza
    }

    public void ULWButtonSelected()
    {  
        Debug.Log("ULF Button Selected");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        ToggleUIElements(false, false, true, true, false);
        //BCEDummyActive = false; // Disattiva BCEDummy e sincronizza
        
    }

    public void RSButtonSelected()
    {
        Debug.Log("RS Button Selected");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        ToggleUIElements(false, false, true, false, true);
        //BCEDummyActive = false; // Disattiva BCEDummy e sincronizza
    }

    public void backToSmellsButton()
    {
        Debug.Log("Back to Smells Button");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        ToggleUIElements(true, false, false, false, false);
        //BCEDummyActive = false; // Disattiva BCEDummy e sincronizza
    }

    private void ToggleUIElements(bool smells, bool bceButtons, bool backToSmells, bool ulfButtons, bool rsButtons)
    {
        smellsButton.SetActive(smells);
        BCEAnimationButtons.SetActive(bceButtons);
        BackToSmellsButton.SetActive(backToSmells);
        ULFAnimationButtons.SetActive(ulfButtons);
        RSAnimationButtons.SetActive(rsButtons);
        BCESceneActive = bceButtons;
        ULWSceneActive = ulfButtons;
        RSSceneActive = rsButtons;
    }

    public void ActivateAndDeactivateBCEAnimation()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        BCEAnimation = BCEAnimation == 0 ? 1 : 0;
        Debug.Log("BCE Animation toggled to " + BCEAnimation);
    }

    public void ActivateDeactivateRunningAnimationBCE()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        BCEAnimation = BCEAnimation == 2 ? 0 : 2;
        Debug.Log("Running BCE Animation toggled to " + BCEAnimation);
    }
}
