using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 40;
    [SerializeField] private float distance = 40;

    private float startPos;
    private float startTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.localPosition.x;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float fPP = Mathf.PingPong((Time.time - startTime) * moveSpeed, System.Math.Abs(distance));
        float newX = startPos + fPP * (distance >= 0 ? 1 : -1);
        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
    }
}
