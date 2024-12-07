using TMPro;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    public TextMeshProUGUI tooltipText; // ������ �� ��������� �������
    public string message = "������� ���, ����� ������"; // ����� ���������

    private void Start()
    {
        if (tooltipText != null)
        {
            tooltipText.text = ""; // ������� ����� ��� ������
        }
    }

    private void OnMouseEnter()
    {
        if (tooltipText != null)
        {
            tooltipText.text = message; // ������������� �����
            tooltipText.enabled = true; // ������ ����� �������
        }
    }

    private void OnMouseExit()
    {
        if (tooltipText != null)
        {
            tooltipText.enabled = false; // �������� �����
        }
    }
}
