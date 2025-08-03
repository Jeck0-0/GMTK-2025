using TMPro;
using UnityEngine;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] TextMeshProUGUI ammoCounter;
    [SerializeField] GameObject deathScreen;
    [SerializeField] GameObject reloadHint;
    [SerializeField] GameObject ESCHint;

    public void UpdateAmmo(int ammo)
    {
        ammoCounter.text = ammo.ToString();
        if (ammo <= 0)
        {
            reloadHint.SetActive(true);
        }
        else
        {
            reloadHint.SetActive(false);
        }
    }
    public void SetDeathScreen(bool on)
    {
        reloadHint.SetActive(false);
        deathScreen.SetActive(on);
    }
    public void SetESCHint(bool on)
    {
        ESCHint.SetActive(on);
    }
}