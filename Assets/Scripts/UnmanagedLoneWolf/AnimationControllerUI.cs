using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRC.SDK3.Data;
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class AnimationControllerUI : UdonSharpBehaviour
{
    [Header("Setup Instructions")]
    [Tooltip("1. Assign all UI elements below\n2. Assign the JSON TextAsset\n3. Set up button OnClick events in the inspector\n4. Ensure UdonSharp is installed")]
    public bool setupComplete = false;

    [Header("UI Elements")]
    [Tooltip("Assign the TextMeshProUGUI component for the animation title")]
    public TextMeshProUGUI AnimationTitle;
    [Tooltip("Assign the TextMeshProUGUI component for the animation description")]
    public TextMeshProUGUI AnimationDescription;

    [Header("Buttons")]
    [Tooltip("Assign the Button component for Play")]
    public Button PlayButton;
    [Tooltip("Assign the Button component for Forward. Then set OnClick() to call OnForwardButtonClick")]
    public Button ForwardButton;
    [Tooltip("Assign the Button component for Back. Then set OnClick() to call OnBackButtonClick")]
    public Button BackButton;
    
    [Header("Data Source")]
    [Tooltip("Assign the TextAsset containing your JSON data")]
    public TextAsset jsonTextAsset;

    private DataList animationDataList;
    [HideInInspector]
    public string[] AnimationTitles;
    [HideInInspector]
    public string[] AnimationDescriptions;
    public string[] AnimationEventsNames;
    public UdonBehaviour animationController;

    private int currentIndex = 0;

    void Start()
    {
        if (!setupComplete)
        {
            Debug.LogWarning("Setup is not complete. Please check the inspector and follow the setup instructions.");
        }

        if (jsonTextAsset != null)
        {
            string jsonString = jsonTextAsset.text;
            DataToken jsonData;

            if (VRCJson.TryDeserializeFromJson(jsonString, out jsonData))
            {
                if (jsonData.TokenType == TokenType.DataList)
                {
                    animationDataList = jsonData.DataList;
                    AnimationTitles = new string[animationDataList.Count];
                    AnimationDescriptions = new string[animationDataList.Count];
                    AnimationEventsNames = new string[animationDataList.Count];

                    for (int i = 0; i < animationDataList.Count; i++)
                    {
                        DataToken dataToken = animationDataList[i];

                        if (dataToken.TokenType == TokenType.DataDictionary)
                        {
                            DataDictionary animationData = dataToken.DataDictionary;
                            AnimationTitles[i] = animationData["title"].String;
                            AnimationDescriptions[i] = animationData["description"].String;
                            AnimationEventsNames[i] = animationData["event"].String;
                        }
                    }

                    Debug.Log("JSON loaded and parsed successfully.");
                    
                    UpdateUI();
                }
                else
                {
                    Debug.LogError("JSON root is not a list.");
                }
            }
            else
            {
                Debug.LogError("Failed to parse JSON data.");
            }
        }
        else
        {
            Debug.LogError("TextAsset is null. Please assign the JSON TextAsset.");
        }
    }

    public void UpdateUI()
    {
        if (AnimationTitles.Length > 0 && AnimationDescriptions.Length > 0)
        {
            AnimationTitle.text = AnimationTitles[currentIndex];
            AnimationDescription.text = AnimationDescriptions[currentIndex];
        }
    }

    public void OnPlayButtonClick()
    {
        String eventMethodName = AnimationEventsNames[currentIndex];
        //send a custom event to the UdonBehaviour that controls the animation
        //make a debug log to show the event name
        Debug.Log("Sending event: " + eventMethodName);
        //i am sending the event to...
        Debug.Log("Sending event to: " + animationController.name);
        animationController.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, eventMethodName);
    }

    public void OnNextButtonClick()
    {
        currentIndex = (currentIndex + 1) % AnimationTitles.Length;
        UpdateUI();
    }

    public void OnBackButtonClick()
    {
        currentIndex = (currentIndex - 1 + AnimationTitles.Length) % AnimationTitles.Length;
        UpdateUI();
    }
}