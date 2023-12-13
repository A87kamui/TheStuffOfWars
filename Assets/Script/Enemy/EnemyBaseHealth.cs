using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBaseHealth : MonoBehaviour
{
    public EnemyMainBaseController mainBase;
    [SerializeField] Slider healthSlider;
    public GameObject troop;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        mainBase = GetComponentInParent<EnemyMainBaseController>();
        index = mainBase.count - 1; // Get count before it increased
    }

    // Update is called once per frame
    void Update()
    {
        baseUpdate();
        
    }

    /// <summary>
    /// Deactivate base troop
    /// Reduce base count by 1
    /// </summary>
    public void baseUpdate()
    {
        if (healthSlider.value <= 0)
        {
            healthSlider.value = healthSlider.maxValue;
            mainBase.pool[index].SetActive(false);  // Deactivate troop from base pool
            mainBase.count -= 1;
        }
    }
}
