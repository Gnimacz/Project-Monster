using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointRecorder : MonoBehaviour
{
    [SerializeField] private Transform target;
    private List<Transform> points = new List<Transform>();
    private bool isRecording = false;
    private bool isPlaying = false;
    private int currentPoint = 0;
    [SerializeField] private float recordInterval = 0.2f;
    private float recordTimer = 0f;

    public List<Transform> Points { get => points; private set => points = value; }


    // Update is called once per frame
    void Update()
    {
        if (!isRecording) return;
        if (recordTimer > recordInterval)
        {
            RecordPoint();
            recordTimer = 0f;
        }
        recordTimer += Time.deltaTime;
    }

    void RecordPoint()
    {
        points.Add(target);
    }

    void Playback()
    {
        if (currentPoint > points.Count) return;
        target.position = points[currentPoint].position;
        currentPoint++;
    }

    IEnumerator PlayBackRecordedPoints(List<Transform> points)
    {
        isPlaying = true;
        foreach (Transform point in points)
        {
            transform.position = point.position;
            yield return new WaitForSeconds(0.1f);
        }
        isPlaying = false;
    }
}
