using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPulse : MonoBehaviour
{
    public TMP_Text text;
    float fadeSpeed = 2f;

    void Start()
    {
        StartCoroutine("FadeTextToZeroAlpha");
    }

    public IEnumerator FadeTextToFullAlpha()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / fadeSpeed));
            yield return null;
        }
        StartCoroutine("FadeTextToZeroAlpha");
    }

    public IEnumerator FadeTextToZeroAlpha()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / fadeSpeed));
            yield return null;
        }
        StartCoroutine("FadeTextToFullAlpha");
    }
}
