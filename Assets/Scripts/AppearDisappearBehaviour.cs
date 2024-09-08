using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AppearDisappearBehaviour : UdonSharpBehaviour
{
    [SerializeField] private float appearDuration = 1f;
    [SerializeField] private float disappearDuration = 1f;
    [SerializeField] private float appearHeight = 5f;

    private Vector3 startPosition;
    private Vector3 hidePosition;
    private bool isAppearing = false;
    private bool isDisappearing = false;
    private float elapsedTime = 0f;

    void Start()
    {
        startPosition = transform.position;
        hidePosition = startPosition + Vector3.up * appearHeight;
        transform.position = hidePosition;
    }

    public void Appear()
    {
        if (!isAppearing && !isDisappearing)
        {
            isAppearing = true;
            elapsedTime = 0f;
        }
    }

    public void Disappear()
    {
        if (!isAppearing && !isDisappearing)
        {
            isDisappearing = true;
            elapsedTime = 0f;
        }
    }

    void Update()
    {
        if (isAppearing)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / appearDuration);
            transform.position = Vector3.Lerp(hidePosition, startPosition, t);

            if (t >= 1f)
            {
                isAppearing = false;
            }
        }
        else if (isDisappearing)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / disappearDuration);
            transform.position = Vector3.Lerp(startPosition, hidePosition, t);

            if (t >= 1f)
            {
                isDisappearing = false;
            }
        }
    }
}