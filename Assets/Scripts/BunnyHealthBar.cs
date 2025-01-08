using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BunnyHealthBar : MonoBehaviour
    {
            [SerializeField] private Slider slider;



            // Start is called once before the first execution of Update after the MonoBehaviour is created
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {
            }


            public void UpdateHealthBar(float currentValue, float maxValue)
            {
                slider.value = currentValue;
            }


    }
}
