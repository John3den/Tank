using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private Rigidbody2D rb;
    private void Start()
    {
        rb.velocity = speed*transform.up;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("obstacle"))
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 newDirection = Vector2.Reflect(transform.up,normal).normalized;
            rb.velocity = newDirection*speed;
            transform.up = rb.velocity;
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            UIController.UIControl.UpdateScore(false);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            UIController.UIControl.UpdateScore(true);
        }
    }
}
