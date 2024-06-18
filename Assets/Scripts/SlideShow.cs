using System.Drawing;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using Image = UnityEngine.UI.Image;

public class SlideShow : UdonSharpBehaviour
{   

    [UdonSynced, FieldChangeCallback(nameof(currentSlide))]
    private int _currentSlide = 1;
    public Image nextSlideSlideController;
    public Image currentSlideSlideController;


    public int currentSlide
    {
        get => _currentSlide;
        set
        {
            _currentSlide = value;
            if (currentSlide > totalSlides)
            {
                _currentSlide = 1;
            }
            else if (currentSlide < 1)
            {
                _currentSlide = totalSlides;
            }

            GetComponent<Renderer>().material = materials[_currentSlide - 1];
            currentSlideSlideController.material = materials[_currentSlide - 1];
            if(_currentSlide == totalSlides){
                nextSlideSlideController.material = materials[0];
            } else {
                nextSlideSlideController.material = materials[_currentSlide];
            }
            
        }
    }

    int totalSlides;

    //array of materials to use as slides
    public Material[] materials;
    

    void Start()
    {
        //apply the first slide to the object
        GetComponent<Renderer>().material = materials[currentSlide-1];
        nextSlideSlideController.material = materials[currentSlide];
        currentSlideSlideController.material = materials[currentSlide-1];
        //get the total number of slides
        totalSlides = materials.Length;

    }


    //if the object receives the custom event change slide, it will change the slide
    public void ChangeSlide()
    {   
        currentSlide++;
        //send the current slide int to the network
        Networking.SetOwner(Networking.LocalPlayer, gameObject);

        RequestSerialization();

        //change the material of the object based on the current slide
        //use the current slide to get the material from the array
        //apply the material to the object
        Networking.SetOwner(Networking.LocalPlayer, gameObject);

        GetComponent<Renderer>().material = materials[currentSlide-1];
        
        
        
    }

    public void NextSlide()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);

        currentSlide++;
        //change the material of the object based on the current slide
        //use the current slide to get the material from the array
        //apply the material to the object
        //GetComponent<Renderer>().material = materials[currentSlide-1];
        RequestSerialization();
    }

    public void PreviousSlide()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        currentSlide--;

        //change the material of the object based on the current slide
        //use the current slide to get the material from the array
        //apply the material to the object
        //GetComponent<Renderer>().material = materials[currentSlide-1];
        RequestSerialization();

    }

}
