using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promtText;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateText(string promtMessage)
    {
        promtText.text = promtMessage;
    }
}
