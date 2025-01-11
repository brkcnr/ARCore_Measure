using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro; // TextMeshPro için gerekli

public class ARMeasurement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject pointPrefab;
    public TMP_Text distanceText;

    private List<GameObject> points = new List<GameObject>();
    private float touchCooldown = 0.1f; // Dokunmalar arası minimum süre
    private float lastTouchTime;

    void Start()
    {

        Debug.Log("ARMeasurement script started.");
        
        // Null kontrolleri
        if (raycastManager == null)
        {
            Debug.LogError("ARRaycastManager atanmadı!");
        }
        if (pointPrefab == null)
        {
            Debug.LogError("PointPrefab atanmadı!");
        }
        if (distanceText == null)
        {
            Debug.LogError("DistanceText atanmadı!");
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && Time.time - lastTouchTime > touchCooldown)
        {
            lastTouchTime = Time.time;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                // Use TrackableType.All to capture all possible AR trackables
                if (raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.AllTypes))
                {
                    Debug.Log($"Raycast başarılı! Yüzeye dokunuldu: {hits[0].pose.position}");
                    Pose hitPose = hits[0].pose;

                    // Nokta oluştur
                    GameObject point = Instantiate(pointPrefab, hitPose.position, hitPose.rotation);
                    points.Add(point);

                    // Mesafe hesapla
                    if (points.Count >= 2)
                    {
                        CalculateDistance();
                    }

                    // Alan ve çevre hesapla
                    if (points.Count >= 3)
                    {
                        CalculatePerimeterAndArea();
                    }
                }
                else
                {
                    Debug.LogWarning("Raycast başarısız. Hiçbir yüzeye dokunulmadı.");
                }
            }
        }
    }

    private void CalculateDistance()
    {
        if (points.Count >= 2)
        {
            Vector3 start = points[points.Count - 2].transform.position;
            Vector3 end = points[points.Count - 1].transform.position;

            float distance = Vector3.Distance(start, end);

            // Mesafe bilgisini ekrana yaz
            if (distanceText != null)
            {
                distanceText.text = $"Mesafe: {distance:F2} metre";
            }

            // Konsola yazdır
            Debug.Log($"[Mesafe Hesaplama] İki nokta arası mesafe: {distance:F2} metre");
        }
    }

    private void CalculatePerimeterAndArea()
    {
        float perimeter = 0f;
        float area = 0f;

        // Çevre Hesaplama
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 current = points[i].transform.position;
            Vector3 next = points[(i + 1) % points.Count].transform.position; // Son noktadan ilk noktaya geçiş

            perimeter += Vector3.Distance(current, next);
        }

        // Alan Hesaplama (Shoelace Formülü - 2D Yaklaşımı)
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 current = points[i].transform.position;
            Vector3 next = points[(i + 1) % points.Count].transform.position;

            area += (current.x * next.z) - (next.x * current.z);
        }
        area = Mathf.Abs(area) * 0.5f;

        // Çevre ve alan bilgilerini ekrana yaz
        if (distanceText != null)
        {
            distanceText.text = $"Çevre: {perimeter:F2} m\nAlan: {area:F2} m²";
        }

        // Konsola yazdır
        Debug.Log($"[Çevre ve Alan Hesaplama] Çevre: {perimeter:F2} metre, Alan: {area:F2} metrekare");
    }

    public void ClearPoints()
    {
        foreach (GameObject point in points)
        {
            Destroy(point);
        }
        points.Clear();

        // Metni temizle
        if (distanceText != null)
        {
            distanceText.text = "";
        }

        Debug.Log("Tüm noktalar temizlendi.");
    }
}
