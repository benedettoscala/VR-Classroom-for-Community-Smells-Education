using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpiralMovement : UdonSharpBehaviour
{
    public GameObject mailPrefab;
    public Transform centerObject;
    public int numberOfMails = 5;
    public float spiralSpeed = 2f;
    public float spiralWidth = 1f;
    public float spiralHeight = 5f;
    public float verticalSpeed = 0.5f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;
    public float proximityToCenter = 1f;

    private GameObject[] mailObjects;
    private float[] angles;
    private float[] heights;
    private float[] bobOffsets;
    private bool isSpiralActive;

    void Start()
    {
        if (mailPrefab == null || centerObject == null)
        {
            Debug.LogError("Please assign the mail prefab and center object!");
            return;
        }

        InitializeArrays();
    }

    void InitializeArrays()
    {
        mailObjects = new GameObject[numberOfMails];
        angles = new float[numberOfMails];
        heights = new float[numberOfMails];
        bobOffsets = new float[numberOfMails];
    }

    void Update()
    {
        if (isSpiralActive)
        {
            UpdateMailPositions();
        }
    }

    public void StartSpiral()
    {
        if (isSpiralActive || mailPrefab == null || centerObject == null) return;

        isSpiralActive = true;
        for (int i = 0; i < numberOfMails; i++)
        {
            angles[i] = (2f * Mathf.PI / numberOfMails) * i;
            heights[i] = (spiralHeight / numberOfMails) * i;
            bobOffsets[i] = Random.Range(0f, 2f * Mathf.PI);

            if (mailObjects[i] == null)
            {
                mailObjects[i] = VRCInstantiate(mailPrefab);
                mailObjects[i].transform.SetParent(centerObject, false);
            }

            if (mailObjects[i] != null)
            {
                mailObjects[i].SetActive(true);
                UpdateMailPosition(i);
            }
            else
            {
                Debug.LogError("Failed to instantiate mail object " + i);
            }
        }
    }

    public void StopSpiral()
    {
        isSpiralActive = false;
        for (int i = 0; i < numberOfMails; i++)
        {
            if (mailObjects[i] != null)
            {
                Destroy(mailObjects[i]);
                mailObjects[i] = null;
            }
        }
    }

    private void UpdateMailPositions()
    {
        for (int i = 0; i < numberOfMails; i++)
        {
            if (mailObjects[i] == null) continue;

            angles[i] += spiralSpeed * Time.deltaTime;
            if (angles[i] >= 2f * Mathf.PI)
            {
                angles[i] -= 2f * Mathf.PI;
            }

            heights[i] += verticalSpeed * Time.deltaTime;
            if (heights[i] > spiralHeight)
            {
                heights[i] = 0f;
            }

            UpdateMailPosition(i);
        }
    }

    private void UpdateMailPosition(int index)
    {
        float x = Mathf.Cos(angles[index]) * spiralWidth * proximityToCenter;
        float z = Mathf.Sin(angles[index]) * spiralWidth * proximityToCenter;

        float bobY = Mathf.Sin(Time.time * bobSpeed + bobOffsets[index]) * bobHeight;

        Vector3 spiralPosition = new Vector3(x, heights[index] + bobY, z);
        mailObjects[index].transform.localPosition = spiralPosition;

        mailObjects[index].transform.localRotation = Quaternion.LookRotation(-spiralPosition.normalized);
    }

    public void SetSpiralSpeed(float speed)
    {
        spiralSpeed = speed;
    }

    public void SetSpiralWidth(float width)
    {
        spiralWidth = width;
    }

    public void SetSpiralHeight(float height)
    {
        spiralHeight = height;
    }

    public void SetVerticalSpeed(float speed)
    {
        verticalSpeed = speed;
    }

    public void SetNumberOfMails(int number)
    {
        if (number > 0 && number != numberOfMails)
        {
            StopSpiral();
            numberOfMails = number;
            InitializeArrays();
            if (isSpiralActive)
            {
                StartSpiral();
            }
        }
    }

    public void SetBobSpeed(float speed)
    {
        bobSpeed = speed;
    }

    public void SetBobHeight(float height)
    {
        bobHeight = height;
    }

    public void SetProximityToCenter(float proximity)
    {
        proximityToCenter = Mathf.Max(0.1f, proximity); // Assicura che il valore non sia troppo vicino a zero
    }
}