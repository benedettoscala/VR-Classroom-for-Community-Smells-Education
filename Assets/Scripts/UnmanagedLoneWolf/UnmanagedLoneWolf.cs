
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UnmanagedLoneWolf : UdonSharpBehaviour
{
    public CloudThoughts[] teamCloudThoughts;
    public CloudThoughts managerCloudThoughts;

    public CloudThoughts loneWolfCloudThoughts;

    public Animator[] teamMembersAnimator;

    public Animator managerAnimator;

    void Start()
    {
        int k = 0;
        foreach (Animator animator in teamMembersAnimator) {
            //se k è dispari, l'animazione è di tipo 1, altrimenti è di tipo 0
            animator.SetInteger("animVal", k % 2);
            k++;
        }
    }

    //synchronized variable
    [UdonSynced, FieldChangeCallback(nameof(synchronizedVariable))]
    int _synchronizedVariable = 0;

    
    int synchronizedVariable {
        get => _synchronizedVariable;
        set {
            int k = 0;
            _synchronizedVariable = value;
            switch (value) {
                case 0:
                    //team
                    k = 0;
                    foreach (Animator animator in teamMembersAnimator) {
                        //se k è dispari, l'animazione è di tipo 1, altrimenti è di tipo 0
                        animator.SetInteger("animVal", k % 2);
                        k++;
                    }
                    managerAnimator.SetInteger("animVal", k);

                    foreach (CloudThoughts cloud in teamCloudThoughts) {
                        cloud.NoThought();
                    }

                    managerCloudThoughts.NoThought();

                    break;
                case 1:
                    //SETTO GLI ANIMATORS//
                    k = 0;
                    foreach (Animator animator in teamMembersAnimator) {
                        //se k è dispari, l'animazione è di tipo 1, altrimenti è di tipo 0
                        animator.SetInteger("animVal", k % 2);
                        k++;
                    }
                    //SETTO GLI ANIMATORS//
                    
                    managerAnimator.SetInteger("animVal", 0);

                    //SETTO LE NUVOLETTE//
                    foreach (CloudThoughts cloud in teamCloudThoughts) {
                        cloud.HappyThought();
                    }
                    managerCloudThoughts.HappyThought();
                    //SETTO LE NUVOLETTE//

                    
                    
                    break;
                case 2:
                    //SETTO GLI ANIMATORS//
                     foreach (Animator animator in teamMembersAnimator) {
                        //se k è dispari, l'animazione è di tipo 1, altrimenti è di tipo 0
                        animator.SetInteger("animVal", k % 2);
                        k++;
                    }

                    managerAnimator.SetInteger("animVal", 1);
                    //SETTO GLI ANIMATORS//

                    //SETTO LE NUVOLETTE//
                    foreach (CloudThoughts cloud in teamCloudThoughts) {
                        cloud.HappyThought();
                    }

                    managerCloudThoughts.ThinkingThought();
                    //SETTO LE NUVOLETTE//
                    
                    break;
            }
        }
    }

    public void metodo1() {
        synchronizedVariable = 1;
    }

    public void metodo2() {
        synchronizedVariable = 2;
    }


}
