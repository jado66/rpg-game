using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FauxLoading : MonoBehaviour
{
    public Image fauxLoadingImage;
    public float fadeDuration = 0.1f;
    public float holdDuration = 0.1f;

    private void Start()
    {
        // Ensure the image is disabled at start
        if (fauxLoadingImage != null)
        {
            fauxLoadingImage.enabled = false;
        }
    }

    public void StartFade()
    {
        if (fauxLoadingImage != null)
        {
            StartCoroutine(FadeCoroutine());
        }
        else
        {
            Debug.LogError("Faux Loading Image not assigned in the QuickFade script!");
        }
    }

    private IEnumerator FadeCoroutine()
    {
        // Enable the image
        fauxLoadingImage.enabled = true;

        // Fade to black
        yield return StartCoroutine(FadeImage(0f, 1f));

        // Hold at black
        yield return new WaitForSeconds(holdDuration);

        // Fade back to transparent
        yield return StartCoroutine(FadeImage(1f, 0f));

        // Disable the image
        fauxLoadingImage.enabled = false;
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color originalColor = fauxLoadingImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fauxLoadingImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
            yield return null;
        }

        fauxLoadingImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, endAlpha);
    }
}