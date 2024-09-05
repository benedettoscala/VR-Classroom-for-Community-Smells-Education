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
    public float smoothness = 5f; // Nuovo parametro per controllare la fluidità

    private GameObject[] mailObjects;
    private float[] angles;
    private float[] heights;
    private float[] bobOffsets;
    private Vector3[] velocities;
    private Vector3[] targetPositions;
    private Quaternion[] targetRotations;
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
        velocities = new Vector3[numberOfMails];
        targetPositions = new Vector3[numberOfMails];
        targetRotations = new Quaternion[numberOfMails];
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
                mailObjects[i].transform.localPosition = targetPositions[i];
                mailObjects[i].transform.localRotation = targetRotations[i];
                velocities[i] = Vector3.zero;
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

            // Usa SmoothDamp per un movimento più fluido
            mailObjects[i].transform.localPosition = Vector3.SmoothDamp(
                mailObjects[i].transform.localPosition, 
                targetPositions[i], 
                ref velocities[i], 
                1f / smoothness
            );

            // Interpolazione sferica per la rotazione
            mailObjects[i].transform.localRotation = Quaternion.Slerp(
                mailObjects[i].transform.localRotation, 
                targetRotations[i], 
                smoothness * Time.deltaTime
            );
        }
    }

    private void UpdateMailPosition(int index)
    {
        float x = Mathf.Cos(angles[index]) * spiralWidth * proximityToCenter;
        float z = Mathf.Sin(angles[index]) * spiralWidth * proximityToCenter;

        float bobY = Mathf.Sin(Time.time * bobSpeed + bobOffsets[index]) * bobHeight;

        Vector3 spiralPosition = new Vector3(x, heights[index] + bobY, z);
        targetPositions[index] = spiralPosition;

        targetRotations[index] = Quaternion.LookRotation(-spiralPosition.normalized);
    }

    // Metodi setter (rimangono invariati)
    public void SetSpiralSpeed(float speed) { spiralSpeed = speed; }
    public void SetSpiralWidth(float width) { spiralWidth = width; }
    public void SetSpiralHeight(float height) { spiralHeight = height; }
    public void SetVerticalSpeed(float speed) { verticalSpeed = speed; }
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
    public void SetBobSpeed(float speed) { bobSpeed = speed; }
    public void SetBobHeight(float height) { bobHeight = height; }
    public void SetProximityToCenter(float proximity)
    {
        proximityToCenter = Mathf.Max(0.1f, proximity);
    }

    // Nuovo metodo per regolare la fluidità
    public void SetSmoothness(float value)
    {
        smoothness = Mathf.Max(0.1f, value);
    }
}