using TMPro;
using UnityEngine;

public class AutomatManager : MonoBehaviour
{
    public SongManager songManager;
    public Transform capsuleTransform; // Ссылка на капсулу (объект игрока)
    public Transform cameraTransform; // Ссылка на камеру
    public Transform targetPosition; // Точка, куда переместить капсулу
    public Vector3 targetCameraRotation; // Желаемый угол камеры (в градусах)
    public float moveSpeed = 5f; // Скорость перемещения
    public KeyCode exitKey = KeyCode.Escape; // Клавиша для выхода из режима фиксации
    public TextMeshProUGUI tooltipText;

    private bool isFixedMode = false; // Находится ли капсула в фиксированном режиме
    private Vector3 originalPosition; // Исходная позиция капсулы
    private Quaternion originalCapsuleRotation; // Исходная ориентация капсулы
    private Quaternion originalCameraRotation; // Исходная ориентация камеры

    private void Start()
    {
        if (capsuleTransform == null)
        {
            capsuleTransform = GetComponent<Transform>(); // Используем объект, к которому привязан скрипт
        }
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Используем основную камеру по умолчанию
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
        // Сохраняем исходное положение и ориентацию капсулы и камеры
        originalPosition = capsuleTransform.position;
        originalCapsuleRotation = capsuleTransform.rotation;
        originalCameraRotation = cameraTransform.localRotation;

        // Перемещаем капсулу и меняем угол камеры
        StartCoroutine(MoveCapsuleAndCamera(targetPosition.position, targetCameraRotation));

        isFixedMode = true;
        capsuleTransform.GetComponent<PlayerController>().enabled = false;
        cameraTransform.GetComponent<CameraController>().enabled = false;
        FindObjectOfType<HighlightOnHover>().enabled = false;
        tooltipText.enabled = false;
    }

    private void ExitFixedMode()
    {
        // Возвращаем капсулу и камеру в исходное положение
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

            // Перемещаем капсулу
            capsuleTransform.position = Vector3.Lerp(startPos, targetPos, t);
            capsuleTransform.rotation = Quaternion.Slerp(startCapsuleRot, targetPosition.rotation, t);

            // Изменяем угол камеры
            cameraTransform.localRotation = Quaternion.Slerp(startCameraRot, targetCamQuaternion, t);

            yield return null;
        }

        capsuleTransform.position = targetPos;
        capsuleTransform.rotation = targetPosition.rotation;
        cameraTransform.localRotation = targetCamQuaternion;
    }
}
