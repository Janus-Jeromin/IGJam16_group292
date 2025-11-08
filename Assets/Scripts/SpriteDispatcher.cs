using UnityEngine;
using System.Collections.Generic;

public class WallRendere : MonoBehaviour
{
    
    [SerializeField] private List<Sprite> wallSprites;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var renderer = GetComponent<SpriteRenderer>();
        
        int index = Random.Range(0, wallSprites.Count);

        Vector2 vecPixelSizeNew = wallSprites[index].bounds.size * wallSprites[index].pixelsPerUnit;
        Vector2 vecPixelSizeOld = renderer.sprite.bounds.size * renderer.sprite.pixelsPerUnit;
        
        Debug.Log(vecPixelSizeNew);
        Debug.Log(vecPixelSizeOld);
        
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.sprite = wallSprites[index];
        
        float fScaleFactor = vecPixelSizeOld.x / vecPixelSizeNew.x / 2.0f;
        
        renderer.size = new Vector2(System.Math.Abs(transform.localScale.x / fScaleFactor),
                                    System.Math.Abs(transform.localScale.y / fScaleFactor));
        transform.localScale = new Vector3(fScaleFactor, fScaleFactor, 1);

        var collider = GetComponent<BoxCollider2D>();

        if (collider != null)
        {
            collider.size = renderer.size;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
