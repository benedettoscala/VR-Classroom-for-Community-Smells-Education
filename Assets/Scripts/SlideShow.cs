using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public class SlideShow : UdonSharpBehaviour
{   
    [UdonSynced, FieldChangeCallback(nameof(currentSlide))]
    private int _currentSlide = 1;
    public Image nextSlideImage;
    public Image currentSlideImage;
    public Image[] mainSlideImages;

    public int currentSlide
    {
        get => _currentSlide;
        set
        {
            _currentSlide = value;
            if (_currentSlide > totalSlides)
            {
                _currentSlide = 1;
            }
            else if (_currentSlide < 1)
            {
                _currentSlide = totalSlides;
            }

            UpdateSlides();
        }
    }

    int totalSlides;

    // Array of sprites to use as slides
    public Sprite[] slides;

    void Start()
    {
        // Get the total number of slides
        totalSlides = slides.Length;

        // Initial update of slides
        UpdateSlides();
    }

    void UpdateSlides()
    {
        // Update main slide images
        for (int i = 0; i < mainSlideImages.Length; i++)
        {
            mainSlideImages[i].sprite = slides[_currentSlide - 1];
        }

        // Update current and next slide previews
        currentSlideImage.sprite = slides[_currentSlide - 1];
        if (_currentSlide == totalSlides)
        {
            nextSlideImage.sprite = slides[0];
        }
        else
        {
            nextSlideImage.sprite = slides[_currentSlide];
        }
    }

    public void ChangeSlide()
    {   
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        currentSlide++;
        RequestSerialization();
    }

    public void NextSlide()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        currentSlide++;
        RequestSerialization();
    }

    public void PreviousSlide()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        currentSlide--;
        RequestSerialization();
    }
}