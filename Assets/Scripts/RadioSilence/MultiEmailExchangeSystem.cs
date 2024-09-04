using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MultiEmailExchangeSystem : UdonSharpBehaviour
{
    public GameObject emailPrefab;
    public Transform[] senders;
    public Transform recipient;
    public float moveSpeed = 2f;
    public float lookSpeed = 5f;
    public int maxEmails = 30;
    public float maxHeight = 5f;
    public float timeBetweenEmails = 0.5f;

    private GameObject[] emailObjects;
    private int[] senderIndices;
    private bool[] movingToRecipient;
    private bool[] isActive;
    private float[] journeyTimes;
    private Vector3[] startPositions;
    private Vector3[] endPositions;
    private int activeEmailCount = 0;

    private float lastEmailTime;
    private bool isExchangeActive = false;

    void Start()
    {
        if (emailPrefab == null || senders == null || senders.Length == 0 || recipient == null)
        {
            Debug.LogError("Please set up the email prefab, at least one sender transform, and one recipient transform!");
            return;
        }

        InitializeArrays();
    }

    void InitializeArrays()
    {
        emailObjects = new GameObject[maxEmails];
        senderIndices = new int[maxEmails];
        movingToRecipient = new bool[maxEmails];
        isActive = new bool[maxEmails];
        journeyTimes = new float[maxEmails];
        startPositions = new Vector3[maxEmails];
        endPositions = new Vector3[maxEmails];
    }

    void Update()
    {
        if (isExchangeActive)
        {
            MoveEmails();
            SendNewEmails();
        }
    }

    public void StartEmailExchange()
    {
        if (isExchangeActive) return;
        isExchangeActive = true;
        lastEmailTime = Time.time;
    }

    public void StopEmailExchange()
    {
        isExchangeActive = false;
        ClearAllEmails();
    }

    private void SendNewEmails()
    {
        if (Time.time - lastEmailTime < timeBetweenEmails) return;

        int senderIndex = Random.Range(0, senders.Length);
        QueueEmail(senderIndex);
        lastEmailTime = Time.time;
    }

    private void QueueEmail(int senderIndex)
    {
        if (activeEmailCount >= maxEmails) return;

        int index = FindNextAvailableIndex();
        if (index == -1) return;

        emailObjects[index] = VRCInstantiate(emailPrefab);
        emailObjects[index].transform.SetParent(recipient, false);
        senderIndices[index] = senderIndex;
        movingToRecipient[index] = true;
        isActive[index] = true;
        journeyTimes[index] = 0f;
        startPositions[index] = recipient.InverseTransformPoint(senders[senderIndex].position) + Vector3.up;
        endPositions[index] = Vector3.up;
        emailObjects[index].transform.localPosition = startPositions[index];
        activeEmailCount++;
    }

    private int FindNextAvailableIndex()
    {
        for (int i = 0; i < maxEmails; i++)
        {
            if (emailObjects[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    private void MoveEmails()
    {
        for (int i = 0; i < maxEmails; i++)
        {
            if (emailObjects[i] == null || !isActive[i]) continue;

            journeyTimes[i] += Time.deltaTime * moveSpeed;
            float journeyFraction = journeyTimes[i] / Vector3.Distance(startPositions[i], endPositions[i]);

            if (journeyFraction >= 1f)
            {
                if (!movingToRecipient[i])
                {
                    ReturnEmailToPool(i);
                }
                else
                {
                    movingToRecipient[i] = false;
                    senderIndices[i] = Random.Range(0, senders.Length);
                    startPositions[i] = endPositions[i];
                    endPositions[i] = recipient.InverseTransformPoint(senders[senderIndices[i]].position) + Vector3.up;
                    journeyTimes[i] = 0f;
                }
            }
            else
            {
                Vector3 currentPosition = Vector3.Lerp(startPositions[i], endPositions[i], journeyFraction);
                float parabolicHeight = Mathf.Sin(journeyFraction * Mathf.PI) * maxHeight;
                currentPosition.y += parabolicHeight;
                emailObjects[i].transform.localPosition = currentPosition;

                LookAtLocalPlayer(emailObjects[i]);
            }
        }
    }

    private void ReturnEmailToPool(int index)
    {
        Destroy(emailObjects[index]);
        emailObjects[index] = null;
        isActive[index] = false;
        activeEmailCount--;
    }

    private void LookAtLocalPlayer(GameObject emailObj)
    {
        if (emailObj == null) return;

        VRCPlayerApi localPlayer = Networking.LocalPlayer;
        if (localPlayer != null)
        {
            Vector3 lookDirection = localPlayer.GetPosition() - recipient.TransformPoint(emailObj.transform.localPosition);
            lookDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            emailObj.transform.rotation = Quaternion.Slerp(emailObj.transform.rotation, targetRotation, lookSpeed * Time.deltaTime);
        }
    }

    private void ClearAllEmails()
    {
        for (int i = 0; i < maxEmails; i++)
        {
            if (emailObjects[i] != null)
            {
                Destroy(emailObjects[i]);
                emailObjects[i] = null;
            }
            isActive[i] = false;
        }
        activeEmailCount = 0;
    }
}