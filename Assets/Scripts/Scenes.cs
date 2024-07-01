using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Scenes : UdonSharpBehaviour
{
    public Animator animFirstScene;
    public Animator animSecondScene;

    public GameObject firstScene;
    public GameObject secondScene;

    public GameObject smellsButton;
    public GameObject BCEAnimationButtons;
    public GameObject BCEDummy;
    public GameObject BackToSmellsButton;

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
            secondScene.SetActive(_animationActive);
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
            if (_changeSceneVariable)
            {
                ChangeSceneUtil();
            }
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

    public void ChangeScene()
    {
        ChangeSceneVariable = true;
    }

    private void ChangeSceneUtil()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        Debug.Log("Cambio scena...");
        SetSceneRigidbodies(firstScene, true);
        AnimationActive = true;
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
        ToggleUIElements(false, true, true, true);
        BCEDummyActive = true; // Attiva BCEDummy e sincronizza
    }

    public void backToSmellsButton()
    {
        Debug.Log("Back to Smells Button");
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        ToggleUIElements(true, false, false, false);
        BCEDummyActive = false; // Disattiva BCEDummy e sincronizza
    }

    private void ToggleUIElements(bool smells, bool bceButtons, bool bceDummy, bool backToSmells)
    {
        smellsButton.SetActive(smells);
        BCEAnimationButtons.SetActive(bceButtons);
        BCEDummy.SetActive(bceDummy);
        BackToSmellsButton.SetActive(backToSmells);
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
