using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TutorialFade : MonoBehaviour
{
    public TextMeshProUGUI textToFade;
    public float fadeDuration = 1f;
    public float fadeDelay = 1f;

    private void Start()
    {
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color originalColor = textToFade.color;

        while (true)
        {
            yield return new WaitForSeconds(fadeDelay);

            yield return FadeTextColor(textToFade, originalColor, new Color(originalColor.r, originalColor.g, originalColor.b, 0f), fadeDuration);

            yield return new WaitForSeconds(fadeDelay);

            yield return FadeTextColor(textToFade, new Color(originalColor.r, originalColor.g, originalColor.b, 0f), originalColor, fadeDuration);
        }
    }

    private IEnumerator FadeTextColor(TextMeshProUGUI text, Color startColor, Color targetColor, float duration)
    {
        float timer = 0f;
        float currentAlpha = startColor.a;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            Color newColor = Color.Lerp(startColor, targetColor, t);
            text.color = newColor;
            yield return null;
        }

        text.color = targetColor;
    }
}




