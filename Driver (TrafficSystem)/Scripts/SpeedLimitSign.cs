using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Добавляем пространство имен для работы с UI

public class SpeedLimitSign : MonoBehaviour
{
    public float speedLimit = 60f; // Ограничение скорости в км/ч
    private SpeedWarningManager speedWarningManager;
    public Text speedWarningText; // Добавляем переменную для текста предупреждения

    void Start()
    {
        // Найти компонент SpeedWarningManager в сцене
        speedWarningManager = FindObjectOfType<SpeedWarningManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.tag);
        if (other.CompareTag("Vehicle"))
        {
            Debug.Log("Car entered speed limit zone.");
            VehicleControl vehicleControl = other.GetComponentInParent<VehicleControl>();
            if (vehicleControl)
            {
                Debug.Log("DriveVehicle component found.");
                StartCoroutine(CheckSpeed(vehicleControl));
            }
            else
            {
                Debug.Log("DriveVehicle component not found.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited by: " + other.name);
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            // Очищаем текст предупреждения при выходе из зоны
            if (speedWarningText != null)
            {
                speedWarningText.text = "";
            }
        }
    }

    IEnumerator CheckSpeed(VehicleControl driveVehicle)
    {
        while (true)
        {
            float currentSpeed = driveVehicle.GetSpeed();
            Debug.Log("Current speed: " + currentSpeed + " km/h");
            if (currentSpeed > speedLimit)
            {
                Debug.Log("Warning: Speed limit exceeded!");
                // Обновляем текст в UI
                if (speedWarningText != null)
                {
                    speedWarningText.text = "Warning: Speed limit exceeded!";
                }
            }
            else
            {
                // Очищаем текст предупреждения, если скорость нормальная
                if (speedWarningText != null)
                {
                    speedWarningText.text = "";
                }
            }
            yield return new WaitForSeconds(1f); // Проверка скорости каждую секунду
        }
    }
}
