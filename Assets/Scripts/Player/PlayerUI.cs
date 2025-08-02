using TMPro;
using UnityEngine;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] GameObject deathScreen;

    public void UpdateAmmo(int ammo)
    {
        ammoCounter.text = ammo.ToString();
    }
    public void SetDeathScreen(bool on)
    {
        deathScreen.SetActive(on);
    }
}