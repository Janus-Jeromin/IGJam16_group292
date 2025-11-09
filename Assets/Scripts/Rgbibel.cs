using UnityEngine;
using UnityEngine.UI;


public class Rgbibel : MonoBehaviour
{
    private Color nextColor;

    private float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 1.0f;
        nextColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        
        if(time <= 0.0f)
        {
            time = 1.0f;
            nextColor = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
        }

        var image = GetComponent<Image>();
        image.color = Color.Lerp(image.color, nextColor, Time.deltaTime);
    }
}
