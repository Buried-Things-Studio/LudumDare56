using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PictureHelpers
{
    public static Sprite GetProfilePicture(Critter critter)
    {
        return Resources.Load<Sprite>("ProfilePics/" + critter.Name) as Sprite;
    }


    public static Sprite GetMoveAffinityPicture(Move move)
    {
        return Resources.Load<Sprite>("Affinities/" + move.Affinity.ToString()) as Sprite;
    }


    public static Sprite GetBugAffinityPicture(Critter critter)
    {
        return Resources.Load<Sprite>("Affinities/" + critter.Affinities[0].ToString()) as Sprite;
    }
}
