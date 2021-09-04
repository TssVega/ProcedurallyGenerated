using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour {

    public Image healthBar;
    public Image manaBar;
    public Image energyBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI energyText;

    public void UpdateHealth(float rate, float amount) {
        healthBar.fillAmount = rate;
        healthText.text = ((int)amount).ToString();
    }
    public void UpdateMana(float rate, float amount) {
        manaBar.fillAmount = rate;
        manaText.text = ((int)amount).ToString();
    }
    public void UpdateEnergy(float rate, float amount) {
        energyBar.fillAmount = rate;
        energyText.text = ((int)amount).ToString();
    }
}
