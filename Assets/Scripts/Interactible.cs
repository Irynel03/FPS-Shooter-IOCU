using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    //mesage to display when player looks at an interactible object
    public string prompMessage;
    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact() { }


}
