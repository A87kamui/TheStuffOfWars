using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBaseHealth : MonoBehaviour
{
    public PlayerBaseController mainBase;
    [SerializeField] Slider healthSlider;
    public GameObject troop;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        mainBase = GetComponentInParent<PlayerBaseController>();
        index = mainBase.count - 1; // Get count before it increased
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
        
    }

    /// <summary>
    /// Deactivate troop
    /// Reduce base count by 1
    /// </summary>
    public void baseUpdate()
    {
        //Debug.Log("Health: " + healthSlider.value);
        if (healthSlider.value <= 0)
        {
            healthSlider.value = healthSlider.maxValue;
            mainBase.pool[index].SetActive(false);
            mainBase.count -= 1;
        }
    }
}
