using TMPro;
using UnityEngine;

public class HighlightOnHover : MonoBehaviour
{
    public TextMeshProUGUI tooltipText; // ������ �� ��������� �������
    public string message = "������� ���, ����� ������"; // ����� ���������
    public Transform player; // ������ �� ������
    private int maxDistance = 4; // ������������ ���������� ��� ����������� ���������
    bool entered;
    private void Start()
    {
        if (tooltipText != null)
        {
            tooltipText.text = ""; // ������� ����� ��� ������
        }
        entered = false;
        maxDistance = 4;
    }

    private void Update()
    {
        // ��������� ���������� ����� ������� � ��������
        float distance = Vector3.Distance(transform.position, player.position);
        // ���� ����� ��������� �� ����������, ������ ��� ������ maxDistance
        if (distance + 0.5 <= maxDistance && entered)
        {
            if (tooltipText != null && !tooltipText.enabled)
            {
                tooltipText.text = message; // ������������� ����� ���������
                tooltipText.enabled = true; // ������ ����� �������
            }
        }
        else
        {
            if (tooltipText != null && tooltipText.enabled)
            {
                tooltipText.enabled = false; // �������� �����, ���� ����� ������� ������
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
