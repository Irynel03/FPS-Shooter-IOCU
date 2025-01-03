using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cam;

    public GameObject aim_point;
    public GameObject shoot_handle;

    // Key inputa

    public bool Key_w;
    public bool Key_s;
    public bool Key_d;
    public bool Key_a;
    public bool key_reload;
    public bool key_jump;
    public bool key_run;


    // Status of walking
    public bool idle;

    public bool forward;
    public bool back;
    public bool right;
    public bool left;

    public bool forward_right;
    public bool back_right;

    public bool forward_left;
    public bool back_left;

    public bool reload;

    public bool jump;
    public bool run;
    public bool cam_toggled;



    private void Start()
    {
        saved_roation_X = vertical_aim_bone_default.transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

       // Here we register the player for the bunnies

        GameObject target_add = GameObject.FindGameObjectWithTag("targets");

        target_add.GetComponent<TargetsForBunny>().Add_Target(gameObject);

        //if (active_assault57.activeSelf)
        //{
            assault57_bool = false;
            assault57_bool = true;

            active_assault57.SetActive(true);
            //Icon_assault57.SetActive(true);
            active_assault57.GetComponent<Assault57Weapon>().Start();

            // restarting animator
            animator_obj.SetActive(false);
            animator_obj.SetActive(true);
        //}
    }


    public float vertical_float_spread;
    public float horizontal_float_spread;

    public int player_health;

    public TextMesh ammo_gui;
    public TextMesh health_gui;

    void FixedUpdate()
    {

        walk_status();
        walk_execute();
        jumping();


        if (Input.GetKeyDown(KeyCode.E))
        {
            action_execute();
        }

        // To disable the aim point, while aimming
        if (Input.GetButton("Fire2"))
        {
            aim_point.SetActive(false);
        }
        else
        {
            aim_point.SetActive(true);
        }

        // Player health status
        health_gui.text = "HP : " + player_health;


        // Current magazine count

        if (active_assault57.activeSelf)
        {
            ammo_gui.text = active_assault57.GetComponent<Assault57Weapon>().magazine_current + " / " + active_assault57.GetComponent<Assault57Weapon>().stored_bullets;
        }


        if (Input.GetKeyDown(KeyCode.L))
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        if (Input.GetKeyDown(KeyCode.O))
        {

            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void LateUpdate()
    {

        aimming();
    }

    public GameObject vertical_aim_bone_default;

    public float min_angle;
    public float max_angle;

    public float vertical_speed;
    public float horizontal_speed;

    public float saved_roation_X;
    public void aimming()
    {

        float add_speed_aim;

        if (Input.GetButton("Fire2"))
        {

            // we slowing down the aimming speed, if the press the aim button Fire2
            add_speed_aim = 0.5f;
        }
        else
        {

            add_speed_aim = 1f;
        }

        // adding recoil movement to the aim

        float add_hor = horizontal_float_spread;
        horizontal_float_spread = 0;


        float add_ver = vertical_float_spread;
        vertical_float_spread = 0;




        transform.eulerAngles = new Vector3(transform.eulerAngles.x, (transform.eulerAngles.y + add_hor) + (Input.GetAxis("Mouse X") * add_speed_aim) * Time.deltaTime * horizontal_speed, transform.eulerAngles.z);



        Vector3 rot = new Vector3((saved_roation_X + add_ver) - (Input.GetAxis("Mouse Y") * add_speed_aim) * Time.deltaTime * vertical_speed, vertical_aim_bone_default.transform.eulerAngles.y, vertical_aim_bone_default.transform.eulerAngles.z);

        // limit of rotation for the aimming
        rot.x = Mathf.Clamp(rot.x, min_angle, max_angle);


        // Here gets the current roation value saved to reuse it, that we don't begin at zero, because the animation overplays all
        saved_roation_X = rot.x;
        vertical_aim_bone_default.transform.eulerAngles = rot;
    }

    //bool is_toggled;
    public Animator ani;
    public float walk_speed;
    public float run_speed;
    //public float Duck_walk_speed;

    public CharacterController controller;

    float forward_back;
    float right_left;

    Vector3 moveDirection;

    public bool in_jump;

    public float jump_speed;

    public bool walking;
    public bool running;
    //public bool walking_side;

    bool changed_state;
    public string state;
    public string old_state;


    public AudioSource walk_sound;

    public AudioClip walk_clip;
    public AudioClip run_clip;
    //public AudioClip walk_side_clip;
    //public AudioClip walk_duck_clip;


    public void walk_execute()
    {
        walking = false;
        running = false;
        //walking_side = false;

        if (idle)
        {
            state = "idle";

            ani.SetInteger("legs", 0);
            walk_speed = 0;
            forward_back = 0;
            right_left = 0;

            walking = false;
            running = false;
        }


        if (forward)
        {
            state = "walk";

            ani.SetFloat("legs_speed", 1);
            ani.SetInteger("legs", 1);
            walk_speed = 250f;

            forward_back = 1.2f;
            right_left = 0;

            walking = true;
            running = false;
        }
        if (back)
        {
            state = "walk";

            ani.SetFloat("legs_speed", -1);
            ani.SetInteger("legs", 1);
            walk_speed = 250f;

            forward_back = -1.2f;
            right_left = 0;

            walking = true;
            running = false;
        }
        if (right)
        {
            state = "side_walk";

            ani.SetFloat("legs_speed", 1);
            ani.SetInteger("legs", 2);
            walk_speed = 150f;

            right_left = 1;

            //walking_side = true;
            walking = true;
            running = false;
        }
        if (left)
        {
            state = "side_walk";

            ani.SetFloat("legs_speed", -1);
            ani.SetInteger("legs", 2);
            walk_speed = 150f;
            right_left = -1;

            //walking_side = true;
            walking = true;
            running = false;
        }
        if (forward_right)
        {
            state = "walk";

            ani.SetFloat("legs_speed", 2);
            ani.SetInteger("legs", 2);
            walk_speed = 150f;

            forward_back = 1;
            right_left = 1;

            walking = true;
            running = false;
        }
        if (forward_left)
        {
            state = "walk";

            ani.SetFloat("legs_speed", 2);
            ani.SetInteger("legs", 2);
            walk_speed = 150f;

            forward_back = 1;
            right_left = -1;

            walking = true;
            running = false;
        }
        if (back_right)
        {
            state = "walk";

            ani.SetFloat("legs_speed", 2);
            ani.SetInteger("legs", 2);
            walk_speed = 150f;

            forward_back = -1;
            right_left = 1;

            walking = true;
            running = false;
        }
        if (back_left)
        {
            state = "walk";

            ani.SetFloat("legs_speed", 2);
            ani.SetInteger("legs", 2);
            walk_speed = 150f;

            forward_back = -1;
            right_left = -1;

            walking = true;
            running = false;
        }
        if (run)
        {
            state = "run";

            ani.SetInteger("legs", 3);
            walk_speed = 400f;

            forward_back = 1.2f;
            right_left = 0;

            walking = false;
            running = true;
        }

        // checking, if the state of the "walking" has changed, if yes, we change the sound
        if (state == "idle" && old_state != "idle")
        {
            old_state = "idle";
            walk_sound.time = 0;
            walk_sound.Stop();
        }
        if (state == "walk" && old_state != "walk")
        {
            old_state = "walk";

            walk_sound.clip = walk_clip;

            walk_sound.time = 0;
            walk_sound.Play();
        }
        if (state == "run" && old_state != "run")
        {
            old_state = "run";
            walk_sound.clip = run_clip;
            walk_sound.time = 0;
            walk_sound.Play();
        }

        moveDirection = new Vector3(Vector3.forward.x * Time.deltaTime * forward_back * walk_speed, 0, Vector3.right.z * Time.deltaTime * forward_back * walk_speed);

        Vector3 FB = transform.TransformDirection(Vector3.forward * forward_back * walk_speed * Time.deltaTime);
        Vector3 RL = transform.TransformDirection(Vector3.right * right_left * walk_speed * Time.deltaTime);
        controller.SimpleMove(FB + RL);
    }

    public void jumping()
    {
        // The drag is always negative, that the player falls down, with pressing the jump key, we add force, which moves the player jump
        if (jump && controller.isGrounded)
        {
            jump_speed = 0.3f;
            transform.position = transform.position + new Vector3(0, 0.1f, 0);
        }

        if (!controller.isGrounded && jump)
        {
            jump_speed -= 0.01f;
            transform.Translate(new Vector3(0, jump_speed, 0));

            if (jump_speed < -0.5f)
            {
                jump_speed = -0.5f;
            }
        }
    }


    bool already_toggles_cam;
    public void walk_status()
    {
        // Reset Movement States
        right = false;
        left = false;
        forward = false;
        back = false;
        forward_right = false;
        forward_left = false;
        back_right = false;
        back_left = false;
        jump = false;
        reload = false;
        run = false;
        idle = false;

        // Handle Key Inputs
        Key_w = Input.GetKey(KeyCode.W);
        Key_s = Input.GetKey(KeyCode.S);
        Key_d = Input.GetKey(KeyCode.D);
        Key_a = Input.GetKey(KeyCode.A);
        key_reload = Input.GetKey(KeyCode.R);
        key_jump = Input.GetKey(KeyCode.Space);
        key_run = Input.GetKey(KeyCode.LeftShift);

        // Determine Movement Direction
        if (Key_w && Key_d)
        {
            forward_right = true;
        }
        else if (Key_w && Key_a)
        {
            forward_left = true;
        }
        else if (Key_s && Key_d)
        {
            back_right = true;
        }
        else if (Key_s && Key_a)
        {
            back_left = true;
        }
        else
        {
            forward = Key_w;
            back = Key_s;
            right = Key_d;
            left = Key_a;
        }

        // Handle Jumping
        if (key_jump)
        {
            jump = true;
        }

        // Handle Reload
        if (key_reload)
        {
            reload = true;
        }

        // Handle Running
        if (key_run && forward)
        {
            run = true;
        }

        // Set Idle State if No Movement
        if (!forward && !back && !right && !left && !forward_right && !forward_left && !back_right && !back_left)
        {
            idle = true;
        }
    }


    public void action_execute()
    {
        // Here we shoot a
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            if (hit.collider.tag == "eq")
            {
                destroy = false;
                //picking_up(hit.transform.GetComponent<equipment>().ID);

                if (destroy)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }


    bool destroy;
    // turning on active weapon and setting up custom adjustments

    //public GameObject drop_assault57;

    // accessing all weapon scripts to controll the change

    public GameObject active_assault57;


    // Icons of the weapons

    //public GameObject Icon_assault57;
    public GameObject assault57_obj;
    public bool assault57_bool;


    public AudioClip click;
    public GameObject animator_obj;

}
