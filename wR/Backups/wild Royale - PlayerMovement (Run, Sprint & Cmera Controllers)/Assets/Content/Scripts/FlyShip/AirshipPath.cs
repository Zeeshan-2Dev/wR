using UnityEngine;

public class AirshipPath : MonoBehaviour
{
    public float speed = 25f;
    public Transform dropZoneCenter;
    public float flightHeight = 200f; // Add this to set altitude

    private Vector3 targetPos;

    void Start()
    {
        Vector3 center = dropZoneCenter.position + Vector3.up * flightHeight;
        float angle = Random.Range(0f, 360f);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

        float spawnDistance = 600f;
        Vector3 spawnPos = center + direction * spawnDistance;
        targetPos = center - direction * spawnDistance;

        transform.position = spawnPos;
        transform.LookAt(targetPos);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 5f)
            Destroy(gameObject);
    }
}
