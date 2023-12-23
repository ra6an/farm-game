using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RGBSlider : MonoBehaviour
{
    [SerializeField] Slider rgbSlider;
    [SerializeField] Image colorPickedImage;
    [SerializeField] Color color;

    private void Start()
    {
        rgbSlider = this.gameObject.GetComponent<Slider>();
    }

    public void RGBSlide()
    {
        var hue = rgbSlider.value;
        colorPickedImage.color = Color.HSVToRGB(hue, 1f, 1f);
    }
}
