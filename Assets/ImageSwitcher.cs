using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{

    public Image image;
    public Sprite[] sprites;
    private float timer = 0f;
    private int currentImage = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer = 0f;
            SwitchImage();
        }
    }

    void SwitchImage()
    {
        image.sprite = sprites[currentImage];
        currentImage++;
        if (currentImage > sprites.Length - 1) currentImage = 0;
    }
}
