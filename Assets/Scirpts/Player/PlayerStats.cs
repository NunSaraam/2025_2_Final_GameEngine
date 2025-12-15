using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health, stamina, hunger, thirst;

    [Header("Drain Rates")]
    public float hungerDrainRate = 1f;
    public float thirstDrainRate = 2f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 10f;

    [Header("Flags")]
    public bool isRunning;

    public PlayerStatusData ExportStatus()
    {
        return new PlayerStatusData()
        {
            hunger = this.hunger,
            thirst = this.thirst,
            health = this.health,
            stamina = this.stamina
        };
    }

    public void ImportStatus(PlayerStatusData data)
    {
        this.hunger = data.hunger;
        this.thirst = data.thirst;
        this.health = data.health;
        this.stamina = data.stamina;
    }

    void Start()
    {
        LoadFromGameData();
    }

    void LoadFromGameData()
    {
        if (GameDataManager.Instance == null) return;

        ImportStatus(GameDataManager.Instance.playerStatusData);
        Debug.Log("[Load] Player stats loaded!");
    }

    void Update()
    {
        HandleVitals();
        HandleStamina();
        HandleHealthDrain();
    }

    void HandleVitals()
    {
        hunger -= hungerDrainRate * Time.deltaTime / 60f;
        thirst -= thirstDrainRate * Time.deltaTime / 60f;

        hunger = Mathf.Clamp(hunger, 0, 100);
        thirst = Mathf.Clamp(thirst, 0, 100);
    }

    void HandleStamina()
    {
        if (isRunning)
            stamina -= staminaDrainRate * Time.deltaTime;
        else
            stamina += staminaRegenRate * Time.deltaTime;

        stamina = Mathf.Clamp(stamina, 0, 100);
    }

    void HandleHealthDrain()
    {
        if (hunger <= 0 || thirst <= 0)
            health -= 5f * Time.deltaTime;
    }
}
