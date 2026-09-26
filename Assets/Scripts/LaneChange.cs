using UnityEngine;

public class LaneChange : MonoBehaviour
{
    public static LaneChange Instance;

    public float laneWidth = 2f;
    public int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    public float moveSpeed = 10f;

    Vector3 targetPosition;

    void Awake()
    {
        Instance = this;
        targetPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
            UpdateTargetPosition();
        }
    }

    public void MoveRight()
    {
        if (currentLane < 2)
        {
            currentLane++;
            UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition()
    {
        float xOffset = (currentLane - 1) * laneWidth;
        targetPosition = new Vector3(xOffset, transform.position.y, transform.position.z);
    }
}