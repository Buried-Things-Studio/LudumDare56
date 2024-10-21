using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WildEncounterDetails : MonoBehaviour
{
    [SerializeField] private Image _profilePictureImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _nameTMP;


    public void PopulateCritterDetails(Critter critter)
    {
        _profilePictureImage.sprite = PictureHelpers.GetProfilePicture(critter);
        _backgroundImage.color = CritterAffinityData.GetAffinityColor(critter.Affinities[0]);
        _nameTMP.color = CritterAffinityData.GetAffinityColor(critter.Affinities[0]);
        _nameTMP.text = critter.Name;
    }
}
