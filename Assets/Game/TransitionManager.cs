using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Image TransitionImage;

    private bool isDirty = true;

    public IEnumerator FadeToBlack(float duration, Action<int> callback)
    {
        this.isDirty = true;
        Color previousColor = this.TransitionImage.color;
        float time = 0.0f;
        do
        {
            time += Time.deltaTime;
            this.TransitionImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(previousColor.a, 1f, Mathf.SmoothStep(0.0f, 1.0f, time / duration)));
            yield return 0;
        } while (time < duration);
        this.isDirty = false;

        callback(1);
    }

    public IEnumerator FadeToTransparent(float duration, Action<int> callback)
    {
        this.isDirty = true;
        Color previousColor = this.TransitionImage.color;
        float time = 0.0f;
        do
        {
            time += Time.deltaTime;
            this.TransitionImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(previousColor.a, 0f, Mathf.SmoothStep(0.0f, 1.0f, time / duration)));
            yield return 0;
        } while (time < duration);
        this.isDirty = false;

        callback(1);
    }
}
