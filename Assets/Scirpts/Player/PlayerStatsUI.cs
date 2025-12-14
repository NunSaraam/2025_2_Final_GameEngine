using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public Image healthFill;
    public Image staminaFill;
    public Image hungerFill;
    public Image thirstFill;

    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (stats == null) return;

        healthFill.fillAmount = stats.health / 100f;
        staminaFill.fillAmount = stats.stamina / 100f;
        hungerFill.fillAmount = stats.hunger / 100f;
        thirstFill.fillAmount = stats.thirst / 100f;
    }
}
