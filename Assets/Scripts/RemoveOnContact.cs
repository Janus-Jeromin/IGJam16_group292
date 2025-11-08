using UnityEngine;
using DefaultNamespace;

public class RemoveOnContact : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.position == GameLogic.Instance.Player.position)
        {
            target.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
