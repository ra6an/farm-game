using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RGBSlider : MonoBehaviour
{
    [SerializeField] Slider rgbSlider;
    [SerializeField] Image colorPickedImage;
    [SerializeField] Color color;

    private float hueVal; 

    bool changeSliderValue = false;

    private void Start()
    {
        rgbSlider = this.gameObject.GetComponent<Slider>();
    }

    private void Update()
    {
        if (changeSliderValue)
        {
            RGBSliderValue(hueVal);
            changeSliderValue = false;
        }
    }

    public void RGBSlide()
    {
        var hue = rgbSlider.value;
        colorPickedImage.color = Color.HSVToRGB(hue, 1f, 1f);

        GameManager.instance.player.GetComponent<ItemContainerInteractController>().SetChestColor(colorPickedImage.color);
    }

    private void RGBSliderValue(float hue)
    {
        rgbSlider.value = hue;
    }

    public void SetHueValue(float hue)
    {
        changeSliderValue = true;
        hueVal = hue;
    }

    

}
