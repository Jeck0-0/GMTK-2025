using TMPro;
using UnityEngine;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] TextMeshProUGUI ammoCounter;

    public void UpdateAmmo(int ammo)
    {
        ammoCounter.text = ammo.ToString();
    }
}