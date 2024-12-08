using TMPro;
using UnityEngine;

public class AutomatManager : MonoBehaviour
{
    public SongManager songManager;
    public Transform capsuleTransform; // ������ �� ������� (������ ������)
    public Transform cameraTransform; // ������ �� ������
    public Transform targetPosition; // �����, ���� ����������� �������
    public Vector3 targetCameraRotation; // �������� ���� ������ (� ��������)
    public float moveSpeed = 5f; // �������� �����������
    public KeyCode exitKey = KeyCode.Escape; // ������� ��� ������ �� ������ ��������
    public TextMeshProUGUI tooltipText;

    private bool isFixedMode = false; // ��������� �� ������� � ������������� ������
    private Vector3 originalPosition; // �������� ������� �������
    private Quaternion originalCapsuleRotation; // �������� ���������� �������
    private Quaternion originalCameraRotation; // �������� ���������� ������

    private void Start()
    {
        if (capsuleTransform == null)
        {
            capsuleTransform = GetComponent<Transform>(); // ���������� ������, � �������� �������� ������
        }
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // ���������� �������� ������ �� ���������
        }
    }

    private void Update()
    {
        if (isFixedMode && Input.GetKeyDown(exitKey))
        {
            ExitFixedMode();
        }
    }

    private void OnMouseDown()
    {
        if (!isFixedMode)
        {
            EnterFixedMode();
            songManager.Initialize();
        }
    }

    private void EnterFixedMode()
    {
        // ��������� �������� ��������� � ���������� ������� � ������
        originalPosition = capsuleTransform.position;
        originalCapsuleRotation = capsuleTransform.rotation;
        originalCameraRotation = cameraTransform.localRotation;

        // ���������� ������� � ������ ���� ������
        StartCoroutine(MoveCapsuleAndCamera(targetPosition.position, targetCameraRotation));

        isFixedMode = true;
        capsuleTransform.GetComponent<PlayerController>().enabled = false;
        cameraTransform.GetComponent<CameraController>().enabled = false;
        FindObjectOfType<HighlightOnHover>().enabled = false;
        tooltipText.enabled = false;
    }

    private void ExitFixedMode()
    {
        // ���������� ������� � ������ � �������� ���������
        StartCoroutine(MoveCapsuleAndCamera(originalPosition, originalCameraRotation.eulerAngles));
        isFixedMode = false;

        capsuleTransform.GetComponent<PlayerController>().enabled = true;
        cameraTransform.GetComponent<CameraController>().enabled = true;
        FindObjectOfType<HighlightOnHover>().enabled = true;
        tooltipText.enabled = true;
        songManager.Stop();
        ScoreManager.ResetScore();

    }

    private System.Collections.IEnumerator MoveCapsuleAndCamera(Vector3 targetPos, Vector3 targetCamRot)
    {
        float elapsed = 0f;
        float duration = Vector3.Distance(capsuleTransform.position, targetPos) / moveSpeed;

        Vector3 startPos = capsuleTransform.position;
        Quaternion startCapsuleRot = capsuleTransform.rotation;
        Quaternion startCameraRot = cameraTransform.localRotation;
        Quaternion targetCamQuaternion = Quaternion.Euler(targetCamRot);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // ���������� �������
            capsuleTransform.position = Vector3.Lerp(startPos, targetPos, t);
            capsuleTransform.rotation = Quaternion.Slerp(startCapsuleRot, targetPosition.rotation, t);

            // �������� ���� ������
            cameraTransform.localRotation = Quaternion.Slerp(startCameraRot, targetCamQuaternion, t);

            yield return null;
        }

        capsuleTransform.position = targetPos;
        capsuleTransform.rotation = targetPosition.rotation;
        cameraTransform.localRotation = targetCamQuaternion;
    }
}
