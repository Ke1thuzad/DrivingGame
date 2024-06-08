using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpeedLimitZone : MonoBehaviour {
    public float speedLimit = 60f;  // Максимальная скорость в этой зоне.

    SpeedManager manager;
    
    VehicleControl vehicleControl;

    void Awake() {
        manager = FindObjectOfType<SpeedManager>();
        if (!manager) {
            throw new NullReferenceException("Speed Manager is not created");
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Vehicle")) return;
        
        // Проверка, что машиной управляет игрок
        VehicleControl temp = other.GetComponentInParent<VehicleControl>();
        if (!temp || !temp.GetComponentInChildren<DriveVehicle>())
            return;

        vehicleControl = temp;
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Vehicle") && vehicleControl)
        {
            // Проверка на скорость и обновление лимита на UI
            manager.UpdateSpeedLimit(speedLimit);
            manager.CheckSpeed(vehicleControl.GetSpeed(), speedLimit);
        }
    }

    void OnTriggerExit(Collider other) {
        // Проверка, что машиной управляет игрок
        VehicleControl temp = other.GetComponentInParent<VehicleControl>();
        if (!temp || !temp.GetComponentInChildren<DriveVehicle>()) 
            return;
        
        vehicleControl = null;
        // При выходе из зоны ограничения скорости сброс до стандартного ограничения скорости
        manager.ResetLimit();
    }
}
