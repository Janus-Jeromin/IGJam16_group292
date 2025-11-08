using UnityEngine;

public class AutoDelete : MonoBehaviour
{
    [SerializeField] private float deleteTime = 1.0f;
    private float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= deleteTime)
        {
            var small = Vector2.Lerp(new Vector2(transform.localScale.x, transform.localScale.y), Vector2.zero, Time.deltaTime * 2.0f);
            transform.localScale = new Vector3(small.x, small.y, 1.0f);
            
            if (transform.localScale.x < 0.01f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
