using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public Vector3 moveAmount = new Vector3(0, 0, -1);
    public float moveSpeed = 1f;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private bool isMovingUp = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + moveAmount;
    }

    void Update()
    {
        if (isMovingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingUp = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isMovingUp = true;
        }
    }
}