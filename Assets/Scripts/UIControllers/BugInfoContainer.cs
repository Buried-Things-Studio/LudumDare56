using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BugInfoContainer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _bugNameTMP;
    [SerializeField] TextMeshProUGUI _levelTMP;
    [SerializeField] TextMeshProUGUI _healthNumbersTMP;
    [SerializeField] Image _healthBarFillImage;
    [SerializeField] Image _affinityColorFadeImage;


    public void PopulateBugData(Critter critter)
    {
        _bugNameTMP.text = critter.Name;
        _levelTMP.text = $"<size=18>Lv </size><mspace=24>{critter.Level}";
        _affinityColorFadeImage.color = CritterAffinityData.GetAffinityColor(critter.Affinities[0]);

        _healthNumbersTMP.text = $"<mspace=14>{critter.CurrentHealth}/{critter.MaxHealth}";
        _healthBarFillImage.fillAmount = (float)critter.CurrentHealth / (float)critter.MaxHealth;
    }


    public void AnimateHealthBarChange(int healthBefore, int healthAfter, int maxHealth)
    {
        StartCoroutine(AnimateHealthBar(healthBefore, healthAfter, maxHealth));
    }


    public IEnumerator AnimateHealthBar(int healthBefore, int healthAfter, int maxHealth)
    {
        //Debug.Log($"Animating health bar: hp from {healthBefore} > {healthAfter} with max {maxHealth}");
        
        int displayedHealth = healthBefore;
        float displayedDecimalHealth = healthBefore;
        int sign = Mathf.RoundToInt(Mathf.Sign(healthAfter - healthBefore));
        bool isComplete = false;

        while (!isComplete)
        {
            displayedDecimalHealth += Time.deltaTime * sign * 40; //should show a difference of 40hp per second
            displayedHealth = Mathf.Clamp(Mathf.RoundToInt(displayedDecimalHealth), 0, maxHealth); 

            //Debug.Log($"{Mathf.RoundToInt(Mathf.Sign(healthAfter - displayedHealth))} != {sign} // ({healthAfter} - {displayedHealth})");
            
            if (Mathf.RoundToInt(Mathf.Sign(healthAfter - displayedDecimalHealth)) != sign)
            {
                displayedDecimalHealth = healthAfter;
                isComplete = true;
                //Debug.Log("Last health viz update...");
            }

            _healthNumbersTMP.text = $"<mspace=14>{displayedHealth}/{maxHealth}";
            _healthBarFillImage.fillAmount = displayedDecimalHealth / (float)maxHealth;
            //Debug.Log($"{_healthBarFillImage.fillAmount} = {displayedDecimalHealth} / {maxHealth}");

            yield return null;
        }

        _healthNumbersTMP.text = $"<mspace=14>{displayedHealth}/{maxHealth}";
        _healthBarFillImage.fillAmount = displayedDecimalHealth / (float)maxHealth;

        yield return null;
    }
}
