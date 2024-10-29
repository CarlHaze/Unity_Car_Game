using UnityEngine;
public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;
    public int segmentCount = 10;
    public GameObject roadPrefab;

    void Start()
    {
        CreateRoad();
    }

    private void CreateRoad()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector3 position = CalculateBezierPoint(t, points[0], points[1], points[2], points[3]);
            Instantiate(roadPrefab, position, Quaternion.identity, transform);
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // (1-t)^3 * p0
        p += 3 * uu * t * p1; // 3 * (1-t)^2 * t * p1
        p += 3 * u * tt * p2; // 3 * (1-t) * t^2 * p2
        p += ttt * p3;        // t^3 * p3

        return p;
    }
}
