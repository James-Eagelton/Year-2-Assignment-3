using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ScrollingTexture : MonoBehaviour
{

     RawImage rawImage;


    private void Start()
    {
        rawImage = GetComponent<RawImage>();
    }



    // Offset values

    public float xFrequency;
    public float yFrequency;
    public float xAmplitude;
    public float yAmplitude;
    
    Vector2 offset = new Vector2(0.1f, 0.1f);

    // Update the texture offset every frame or whenever you need it
    void Update()
    {
        offset.x = Mathf.Sin(Time.time * xFrequency) * xAmplitude;
        offset.y = Mathf.Sin(Time.time * yFrequency) * yAmplitude;
        
        
        if (rawImage != null && rawImage.texture != null)
        {
            // Apply the offset to the uvRect property
            rawImage.uvRect = new Rect(rawImage.uvRect.x + offset.x * Time.deltaTime,
                                       rawImage.uvRect.y + offset.y * Time.deltaTime,
                                       rawImage.uvRect.width,
                                       rawImage.uvRect.height);
        }
    }
}
