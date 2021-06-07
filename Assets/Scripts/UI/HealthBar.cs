using System;
using System.Collections;
using System.Collections.Generic;
using Kelo.Stats;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image foregroundImage;
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    [Range(0, 1)]
    private float updateSpeedSeconds = 0.3f;

    [SerializeField]
    private Health health;
    public Transform camera;

    public bool isUIBar = false;

    PBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        if(!isUIBar){
        camera = Camera.main.transform;
        }
        healthBar = new PBar(this,foregroundImage,updateSpeedSeconds);
        health.OnHealthPctChanged += HandlePctChanged;
        if(health.GetCurrentHealth() != health.GetMaxHealth())
        {
            foregroundImage.enabled = true;
            backgroundImage.enabled = true;
        }
    }

    private void HandlePctChanged(float currentHealthPct)
    {
        healthBar.HandleChange(currentHealthPct);
        if(!foregroundImage.enabled && !backgroundImage.enabled)
        {
        foregroundImage.enabled = true;
        backgroundImage.enabled = true;

        }
    }

    void LateUpdate()
    {
        if(isUIBar)
        return;
        if(health.IsDead())
        {
            this.gameObject.SetActive(false);
            return;
        }
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
               camera.transform.rotation * Vector3.up);
        
    }
}
