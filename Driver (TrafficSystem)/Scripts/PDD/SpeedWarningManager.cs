using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedWarningManager : MonoBehaviour
{
    public Text warningText;
    private Coroutine warningCoroutine;

    void Start()
    {
        // Начально скрываем текст предупреждения
        warningText.text = "";
    }

    public void ShowWarning(string message)
    {
        // Если уже есть запущенная корутина, останавливаем ее
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
        }
        // Запускаем корутину для показа и скрытия сообщения
        warningCoroutine = StartCoroutine(ShowWarningCoroutine(message));
    }

    private IEnumerator ShowWarningCoroutine(string message)
    {
        warningText.text = message;
        yield return new WaitForSeconds(2f); // Показываем сообщение в течение 2 секунд
        warningText.text = "";
    }
}