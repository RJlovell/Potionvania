using UnityEngine;

public class OrcScript : MonoBehaviour
{
    //protected float orcCurrentHealth;
    //public float orcMaxHealth = 3;
    public float orcDamage = 10;

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

    //private void Update()
    //{
    //    IsOrcDead();
    //}

    //public void TakeDamage(float damage)
    //{
    //    orcCurrentHealth -= damage;
    //}

    //public bool IsOrcDead()
    //{
    //    if(orcCurrentHealth <= 0)
    //    {
    //        Debug.Log("The orc has died");
    //        orcCurrentHealth = orcMaxHealth;
    //        gameObject.SetActive(false);
    //        return true;
    //    }
    //    return false;
    //}
}