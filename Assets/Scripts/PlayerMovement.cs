using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private void Update()
    {
        transform.Rotate(new Vector3(0,0,-Input.GetAxis("Horizontal")*3));
        if (Input.GetKey("w") || Input.GetKey("s"))
        {
            rb.velocity = 2*transform.up*Input.GetAxis("Vertical");
        }
        else
        {
            rb.velocity = new Vector3(0,0,0);
        }
    }
}
