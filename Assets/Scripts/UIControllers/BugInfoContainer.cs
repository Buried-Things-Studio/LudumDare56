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
    [SerializeField] TextMeshProUGUI _abilityTMP;
    [SerializeField] Image _healthBarFillImage;
    [SerializeField] Image _affinityColorFadeImage;
    [SerializeField] private Image _typeIconImage;


    [SerializeField] RectTransform _catchThresholdBarRT;
    [SerializeField] Image _catchThresholdBarImage;
    [SerializeField] TextMeshProUGUI _catchThresholdBarTMP;

    private Color _catchThresholdInactiveColor = new Color32(0x48, 0x48, 0x48, 0xFF); //#484848FF
    private Color _catchThresholdActiveColor = new Color32(0x2C, 0x96, 0x3E, 0xFF); //#2C963EFF
    private float _maxCatchThresholdX = 332; //this is just eyeballed
    private bool _isWildCritter;


    public void PopulateBugData(Critter critter, bool isWildCritter)
    {
        _isWildCritter = isWildCritter;
        
        _bugNameTMP.text = critter.Name;
        _levelTMP.text = $"<size=18>Lv </size><mspace=16>{critter.Level}";
        _affinityColorFadeImage.color = CritterAffinityData.GetAffinityColor(critter.Affinities[0]);
        _abilityTMP.text = $"Ability: {(critter.Ability == null ? "None" : critter.Ability.Name)}";

        _healthNumbersTMP.text = $"<mspace=14>{critter.CurrentHealth}/{critter.MaxHealth}";
        _healthBarFillImage.fillAmount = (float)critter.CurrentHealth / (float)critter.MaxHealth;

        _typeIconImage.sprite = PictureHelpers.GetBugAffinityPicture(critter);
        _typeIconImage.color = CritterAffinityData.GetAffinityColor(critter.Affinities[0]);

        if (!_isWildCritter)
        {
            _catchThresholdBarImage.gameObject.SetActive(false);
        }
        else
        {
            _catchThresholdBarImage.gameObject.SetActive(true);
            
            _catchThresholdBarRT.anchoredPosition = new Vector2(_maxCatchThresholdX * CritterHelpers.GetCatchHealthThresholdFraction(critter), _catchThresholdBarRT.anchoredPosition.y);
            bool isCatchable = critter.CurrentHealth <= CritterHelpers.GetCatchHealthThreshold(critter);
            _catchThresholdBarImage.color = isCatchable ? _catchThresholdActiveColor : _catchThresholdInactiveColor;
            _catchThresholdBarTMP.color = isCatchable ? _catchThresholdActiveColor : _catchThresholdInactiveColor;
        }
    }


    public void AnimateHealthBarChange(int level, int healthBefore, int healthAfter, int maxHealth)
    {
        StartCoroutine(AnimateHealthBar(level, healthBefore, healthAfter, maxHealth));
    }


    private void UpdateCatchThresholdColor(int currentHealth, int maxHealth, int level)
    {
        if (!_isWildCritter)
        {
            return;
        }
        
        bool isCatchable = currentHealth <= CritterHelpers.GetCatchHealthThreshold(maxHealth, level);
        _catchThresholdBarImage.color = isCatchable ? _catchThresholdActiveColor : _catchThresholdInactiveColor;
        _catchThresholdBarTMP.color = isCatchable ? _catchThresholdActiveColor : _catchThresholdInactiveColor;
    }


    public IEnumerator AnimateHealthBar(int level, int healthBefore, int healthAfter, int maxHealth)
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
            UpdateCatchThresholdColor(displayedHealth, maxHealth, level);

            yield return null;
        }

        _healthNumbersTMP.text = $"<mspace=14>{displayedHealth}/{maxHealth}";
        _healthBarFillImage.fillAmount = displayedDecimalHealth / (float)maxHealth;

        yield return null;
    }
}
