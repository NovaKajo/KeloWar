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
    private Health health;

    public Transform camera;

    PBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        healthBar = new PBar(this,foregroundImage);
        health.OnHealthPctChanged += HandlePctChanged;
    }

    private void HandlePctChanged(float currentHealthPct)
    {
        healthBar.HandleChange(currentHealthPct);
    }

    void LateUpdate()
    {
     
        transform.LookAt(transform.position + camera.position);
            
        
        
    }
}
