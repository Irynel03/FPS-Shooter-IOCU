using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class KillerBunny : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;

    public float walk_speed;
    public float hit_range;
    public Transform head;

    private void Start()
    {
        player = GameObject.Find("Player_New");

        //GameObject g = GameObject.Find("props_active");
        //g.GetComponent<find_destory_able_props>().objs_7.Add(head);

        sound_next_ticks = UnityEngine.Random.Range(250, 1000);
        agent.GetComponent<NavMeshAgent>().avoidancePriority = UnityEngine.Random.Range(0, 100);

        target_source = GameObject.FindGameObjectWithTag("targets");

        StartCoroutine(Target_update());
    }

    bool in_walk;

    public int health;
    public bool in_hitting;
    public int walking_stuck_ticks;

    bool already_died;
    public bool hit_ready;

    public GameObject[] Target_list;
    public List<GameObject> Target_list_active_check = new List<GameObject>();
    public GameObject target_source;

    void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        if (already_died)
        {
            return;
        }

        if (health < 1)
        {
            StopAllCoroutines();
        }

        sound_next_ticks -= 1;
        walking_stuck_ticks -= 1;

        if (sound_next_ticks < 0 && !in_bunny_sound)
        {
            ani.SetInteger("mouth", 1);
            in_bunny_sound = true;
        }

        if (Target_list.Length > 0)
        {
            if (Vector3.Distance(transform.position, Target_list[0].transform.position) < hit_range)
            {
                // hit
                agent.speed = 0;
                ani.SetInteger("corp", 2);
                ani.SetInteger("legs", 0);
                in_hitting = true;
                in_walk = false;
                hit_ready = true;

                DealDamageToPlayer();
            }
            else
            {
                // move to the player

                Debug.Log("string"+Target_list[0]);
                Debug.Log("string2"+Target_list[0].transform.position);
                agent.SetDestination(Target_list[0].transform.position);
               

                agent.speed = walk_speed;
                in_hitting = false;
                in_walk = true;
                hit_ready = false;
                ani.SetInteger("legs", 1);

                // choosing between 2 walk animation, to make the killybunny less static in walking
                if (!in_corp_walk_determining)
                {
                    in_corp_walk_determining = true;
                    corp_walk_animation_r = StartCoroutine(Corp_walk_animation());
                }
            }
        }
    }

    public float attackCooldown = 2f;
    private float lastAttackTime = -Mathf.Infinity;

    private void DealDamageToPlayer()
    {
        if (hit_ready && player != null && Time.time >= lastAttackTime + attackCooldown)
        {
            var playerHealth = player.GetComponent<PlayerController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(20);
                hit_ready = false;
                lastAttackTime = Time.time;
            }
        }
    }


    public IEnumerator Target_update()
    {
        yield return new WaitForSeconds(0.1f);

        Target_list = target_source.GetComponent<TargetsForBunny>().bunny_targets.ToArray();

        Target_list = Target_list.OrderBy(point => Vector3.Distance(transform.position, point.transform.position)).ToArray();

        Target_list_active_check.Clear();

        foreach (GameObject g in Target_list)
        {
            if (g.activeSelf)
            {
                Target_list_active_check.Add(g);
            }
        }

        Target_list = Target_list_active_check.ToArray();
        StartCoroutine(Target_update());
    }

    public void Receive_dmg(int dmg, bool headshot)
    {
        health -= dmg;

        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public Animator ani;

    public IEnumerator Hit()
    {
        yield return new WaitForSeconds(0);
    }

    public GameObject Audio_spawn;
    public AudioClip Bunny_sound;

    public int sound_next_ticks;
    public bool in_bunny_sound;


    bool in_corp_walk_determining;
    Coroutine corp_walk_animation_r;

    public IEnumerator Corp_walk_animation()
    {
        yield return new WaitForSeconds(0);

        in_corp_walk_determining = true;

        int r = UnityEngine.Random.Range(1, 4);
        float l = 0.411f;

        if (r == 0)
        {
            ani.SetInteger("corp", 0);
        }
        if (r == 1)
        {
            ani.SetInteger("corp", 0);
        }
        if (r == 2)
        {
            ani.SetInteger("corp", 0);
        }
        if (r == 3)
        {
            ani.SetInteger("corp", 1);
        }

        if (in_walk == false)
        {
            StopCoroutine(corp_walk_animation_r);
        }

        yield return new WaitForSeconds(l);
        in_corp_walk_determining = false;
    }
}
