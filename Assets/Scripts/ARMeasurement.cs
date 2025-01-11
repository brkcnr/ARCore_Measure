using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ARMeasurement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject pointPrefab;
    public TMP_Text distanceText;
    public GameObject linePrefab; // LineRenderer prefab'i

    private List<GameObject> points = new List<GameObject>();
    private List<GameObject> lines = new List<GameObject>(); // Çizgiler için liste
    private float touchCooldown = 0.1f; // Dokunmalar arası minimum süre
    private float lastTouchTime;

    public void Start()
    {
        Debug.Log("ARMeasurement script started.");
        
        // Null kontrolleri
        if (raycastManager == null) Debug.LogError("ARRaycastManager atanmadı!");
        if (pointPrefab == null) Debug.LogError("PointPrefab atanmadı!");
        if (distanceText == null) Debug.LogError("DistanceText atanmadı!");
        if (linePrefab == null) Debug.LogError("LinePrefab atanmadı!");
    }

    public void Update()
    {
        if (Input.touchCount > 0 && Time.time - lastTouchTime > touchCooldown)
        {
            lastTouchTime = Time.time;
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.AllTypes))
                {
                    Debug.Log($"Raycast başarılı! Yüzeye dokunuldu: {hits[0].pose.position}");
                    Pose hitPose = hits[0].pose;

                    // Nokta oluştur
                    GameObject point = Instantiate(pointPrefab, hitPose.position, hitPose.rotation);
                    points.Add(point);

                    // Çizgi oluştur
                    if (points.Count >= 2)
                    {
                        DrawLineBetweenLastPoints();
                    }

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

    private void DrawLineBetweenLastPoints()
    {
        if (points.Count >= 2)
        {
            // Yeni bir çizgi oluştur
            GameObject line = Instantiate(linePrefab);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            if (lineRenderer != null)
            {
                // Çizgi başlangıç ve bitiş noktalarını ayarla
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, points[points.Count - 2].transform.position);
                lineRenderer.SetPosition(1, points[points.Count - 1].transform.position);

                // Çizgiye renk veya materyal ekle
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;

                // Çizgiyi listeye ekle
                lines.Add(line);
            }
            else
            {
                Debug.LogError("LineRenderer bileşeni bulunamadı!");
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
            Vector3 next = points[(i + 1) % points.Count].transform.position;

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

        Debug.Log($"[Çevre ve Alan Hesaplama] Çevre: {perimeter:F2} metre, Alan: {area:F2} metrekare");
    }

    public void ClearPoints()
    {
        foreach (GameObject point in points)
        {
            Destroy(point);
        }
        points.Clear();

        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();

        if (distanceText != null)
        {
            distanceText.text = "";
        }

        Debug.Log("Tüm noktalar temizlendi.");
    }
}
