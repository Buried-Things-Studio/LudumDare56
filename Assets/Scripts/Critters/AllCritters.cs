using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletAnt : Critter
{
    public BulletAnt()
    {
        Name = "Bullet Ant";
        Affinities.Add(CritterAffinity.Ant);
        Moves.Add(new Snip(){UserGUID = GUID});

        MaxHealth = 15;
        CurrentHealth = 15;
        HealthLevelIncrease = new Vector2Int(2, 5);

        MaxSpeed = 6;
        CurrentSpeed = 6;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 12;
        CurrentSharpAttack = 12;
        SharpAttackLevelIncrease = new Vector2Int(4, 5);

        MaxSharpDefense = 5;
        CurrentSharpDefense = 5;
        SharpDefenseLevelIncrease = new Vector2Int(2, 4);

        MaxBluntAttack = 5;
        CurrentBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 5;
        CurrentBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(2, 4);
    }
}


public class Bumblebee : Critter
{
    public Bumblebee()
    {
        Name = "Bumblebee";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 35;
        CurrentHealth = 35;
        HealthLevelIncrease = new Vector2Int(5, 8);

        MaxSpeed = 2;
        CurrentSpeed = 2;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 10;
        CurrentSharpAttack = 10;
        SharpAttackLevelIncrease = new Vector2Int(3, 5);

        MaxSharpDefense = 3;
        CurrentSharpDefense = 3;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 8;
        CurrentBluntAttack = 8;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 8;
        CurrentBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(2, 4);
    }
}

public class Wasp : Critter
{
    public Wasp()
    {
        Name = "Wasp";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 35;
        CurrentHealth = 35;
        HealthLevelIncrease = new Vector2Int(5, 8);

        MaxSpeed = 2;
        CurrentSpeed = 2;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 10;
        CurrentSharpAttack = 10;
        SharpAttackLevelIncrease = new Vector2Int(3, 5);

        MaxSharpDefense = 3;
        CurrentSharpDefense = 3;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 8;
        CurrentBluntAttack = 8;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 8;
        CurrentBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(2, 4);
    }
}


public class Hornet : Critter
{
    public Hornet()
    {
        Name = "Hornet";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 25;
        CurrentHealth = 25;
        HealthLevelIncrease = new Vector2Int(5, 7);

        MaxSpeed = 3;
        CurrentSpeed = 3;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 15;
        CurrentSharpAttack = 15;
        SharpAttackLevelIncrease = new Vector2Int(4, 6);

        MaxSharpDefense = 2;
        CurrentSharpDefense = 2;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 5;
        CurrentBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(1, 3);

        MaxBluntDefense = 7;
        CurrentBluntDefense = 7;
        BluntDefenseLevelIncrease = new Vector2Int(1, 4);
    }
}


public class ScarabBeetle : Critter
{
    public ScarabBeetle()
    {
        Name = "Scarab Beetle";
        Affinities.Add(CritterAffinity.Beetle);
        Moves.Add(new RollDung(){UserGUID = GUID});
        Moves.Add(new ShellBump(){UserGUID = GUID});

        MaxHealth = 25;
        CurrentHealth = 25;
        HealthLevelIncrease = new Vector2Int(4, 5);

        MaxSpeed = 6;
        CurrentSpeed = 6;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 6;
        CurrentSharpAttack = 6;
        SharpAttackLevelIncrease = new Vector2Int(2, 4);

        MaxSharpDefense = 7;
        CurrentSharpDefense = 7;
        SharpDefenseLevelIncrease = new Vector2Int(3, 5);

        MaxBluntAttack = 5;
        CurrentBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 8;
        CurrentBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(3, 4);
    }
}

public class BlackWidowSpider : Critter
{
    public BlackWidowSpider()
    {
        Name = "Black Widow Spider";
        Affinities.Add(CritterAffinity.Spider);
        Moves.Add(new WebTrap(){UserGUID = GUID});
        Moves.Add(new Snip(){UserGUID = GUID});

        MaxHealth = 20;
        CurrentHealth = 20;
        HealthLevelIncrease = new Vector2Int(2, 4);

        MaxSpeed = 5;
        CurrentSpeed = 5;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 10;
        CurrentSharpAttack = 10;
        SharpAttackLevelIncrease = new Vector2Int(3, 5);

        MaxSharpDefense = 6;
        CurrentSharpDefense = 6;
        SharpDefenseLevelIncrease = new Vector2Int(3, 4);

        MaxBluntAttack = 3;
        CurrentBluntAttack = 3;
        BluntAttackLevelIncrease = new Vector2Int(1, 2);

        MaxBluntDefense = 5;
        CurrentBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(3, 4);
    }
}

public class HawaiianSmilingSpider : Critter
{
    public HawaiianSmilingSpider()
    {
        Name = "Hawaiian Smiling Spider";
        Affinities.Add(CritterAffinity.Spider);
        Moves.Add(new WebTrap(){UserGUID = GUID});
        Moves.Add(new MenacingGrin(){UserGUID = GUID});

        MaxHealth = 18;
        CurrentHealth = 18;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 7;
        CurrentSpeed = 7;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 8;
        CurrentSharpAttack = 8;
        SharpAttackLevelIncrease = new Vector2Int(3, 4);

        MaxSharpDefense = 6;
        CurrentSharpDefense = 6;
        SharpDefenseLevelIncrease = new Vector2Int(3, 4);

        MaxBluntAttack = 5;
        CurrentBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 6;
        CurrentBluntDefense = 6;
        BluntDefenseLevelIncrease = new Vector2Int(3, 4);
    }
}

public class MonarchButterfly : Critter
{
    public MonarchButterfly()
    {
        Name = "Monarch Butterfly";
        Affinities.Add(CritterAffinity.Caterpillar);
        Moves.Add(new WingStrike(){UserGUID = GUID});
        Moves.Add(new Astonish(){UserGUID = GUID});

        MaxHealth = 20;
        CurrentHealth = 20;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 4;
        CurrentSpeed = 4;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 3;
        CurrentSharpAttack = 3;
        SharpAttackLevelIncrease = new Vector2Int(1, 3);

        MaxSharpDefense = 5;
        CurrentSharpDefense = 5;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 9;
        CurrentBluntAttack = 9;
        BluntAttackLevelIncrease = new Vector2Int(4, 5);

        MaxBluntDefense = 7;
        CurrentBluntDefense = 7;
        BluntDefenseLevelIncrease = new Vector2Int(3, 5);
    }
}