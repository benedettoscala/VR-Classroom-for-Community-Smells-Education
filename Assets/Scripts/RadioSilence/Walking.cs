using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Walking : UdonSharpBehaviour
{
    public GameObject[] walkingPoints;
    public GameObject[] lookingPoints;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;
    public float waitTime = 4f;

    [HideInInspector]
    public UdonSharpBehaviour eventReceiver;

    private int currentPointIndex = 0;
    private int targetPointIndex = -1;
    private float waitCounter = 0f;
    public bool isWalking = false;
    private bool isLooking = false;
    private bool isFinalLook = false;

    void Start()
    {
        if (walkingPoints.Length == 0)
        {
            Debug.LogError("No walking points assigned!");
        }
        if (lookingPoints.Length != walkingPoints.Length)
        {
            Debug.LogError("Number of looking points must match number of walking points!");
        }
    }

    void Update()
    {
        if (!isWalking && !isLooking && !isFinalLook || walkingPoints.Length == 0) return;

        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
            if (isLooking || isFinalLook)
            {
                LookAtPoint(isFinalLook ? lookingPoints[lookingPoints.Length - 1].transform.position : lookingPoints[currentPointIndex].transform.position);
            }
            return;
        }

        if (isLooking && !isFinalLook)
        {
            isLooking = false;
            isWalking = true;
            SendCustomEventDelayedSeconds("OnStartWalking", 0f); // Chiamata aggiunta qui
            return;
        }

        if (isFinalLook)
        {
            OnWaitComplete();
            return;
        }

        // Calculate direction to the current target
        Vector3 directionToTarget = walkingPoints[currentPointIndex].transform.position - transform.position;
        directionToTarget.y = 0; // Keep the rotation only in the horizontal plane

        // Rotate towards the target
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move towards the current point
        transform.position = Vector3.MoveTowards(transform.position, walkingPoints[currentPointIndex].transform.position, moveSpeed * Time.deltaTime);

        // Check if we've reached the current point
        if (Vector3.Distance(transform.position, walkingPoints[currentPointIndex].transform.position) < 0.01f)
        {
            // Check if we've reached the target point
            if (currentPointIndex == targetPointIndex)
            {
                StopWalking();
                return;
            }

            // Move to the next point
            currentPointIndex = (currentPointIndex + 1) % walkingPoints.Length;
            waitCounter = waitTime;
            isWalking = false;
            isLooking = true;
            SendCustomEventDelayedSeconds("OnWaiting", 0f);
        }
    }

    private void LookAtPoint(Vector3 point)
    {
        Vector3 directionToLook = point - transform.position;
        directionToLook.y = 0; // Keep the rotation only in the horizontal plane

        if (directionToLook != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void StartWalking(int targetIndex)
    {
        if (walkingPoints.Length == 0)
        {
            Debug.LogError("Cannot start walking: No walking points assigned!");
            return;
        }

        if (targetIndex < 0 || targetIndex >= walkingPoints.Length)
        {
            Debug.LogError($"Invalid target index: {targetIndex}. Must be between 0 and {walkingPoints.Length - 1}");
            return;
        }

        targetPointIndex = targetIndex;

        // Find the nearest walking point to start from
        float minDistance = float.MaxValue;
        int nearestPointIndex = 0;
        for (int i = 0; i < walkingPoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, walkingPoints[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPointIndex = i;
            }
        }

        currentPointIndex = nearestPointIndex;
        isWalking = true;
        isLooking = false;
        isFinalLook = false;
        SendCustomEventDelayedSeconds("OnStartWalking", 0f);
        Debug.Log($"Walking started towards point {targetPointIndex}!");
    }

    public void StopWalking()
    {
        isWalking = false;
        isLooking = false;
        isFinalLook = true;
        waitCounter = waitTime; // Set a wait time before calling OnWalkingComplete
        SendCustomEventDelayedSeconds("OnStopWalking", 0f);
        Debug.Log("Walking stopped, looking at final point!");
    }

    private void OnWaitComplete()
    {
        isFinalLook = false;
        Debug.Log("Final look complete!");
        SendCustomEventDelayedSeconds("OnWalkingComplete", 0f);
    }

    public void OnStartWalking()
    {
        if (eventReceiver != null)
        {
            eventReceiver.SendCustomEvent("OnStartWalking");
        }
    }

    public void OnStopWalking()
    {
        if (eventReceiver != null)
        {
            eventReceiver.SendCustomEvent("OnStopWalking");
        }
    }

    public void OnWaiting()
    {
        if (eventReceiver != null)
        {
            eventReceiver.SendCustomEvent("OnWaiting");
        }
    }

    public void OnWalkingComplete()
    {
        if (eventReceiver != null)
        {
            eventReceiver.SendCustomEvent("OnWalkingComplete");
        }
    }
}