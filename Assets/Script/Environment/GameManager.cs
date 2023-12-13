using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Slider playerSlider;
    [SerializeField] Slider enemySlider;
    public static GameManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSlider.value <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
        else if (enemySlider.value <= 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }
}
