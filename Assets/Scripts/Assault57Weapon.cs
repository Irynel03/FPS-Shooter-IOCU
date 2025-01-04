using System.Collections;
using UnityEngine;

public class Assault57Weapon : MonoBehaviour
{
    public GameObject player;

    //public GameObject recyle_particles_performance;
    public GameObject shoot_handle;
    public void Start()
    {

        shoot_handle = player.GetComponent<PlayerController>().shoot_handle;

        //recyle_particles_performance = GameObject.FindGameObjectWithTag("rycle");

        ani.SetInteger("assault57", 0);
    }
    private void OnEnable()
    {
        in_reload = false;
        in_shoot = false;
    }

    float Power_bolt;

    bool running;
    bool walking;
    bool reload;


    void Update()
    {
        Input_Status();

        // Input from the main player
        running = player.GetComponent<PlayerController>().running;
        walking = player.GetComponent<PlayerController>().walking;
        reload = player.GetComponent<PlayerController>().reload;
        cam_toggled = player.GetComponent<PlayerController>().cam_toggled;

        // push force, which pushes the shoot animation back.

        if (Power_bolt > -1)
        {
            Power_bolt -= 0.1f;

        }
        // pushing the animation of "shoot" back in the start position/animation as negative force, against the recoil
        if (Power_bolt < -1)
        {
            Power_bolt = -1;
        }

        ani.SetFloat("Power_bolt", Power_bolt);

        // Shooting
        if (button_shoot && !in_shoot && magazine_current > 0 && !in_reload && !running)
        {
            in_shoot = true;
            finished_shoot = false;
            shooting = StartCoroutine(shoot());


        }
        // checking shoot exit
        if (!button_shoot && finished_shoot)
        {
            in_shoot = false;
            current_spread = 0;
        }


        if (button_aim && !button_shoot && !in_reload)
        {
            // aim animation / animator
            ani.SetInteger("assault57", 4);
        }
        if (!button_aim && !button_shoot && !in_reload)
        {
            // idle animation / animator
            ani.SetInteger("assault57", 0);
        }



        // walk status
        if (walking && !in_reload)
        {

            if (!button_aim)
            {
                // idle walk

                ani.SetInteger("assault57", 2);

                //setting animation speed
                ani.SetFloat("weapon_speed", 1);
            }

            if (button_aim)
            {
                ani.SetInteger("assault57", 6);

                // animation speed
                ani.SetFloat("weapon_speed", 1);
            }
        }

        // animation : running
        if (running && !in_reload)
        {
            ani.SetInteger("assault57", 8);
        }


        // aim and shoot
        if (button_aim && button_shoot && !running && !in_reload && magazine_current > 0)
        {
            ani.SetInteger("assault57", 7);
        }
        // idle and shoot
        if (!button_aim && button_shoot && !running && !in_reload && magazine_current > 0)
        {
            ani.SetInteger("assault57", 1);
        }

        // reloading
        if (reload && !in_reload && stored_bullets > 0 && magazine_current != magazine_max)
        {
            reloading = StartCoroutine(reload_start());

        }
    }


    bool button_shoot;
    bool button_aim;
    bool key_reload;
    bool cam_toggled;


    public void Input_Status()
    {

        if (Input.GetButton("Fire1"))
        {
            button_shoot = true;
        }
        else
        {
            button_shoot = false;
        }

        if (Input.GetButton("Fire2"))
        {
            button_aim = true;
        }
        else
        {
            button_aim = false;
        }


        if (Input.GetKey(KeyCode.R))
        {
            key_reload = true;
        }
        else
        {
            key_reload = false;
        }
    }

    public GameObject Shoot_start_point;
    public GameObject muzzle;
    float Push_bolt;

    bool in_shoot;
    Coroutine shooting;
    public bool full_auto;
    public float firerate;
    bool finished_shoot;


    public bool silence;
    public AudioClip silence_shoot_sound;
    public AudioClip shoot_sound;

    public int Joule;

    public float vertical_force;
    public float horizontal_force;

    public float spread_height;
    public float current_spread;

