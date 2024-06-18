
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LaserPointer : UdonSharpBehaviour
{

    public LineRenderer lineRenderer;
    public GameObject laserTarget;

    [UdonSynced, FieldChangeCallback(nameof(laserActivated))]
    public bool _laserActivated = false;

    public bool laserActivated{
        get{
            return _laserActivated;
        }
        set{
            _laserActivated = value;
            if(_laserActivated){
                lineRenderer.enabled = true;
                laserTarget.SetActive(true);
            }else{
                lineRenderer.enabled = false;
                laserTarget.SetActive(false);
            }
        }
    }

    void Start()
    {
        lineRenderer.enabled = false;
        laserTarget.SetActive(false);
    }

    public override void OnPickupUseDown()
    {
        laserActivated = true;
    }

    public override void OnPickupUseUp()
    {
        laserActivated = false;
    }

    public override void OnPickup()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        Networking.SetOwner(Networking.LocalPlayer, lineRenderer.gameObject);
        Networking.SetOwner(Networking.LocalPlayer, laserTarget.gameObject);
        base.OnPickup();
    }
}
