using TMPro;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    public TextMeshProUGUI tooltipText; // Ссылка на текстовый элемент
    public string message = "Нажмите ЛКМ, чтобы начать"; // Текст подсказки
    public Transform player; // Ссылка на игрока
    private int maxDistance = 4; // Максимальное расстояние для отображения подсказки
    bool entered;
    private void Start()
    {
        if (tooltipText != null)
        {
            tooltipText.text = ""; // Очищаем текст при старте
        }
        entered = false;
        maxDistance = 4;
    }

    private void Update()
    {
        // Проверяем расстояние между игроком и объектом
        float distance = Vector3.Distance(transform.position, player.position);
        // Если игрок находится на расстоянии, меньше или равном maxDistance
        if (distance + 0.5 <= maxDistance && entered)
        {
            if (tooltipText != null && !tooltipText.enabled)
            {
                tooltipText.text = message; // Устанавливаем текст подсказки
                tooltipText.enabled = true; // Делаем текст видимым
            }
        }
        else
        {
            if (tooltipText != null && tooltipText.enabled)
            {
                tooltipText.enabled = false; // Скрываем текст, если игрок слишком далеко
            }
        }
    }

    private void OnMouseEnter()
    {
        entered = true;
    }

    private void OnMouseExit()
    {
        entered = false;
    }
}
