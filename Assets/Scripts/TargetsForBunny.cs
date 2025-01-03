using System.Collections.Generic;
using UnityEngine;

public class TargetsForBunny : MonoBehaviour
{
    public List<GameObject> bunny_targets = new List<GameObject>();

    public void Add_Target(GameObject me)
    {
        bunny_targets.Add(me);
    }
}
