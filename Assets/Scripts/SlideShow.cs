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
    public Image mainSlideImage;

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

            mainSlideImage.sprite = slides[_currentSlide - 1];
            currentSlideImage.sprite = slides[_currentSlide - 1];
            if(_currentSlide == totalSlides){
                nextSlideImage.sprite = slides[0];
            } else {
                nextSlideImage.sprite = slides[_currentSlide];
            }
        }
    }

    int totalSlides;

    // Array of sprites to use as slides
    public Sprite[] slides;

    void Start()
    {
        // Apply the first slide
        mainSlideImage.sprite = slides[currentSlide - 1];
        nextSlideImage.sprite = slides[currentSlide];
        currentSlideImage.sprite = slides[currentSlide - 1];
        // Get the total number of slides
        totalSlides = slides.Length;
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