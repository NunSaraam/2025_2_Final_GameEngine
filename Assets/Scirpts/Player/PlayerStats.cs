using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 100f;
    public float stamina = 100f;
    public float hunger = 100f;
    public float thirst = 100f;

    [Header("Drain Rates")]
    public float hungerDrainRate = 1f;
    public float thirstDrainRate = 2f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;

    [Header("Flags")]
    public bool isRunning;

    void Update()
    {
        // 기본적인 배고픔/목마름 감소 (시간 기반)
        hunger -= hungerDrainRate * Time.deltaTime / 60f;
        thirst -= thirstDrainRate * Time.deltaTime / 60f;

        hunger = Mathf.Clamp(hunger, 0, 100);
        thirst = Mathf.Clamp(thirst, 0, 100);

        // 스태미나 감소 / 회복
        if (isRunning)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
        }
        stamina = Mathf.Clamp(stamina, 0, 100);

        // 배고픔/목마름이 0일 때 체력 감소
        if (hunger <= 0 || thirst <= 0)
        {
            health -= 5f * Time.deltaTime;
        }
    }
}
