using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UnmanagedLoneWolf : UdonSharpBehaviour
{
    public CloudThoughts[] teamCloudThoughts;
    public CloudThoughts managerCloudThoughts;
    public Animator[] teamMembersAnimator;
    public Animator managerAnimator;
    public Animator loneWolfAnimator;
    public GameObject loneWolf;
    public CloudThoughts loneWolfCloud;
    
    public GameObject bigSmellTitle;

    [UdonSynced, FieldChangeCallback(nameof(synchronizedVariable))]
    private int _synchronizedVariable = 0;

    private string[] teamMemberPhrases = new string[] {
        "Ha riscritto tutto il mio codice senza chiedermelo!",
        "Continua a insistere che il sito dovrebbe essere azzurro quando abbiamo scelto per il rosso!",
        "Perché continua a usare il PascalCase se abbiamo deciso di fare tutto in camelCase?!?",
    };

    private float timeBetweenPhrases = 3f;
    private int currentPhraseIndex = 0;

    void Start()
    {
        bigSmellTitle.SetActive(false);
        loneWolf.SetActive(false);
        ResetTeamAnimations();
    }

    private int synchronizedVariable
    {
        get => _synchronizedVariable;
        set
        {
            _synchronizedVariable = value;
            HandleStateChange(value);
        }
    }

    private void HandleStateChange(int state)
    {
        switch (state)
        {
            case 0:
                HandleNormalState();
                break;
            case 1:
                HandleHappyState();
                break;
            case 2:
                HandleLoneWolfState();
                break;
            case 3:
                HandleAngryState();
                break;
            case 4:
                HandleDontCareState();
                break;
            case 5:
                HandleEndState();
                break;
        }
    }

    // Costanti per i diversi stati emotivi
    private const int THOUGHT_NONE = 0;
    private const int THOUGHT_HAPPY = 1;
    private const int THOUGHT_ANGRY = 2;
    private const int THOUGHT_THINKING = 3;

    private const int QUESTION_MARK = 4;

    private void HandleEndState()
    {
        HandleDontCareState();
        bigSmellTitle.SetActive(true);
        //loneWolfCloud.setDontCareMeme(true);
    }
    private void HandleNormalState()
    {
        ResetTeamAnimations();
        managerAnimator.SetInteger("animVal", teamMembersAnimator.Length);
        SetTeamCloudThoughts(THOUGHT_NONE);
        loneWolf.SetActive(false);
        loneWolfCloud.NoThought();
        bigSmellTitle.SetActive(false);
        loneWolfCloud.setDontCareMeme(false);
    }

    private void HandleHappyState()
    {
        ResetTeamAnimations();
        managerAnimator.SetInteger("animVal", 0);
        SetTeamCloudThoughts(THOUGHT_HAPPY);
        managerCloudThoughts.HappyThought();
        loneWolf.SetActive(false);
        loneWolfCloud.NoThought();
        bigSmellTitle.SetActive(false);
        loneWolfCloud.setDontCareMeme(false);
    }

    private void HandleLoneWolfState()
    {
        ResetTeamAnimations();
        loneWolf.SetActive(true);
        managerAnimator.SetInteger("animVal", 1);
        SetTeamCloudThoughts(THOUGHT_HAPPY);
        managerCloudThoughts.QuestionThought();
        loneWolfCloud.HappyThought();
        loneWolfCloud.setDontCareMeme(false);
        bigSmellTitle.SetActive(false);
    }

    private void HandleAngryState()
    {
        managerCloudThoughts.AngryThought();
        loneWolf.SetActive(true);
        loneWolfCloud.NoThought();
        bigSmellTitle.SetActive(false);
        // Reset phrase index and start the coroutine to display phrases sequentially
        currentPhraseIndex = 0;

        loneWolfCloud.setDontCareMeme(false);
        SendCustomEventDelayedSeconds(nameof(DisplayNextPhrase), 0f);
    }

    public void HandleDontCareState()
    {
        SetTeamCloudThoughts(THOUGHT_ANGRY);
        managerCloudThoughts.AngryThought();
        managerAnimator.SetInteger("animVal", 1);
        SetAllTeamAnimations(2);

        bigSmellTitle.SetActive(false);
        loneWolf.SetActive(true);
        loneWolfCloud.setDontCareMeme(true);
    }

    private void SetTeamCloudThoughts(int thoughtType)
    {
        foreach (var cloud in teamCloudThoughts)
        {
            switch (thoughtType)
            {
                case THOUGHT_NONE:
                    cloud.NoThought();
                    break;
                case THOUGHT_HAPPY:
                    cloud.HappyThought();
                    break;
                case THOUGHT_ANGRY:
                    cloud.AngryThought();
                    break;
                case THOUGHT_THINKING:
                    cloud.ThinkingThought();
                    break;
                case QUESTION_MARK:
                    cloud.QuestionThought();
                    break;
                // Aggiungi altri casi per eventuali altri stati emotivi
                default:
                    cloud.NoThought();
                    break;
            }
            cloud.deactivateText();
        }
    }

    public void DisplayNextPhrase()
    {
        if (currentPhraseIndex < teamMemberPhrases.Length)
        {
            teamCloudThoughts[currentPhraseIndex].AngryThought();
            teamCloudThoughts[currentPhraseIndex].activateText();
            teamCloudThoughts[currentPhraseIndex].setText(teamMemberPhrases[currentPhraseIndex]);
            teamMembersAnimator[currentPhraseIndex].SetInteger("animVal", 2);
            
            currentPhraseIndex++;
            
            if (currentPhraseIndex < teamMemberPhrases.Length)
            {
                SendCustomEventDelayedSeconds(nameof(DisplayNextPhrase), timeBetweenPhrases);
            }
        }
    }

    private void ResetTeamAnimations()
    {
        for (int i = 0; i < teamMembersAnimator.Length; i++)
        {
            teamMembersAnimator[i].SetInteger("animVal", i % 2);
        }
    }

    private void SetAllTeamAnimations(int value)
    {
        foreach (var animator in teamMembersAnimator)
        {
            animator.SetInteger("animVal", value);
        }
    }

    private void ResetCloudThoughts()
    {
        foreach (var cloud in teamCloudThoughts)
        {
            cloud.NoThought();
        }
    }

    private void SetTeamCloudThoughts(bool isHappy)
    {
        foreach (var cloud in teamCloudThoughts)
        {
            if (isHappy)
            {
                cloud.HappyThought();
            }
            else
            {
                cloud.AngryThought();
            }
            cloud.deactivateText();
        }
    }

    public void metodo1() { synchronizedVariable = 1; }
    public void metodo2() { synchronizedVariable = 2; }
    public void metodo3() { synchronizedVariable = 3; }

    public void metodo4() { synchronizedVariable = 4; }

    public void metodo5() { synchronizedVariable = 5; }
}