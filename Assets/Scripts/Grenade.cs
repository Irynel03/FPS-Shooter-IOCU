using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 5f;
    public float radius = 25f;

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

        if (countdown<=0 && !hasExploded && gameObject.name != "M26 [Prefab]")
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            //Debug.Log("Nearby object: " + nearbyObject.name);
            
            if (nearbyObject.name == "Bone")
            {
                Debug.Log("Enemy hit by grenade");
                //Debug.Log(nearbyObject.GetComponent<BunnyReceiveDmg>());
                if (nearbyObject.GetComponent<BunnyReceiveDmg>() != null)
                {
                    nearbyObject.GetComponent<BunnyReceiveDmg>().take_dmg(5000);
                }
            }
        }

        Destroy(gameObject);
    }
}
