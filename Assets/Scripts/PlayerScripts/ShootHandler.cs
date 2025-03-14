using System.Collections.Generic;
using UnityEngine;

public class ShootHandler : MonoBehaviour
{
    public struct add_shoot
    {
        public Vector3 pos;
        public Vector3 rot;
        public int dmg;

        public add_shoot(Vector3 pos, Vector3 rot, int dmg)
        {
            this.pos = pos;
            this.rot = rot;
            this.dmg = dmg;
        }
    }

    private List<add_shoot> added_shoots = new List<add_shoot>();

    void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        foreach (add_shoot s in added_shoots)
        {
            shoot(s.pos, s.rot, s.dmg);
        }

        added_shoots.Clear();
    }

    public void register_shoot(Vector3 pos, Vector3 rot, int dmg)
    {
        added_shoots.Add(new add_shoot(pos, rot, dmg));
    }

    public void shoot(Vector3 pos, Vector3 rot, int dmg)
    {

        RaycastHit hit;

        if (Physics.Raycast(pos, rot, out hit))
        {
            if (hit.collider.tag == "body")
            {
                Debug.Log("Shoot called with pos: " + pos + ", rot: " + rot + ", dmg: " + dmg);
                Debug.Log(hit.collider.gameObject);
                //Debug.Log(hit.collider.gameObject.GetComponent<KillerBunny>);
                //hit.collider.gameObject.GetComponent<KillerBunny>().Receive_dmg(dmg, false);
                hit.collider.gameObject.GetComponent<BunnyReceiveDmg>().take_dmg(dmg);
            }

            if (hit.collider.tag == "metall")
            {
                if (hit.collider.GetComponent<Rigidbody>())
                {
                    hit.collider.GetComponent<Rigidbody>().AddExplosionForce(dmg / 10, hit.point, 10);
                }
            }
        }
    }
}
