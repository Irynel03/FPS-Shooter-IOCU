using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance=3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    void Start()
    {
        cam=GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        playerUI.UpdateText("");
        Ray ray= new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance,mask))
        {
            if (hitInfo.collider.GetComponent<Interactible>()!=null)
            {
                Interactible interactible = hitInfo.collider.GetComponent<Interactible>();
                playerUI.UpdateText(interactible.prompMessage);
               if (inputManager.onFoot.Interact.triggered)
                {
                    interactible.BaseInteract();
                }
            }
        }
    }
}
