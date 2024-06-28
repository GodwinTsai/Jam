using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoderEffect : MonoBehaviour
{
    public float flickerSpeed = 2.0f; // 闪烁速度
    public float minAlpha = 0.2f; // 最小透明度
    public float maxAlpha = 1.0f; // 最大透明度

    private Image _image;
    private Color originalColor;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    void Start()
    {
        originalColor = _image.color;
    }

    private void Update()
    {
        if (_image == null)
        {
            return;
        }
        
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * flickerSpeed, 1));

        Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        // 设置新的颜色
        _image.color = newColor;
    }
}