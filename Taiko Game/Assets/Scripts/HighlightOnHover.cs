using TMPro;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    public TextMeshProUGUI tooltipText; // Ссылка на текстовый элемент
    public string message = "Нажмите ЛКМ, чтобы начать"; // Текст подсказки

    private void Start()
    {
        if (tooltipText != null)
        {
            tooltipText.text = ""; // Очищаем текст при старте
        }
    }

    private void OnMouseEnter()
    {
        if (tooltipText != null)
        {
            tooltipText.text = message; // Устанавливаем текст
            tooltipText.enabled = true; // Делаем текст видимым
        }
    }

    private void OnMouseExit()
    {
        if (tooltipText != null)
        {
            tooltipText.enabled = false; // Скрываем текст
        }
    }
}
