using UnityEngine;

public class OrcScript : MonoBehaviour
{
    //protected float orcCurrentHealth;
    //public float orcMaxHealth = 3;
    public int orcDamage;

    //private void Start()
    //{
    //    orcCurrentHealth = orcMaxHealth;
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Colliding with the player without triggering the ray casting");
        }
    }
}