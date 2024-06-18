
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Pulsante : UdonSharpBehaviour
{
    //animator del pulsante
    public Animator anim;

    [UdonSynced, FieldChangeCallback(nameof(AnimationActive))]
    private bool _animationActive = false;

    
    private bool AnimationActive{
        get{
            _animationActive = anim.GetBool("ActivateAnimation");
            return anim.GetBool("ActivateAnimation");
        }
        set{
            _animationActive = value;
            anim.SetBool("ActivateAnimation", value);
        }
    
    }

    void Start()
    {
        
    }

    public override void Interact(){
        if(AnimationActive){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            InteractionText = "Start Animation";
            AnimationActive = false;
        }else{
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            InteractionText = "Stop Animation";
            AnimationActive = true;
        }
    }
}