    public IEnumerator shoot()
    {
        yield return new WaitForSeconds(0);

        // increasing the spread, while in automatic fire
        current_spread += spread_height;

        Vector3 Add_spread = Shoot_start_point.transform.forward;

        float hor = Random.Range(-current_spread, current_spread);
        float ver = Random.Range(-current_spread, current_spread);

        Add_spread = new Vector3(hor, ver, 0);

        // pushing the shoot animation completly back to 1
        Power_bolt = 1;

        //bullet_drop();


        // adding recoil -> movement of PlayerController

        float add_ver_force = UnityEngine.Random.Range(0, vertical_force);
        float add_hor_force = UnityEngine.Random.Range(-horizontal_force, horizontal_force);

        // full recoil
        if (!suppressor_a_bool && !suppressor_mac10_bool && !suppressor_c_bool && !suppressor_d_bool)
        {
            player.GetComponent<PlayerController>().vertical_float_spread = -add_ver_force;
            player.GetComponent<PlayerController>().horizontal_float_spread = add_hor_force;
        }


        //  Suppressor = 33% less recoil
        if (suppressor_a_bool || suppressor_mac10_bool || suppressor_c_bool || suppressor_d_bool)
        {
            player.GetComponent<PlayerController>().vertical_float_spread = -add_ver_force / 3;
            player.GetComponent<PlayerController>().horizontal_float_spread = add_hor_force / 3;
        }


        GameObject spawned_muzzle = Instantiate(muzzle, Shoot_start_point.transform.position, Shoot_start_point.transform.rotation);
        spawned_muzzle.GetComponent<muzzle_flash>().origin = Shoot_start_point;


        // adding shoot sound
        if (!suppressor_a_bool && !suppressor_mac10_bool && !suppressor_c_bool && !suppressor_d_bool)
        {
            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = shoot_sound;
            g.GetComponent<AudioSource>().maxDistance = 100;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;
        }
        if (suppressor_a_bool || suppressor_mac10_bool || suppressor_c_bool || suppressor_d_bool)
        {
            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = silence_shoot_sound;
            g.GetComponent<AudioSource>().maxDistance = 100;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;

            //The Suppressor decreases the spread to 33 %
            Add_spread -= (Add_spread / 3);
        }

        //RaycastHit hit;

        // shooting from the camera
        shoot_handle.GetComponent<ShootHandler>().register_shoot(Cam.transform.position, Cam.transform.TransformDirection(Vector3.forward - Add_spread), Joule);

        // If it is fullauto, a firerate delay  get added
        if (full_auto)
        {
            yield return new WaitForSeconds(firerate);
        }

        magazine_current -= 1;

        // checking for enough bullet in magazine
        if (magazine_current > 0 && full_auto && button_shoot)
        {
            StopCoroutine(shoot());
            shooting = StartCoroutine(shoot());
        }

        // break the shoot, if semi auto
        if (!full_auto)
        {
            finished_shoot = true;
            StopCoroutine(shoot());
        }

        // breaking shoot, if we don't press the shoot button
        if (!button_shoot)
        {
            finished_shoot = true;
            StopCoroutine(shoot());
        }

    }


    public int magazine_max;
    public int magazine_current;
    public int stored_bullets;
    public AudioClip reload_unempty;
    public AudioClip reload_empty;


    Coroutine reloading;
    bool in_reload;
    float reload_time;
    bool finished_reload_in_reload;

    public GameObject Clip_on_point;
    public IEnumerator reload_start()
    {


        in_reload = true;
        finished_reload_in_reload = false;


        // 2 different types of reloading :  empty & unempty
        if (magazine_current > 0)
        {
            reload_time = 1.2581f;
            ani.SetInteger("assault57", 5); // unempty reload

            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = reload_unempty;
            g.GetComponent<AudioSource>().maxDistance = 15;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;
        }
        else
        {
            reload_time = 1.79968f;
            ani.SetInteger("assault57", 3); // empty reload

            GameObject g = Instantiate(Clip_on_point, Shoot_start_point.transform.position, transform.rotation);
            g.GetComponent<AudioSource>().clip = reload_empty;
            g.GetComponent<AudioSource>().maxDistance = 15;
            g.GetComponent<AudioSource>().Play();
            g.transform.parent = Shoot_start_point.transform;
        }


        yield return new WaitForSeconds(reload_time);


        // not enough bullets, to fill up the magazine
        if (stored_bullets < magazine_max && !finished_reload_in_reload)
        {
            finished_reload_in_reload = true;
            stored_bullets += magazine_current;
            magazine_current = 0;
            magazine_current = stored_bullets;
            stored_bullets = 0;
        }

        //  enough bullets, to fill up the magazine
        if (stored_bullets > magazine_max || stored_bullets == magazine_max && !finished_reload_in_reload)
        {
            finished_reload_in_reload = true;
            stored_bullets += magazine_current;
            magazine_current = 0;
            magazine_current = magazine_max;
            stored_bullets -= magazine_max;
        }
        // unlock weapon for shooting
        finished_shoot = true;
        in_reload = false;


        StopCoroutine(reload_start());

    }



    public GameObject Cam;
    public Animator ani;



    public Vector3 idle_cam;
    public Vector3 aim_cam;
    public Vector3 run_cam;


    public Vector3 red_dot_a_cam;
    public Vector3 red_dot_b_cam;
    public Vector3 red_dot_c_cam;
    public GameObject red_dot_c_cam_obj;
    public Vector3 red_dot_d_cam;
    public Vector3 red_dot_e_cam;
    public GameObject red_dot_e_cam_obj;

    public Vector3 scope_a_cam;
    public GameObject scope_a_cam_obj;
    public Vector3 scope_b_cam;
    public GameObject scope_b_cam_obj;
    public Vector3 scope_c_cam;
    public GameObject scope_c_cam_obj;

    public bool lamp_a_bool;
    public bool lamp_laser_a_bool;
    public bool lamp_laser_b_bool;
    public bool laser_a_bool;

    public bool red_dot_a_bool;
    public bool red_dot_b_bool;
    public bool red_dot_c_bool;
    public bool red_dot_d_bool;
    public bool red_dot_e_bool;

    public bool scope_a_bool;
    public bool scope_b_bool;
    public bool scope_c_bool;

    public bool suppressor_a_bool;
    public bool suppressor_mac10_bool;
    public bool suppressor_c_bool;
    public bool suppressor_d_bool;


    public GameObject Equipment_holder_a;  // holder for equipments
    public GameObject Equipment_holder_b;

    public GameObject lamp_a;
    public GameObject lamp_laser_a;
    public GameObject lamp_laser_b;
    public GameObject laser_a;

    public GameObject red_dot_a;
    public GameObject red_dot_b;
    public GameObject red_dot_c;
    public GameObject red_dot_d;
    public GameObject red_dot_e;

    public GameObject scope_a;
    public GameObject scope_b;
    public GameObject scope_c;

    public GameObject suppressor_a;
    public GameObject suppressor_mac10;
    public GameObject suppressor_c;
    public GameObject suppressor_d;

}



