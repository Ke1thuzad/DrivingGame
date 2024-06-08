using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class svetofor : MonoBehaviour
{
    public enum LightColor { Red, Yellow, Green }

    public float redDuration = 5f; // Длительность красного света
    public float yellowDuration = 2f; // Длительность желтого света
    public float greenDuration = 5f; // Длительность зеленого света

    private LightColor currentColor; // Текущий цвет светофора
    private float timer = 0f; // Таймер для отслеживания времени

    void Start()
    {
        // Инициализация начального состояния светофора
        currentColor = LightColor.Red;
        SetLightColor();
    }

    void Update()
    {
        // Обновление таймера
        timer += Time.deltaTime;

        // Проверка текущего цвета и обновление состояния светофора
        switch (currentColor)
        {
            case LightColor.Red:
                if (timer >= redDuration)
                {
                    currentColor = LightColor.Green;
                    timer = 0f;
                    SetLightColor();
                }
                break;
            case LightColor.Yellow:
                if (timer >= yellowDuration)
                {
                    currentColor = LightColor.Red;
                    timer = 0f;
                    SetLightColor();
                }
                break;
            case LightColor.Green:
                if (timer >= greenDuration)
                {
                    currentColor = LightColor.Yellow;
                    timer = 0f;
                    SetLightColor();
                }
                break;
        }
    }

    // Установка цвета светофора
    void SetLightColor()
    {
        switch (currentColor)
        {
            case LightColor.Red:
                // Установка красного света
                break;
            case LightColor.Yellow:
                // Установка желтого света
                break;
            case LightColor.Green:
                // Установка зеленого света
                break;
        }
    }

    // Методы для получения текущего цвета светофора
    public bool isRed()
    {
        return currentColor == LightColor.Red;
    }

    public bool isYellow()
    {
        return currentColor == LightColor.Yellow;
    }

    public bool isGreen()
    {
        return currentColor == LightColor.Green;
    }
}