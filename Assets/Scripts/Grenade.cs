using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 5f;
    public float radius = 5f;

    public GameObject explosionEffect;

    private float countdown;
    bool hasExploded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        //foreach (Collider nearbyObject in colliders)
        //{
        //    if(nearbyObject.CompareTag(""))
        //}

        Destroy(gameObject);
    }
}
