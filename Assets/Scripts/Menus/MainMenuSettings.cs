



using System.Collections;
using UnityEngine;

public class MainMenuSettings : BaseMenuSettings
{
    private float fadeInTime = 0.5f;

    [SerializeField] private SpriteRenderer backgroundImage;


    protected override void Start()
    {
        base.Start();

        // Start the fade-in process
        StartCoroutine(FadeInMainMenu(backgroundImage, fadeInTime));
    }

    protected override void OnBackButtonClicked()
    {
        base.OnBackButtonClicked();
        GameManager.One.LoadMainMenuFromOtherMainMenu();
    }


    private IEnumerator FadeInMainMenu(SpriteRenderer backgroundImage, float duration)
    {
        Color tempColor = backgroundImage.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            tempColor.a = Mathf.Lerp(0f, 1f, t);
            backgroundImage.color = tempColor;
            yield return null;
        }

        tempColor.a = 1f;
        backgroundImage.color = tempColor;
    }
}