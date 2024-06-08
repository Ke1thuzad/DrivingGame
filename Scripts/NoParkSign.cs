using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoParkSign : MonoBehaviour {
    public float waitTime = 5f;  // Время, после которого приходит предупреждение о неверной остановке (в сек.)

    float currentTime = 0f;
    VehicleControl vehicleControl;
    
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Vehicle")) return;
        
        // Проверка, что машиной управляет игрок
        VehicleControl temp = other.GetComponentInParent<VehicleControl>();
        if (!temp || !temp.GetComponentInChildren<DriveVehicle>())
            return;
        
        vehicleControl = temp;
    }

    void OnTriggerStay(Collider other) {
        // Проверка, что машиной управляет игрок
        if (!other.CompareTag("Vehicle") || !vehicleControl) return;

        // Проверка, что машина стоит на месте
        if (vehicleControl.GetSpeed() <= 0.5f)
            currentTime += Time.deltaTime;
        else currentTime = 0;

        // Если она стоит дольше, чем задано, то прислать уведомление.
        if (currentTime >= waitTime) {
            NotificationSystem.Manager.ShowNotification("You can't park here!");
        }
    }

    void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Vehicle")) return;
        
        VehicleControl temp = other.GetComponentInParent<VehicleControl>();
        if (!temp || !temp.GetComponentInChildren<DriveVehicle>())
            return;
        
        vehicleControl = null;
        currentTime = 0;
    }
}
