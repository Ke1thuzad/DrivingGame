using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour {
    public Image speedLimitSign; // Картинка знака на UI.
    
    TextMeshProUGUI speedLimitText;  // Текст на UI, который будет обновляться при въезде в зону знака.

    public float standardSpeedLimit = 60f;  // Стандартное ограничение скорости по всей местности.
    public float penaltyFreeLimit = 5f;  // Какое превышение является ненаказуемым (в России - 20км/ч).

    DriveVehicle playerVehicle;

    void Awake() {
        // Поиск текста в детях картинки знака.
        speedLimitText = speedLimitSign.GetComponentInChildren<TextMeshProUGUI>();
        speedLimitText.text = "";

        // При первом появлении сброс ограничения скорости до стандартной.
        ResetLimit();

        // Поиск игрока.
        playerVehicle = FindObjectOfType<DriveVehicle>();
    }

    void FixedUpdate() {
        SwitchSign(GameControl.driving);  // Установить видимость знака, если игрок едет на машине.
        CheckSpeed(playerVehicle.GetCurrentSpeed(), standardSpeedLimit);  // Проверка скорости игрока вне знака.
    }

    void SwitchSign(bool state) {
        speedLimitSign.enabled = state;
        speedLimitText.enabled = state;
    }

    // Обновление ограничения на визуальном UI тексте.
    public void UpdateSpeedLimit(float speedLimit) {
        speedLimitText.text = speedLimit.ToString(CultureInfo.CurrentCulture);
    }

    // Проверка на превышение скорости.
    public void CheckSpeed(float vehicleSpeed, float speedLimit) {
        if (vehicleSpeed > speedLimit + penaltyFreeLimit) {
            NotificationSystem.Manager.ShowNotification($"You are going above the speed limit ({speedLimit} km/h)!");
        }
    }

    public void ResetLimit() {
        UpdateSpeedLimit(standardSpeedLimit);
    }
}
