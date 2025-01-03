using UnityEngine;

public class BunnyReceiveDmg : MonoBehaviour
{
    public bool is_head;

    public int head_break_force;
    public GameObject If_head_then_parts;

    public GameObject killer_buny_without_head;
    public GameObject main_bunny_skin;

    public Texture head_shot_blood_tex;


    public GameObject bunny_main;
    public bool dead;

    public void take_dmg(int dmg)
    {
        if (!is_head)
        {
            if (!dead)
            {
                bunny_main.GetComponent<KillerBunny>().Receive_dmg(dmg, false);
            }
        }
        if (is_head)
        {
            head_break_force -= dmg * 2;

            if (head_break_force < 0)
            {
                head_break();
            }

            if (!dead)
            {
                bunny_main.GetComponent<KillerBunny>().Receive_dmg(dmg * 2, true);
            }
        }
    }

    public void head_break()
    {
        main_bunny_skin.SetActive(false);
        killer_buny_without_head.SetActive(true);
        If_head_then_parts.SetActive(true);

        Destroy(GetComponent<BoxCollider>());
        Destroy(GetComponent<BunnyReceiveDmg>());
    }

    public void Burn_me()
    {

    }
}