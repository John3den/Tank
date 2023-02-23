using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            Instantiate(bullet,transform.position,transform.rotation);
        }
    }
}
