using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PictureHelpers
{
    public static Sprite GetProfilePicture(Critter critter)
    {
        Debug.Log("pic sought");
        return Resources.Load<Sprite>("ProfilePics/" + critter.Name) as Sprite;
    }
}
