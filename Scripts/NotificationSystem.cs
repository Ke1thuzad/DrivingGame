using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NotificationSystem : MonoBehaviour {

    public static NotificationSystem Manager;
    
    public Image frame;  // Рамка уведомления.
    public TextMeshProUGUI innerText;  // Текст внутри уведомления.

    public float notificationLingerDuration = 5f;  // Сколько секунд будет показываться уведомление.
    public float notificationCooldown = 3f;  // Перерыв между уведомлениями.
    public float notificationTransitionSpeed = 0.5f;  // Скорость появления уведомления.

    Vector2 startingPosition;
    Vector2 endingPosition;

    bool isInProgress = false;
    
    // Задать видимость объектов и стартовые позиции для анимаций при запуске игры.
    void Awake() {
        Manager = this;
        endingPosition = frame.rectTransform.anchoredPosition;
        startingPosition = endingPosition;

        startingPosition.x = -startingPosition.x * 1.2f;
        frame.rectTransform.anchoredPosition = endingPosition;
        
        frame.enabled = false;
        innerText.enabled = false;
        innerText.text = "";
    }

    public void ShowNotification(string message) {
        if (isInProgress) return;
        isInProgress = true;
        innerText.text = message;
        innerText.enabled = true;
        frame.enabled = true;
        StartCoroutine(NotificationAnimations());
    }

    IEnumerator NotificationAnimations() {
        // Запуск анимаций на появление и исчезновение.
        yield return StartCoroutine(Transition(startingPosition, endingPosition));
        yield return new WaitForSeconds(notificationLingerDuration);
        yield return StartCoroutine(Transition(endingPosition, startingPosition));
        
        // Отключение видимости уведомления
        frame.enabled = false;
        innerText.enabled = false;
        
        // Ожидание перерыва между уведомлениями.
        yield return new WaitForSeconds(notificationCooldown);
        
        isInProgress = false;
    }

    // Плавный переход из позиции start в finish.
    IEnumerator Transition(Vector3 start, Vector3 finish) {
        float progress = 0;
        frame.rectTransform.anchoredPosition = start;
        
        while (progress < 1) {
            progress += Time.deltaTime * notificationTransitionSpeed;
            frame.rectTransform.anchoredPosition = Vector3.Lerp(start, finish, EaseOutCubic(progress));
            yield return null;
        }

        frame.rectTransform.anchoredPosition = finish;
    }

    float EaseOutCubic(float x) {
        return 1 - Mathf.Pow(1 - x, 3);
    }

}
