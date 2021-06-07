using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PBar
{
    private MonoBehaviour bar;
    private Image barforegroundImage;

    [SerializeField]
    private float updateSpeedSeconds = 0.3f;

    public PBar(MonoBehaviour bar, Image foregroundImage)
    {
        this.bar = bar;
        barforegroundImage = foregroundImage;
    }

    public void HandleChange(float pct)
    {
        
        bar.StartCoroutine(ChangeToPct(pct,barforegroundImage));
    }

    private IEnumerator ChangeToPct(float pct,Image foregroundImage)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;

        }
        foregroundImage.fillAmount = pct;
    }

}
