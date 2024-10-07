using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BananaSlug : Critter
{
    public BananaSlug()
    {
        Name = "Banana Slug";
        Affinities.Add(CritterAffinity.Mollusc);
        Moves.Add(new Smother(){UserGUID = GUID});
        Moves.Add(new MysteriousSlime(){UserGUID = GUID});

        MaxHealth = 35;
        CurrentHealth = 35;
        HealthLevelIncrease = new Vector2Int(5, 8);

        MaxSpeed = 2;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 1;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 4;
        SharpDefenseLevelIncrease = new Vector2Int(1, 4);

        MaxBluntAttack = 3;
        BluntAttackLevelIncrease = new Vector2Int(1, 3);

        MaxBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(4, 8);
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
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 10;
        SharpAttackLevelIncrease = new Vector2Int(4, 6);

        MaxSharpDefense = 6;
        SharpDefenseLevelIncrease = new Vector2Int(3, 4);

        MaxBluntAttack = 3;
        BluntAttackLevelIncrease = new Vector2Int(1, 2);

        MaxBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(3, 4);
    }
}


public class BulletAnt : Critter
{
    public BulletAnt()
    {
        Name = "Bullet Ant";
        Affinities.Add(CritterAffinity.Ant);
        Moves.Add(new Snip(){UserGUID = GUID});

        MaxHealth = 17;
        CurrentHealth = 17;
        HealthLevelIncrease = new Vector2Int(2, 5);

        MaxSpeed = 6;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 14;
        SharpAttackLevelIncrease = new Vector2Int(4, 5);

        MaxSharpDefense = 5;
        SharpDefenseLevelIncrease = new Vector2Int(2, 4);

        MaxBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(2, 4);
    }
}


// public class Bumblebee : Critter
// {
//     public Bumblebee()
//     {
//         Name = "Bumblebee";
//         Affinities.Add(CritterAffinity.Bee);
//         Moves.Add(new Bonk(){UserGUID = GUID});
//         Moves.Add(new HoneyDrink(){UserGUID = GUID});

//         MaxHealth = 35;
//         CurrentHealth = 35;
//         HealthLevelIncrease = new Vector2Int(5, 8);

//         MaxSpeed = 2;
//         SpeedLevelIncrease = new Vector2Int(1, 2);

//         MaxSharpAttack = 10;
//         SharpAttackLevelIncrease = new Vector2Int(3, 5);

//         MaxSharpDefense = 3;
//         SharpDefenseLevelIncrease = new Vector2Int(2, 3);

//         MaxBluntAttack = 8;
//         BluntAttackLevelIncrease = new Vector2Int(2, 3);

//         MaxBluntDefense = 8;
//         BluntDefenseLevelIncrease = new Vector2Int(2, 4);
//     }
// }


public class FireAnt : Critter
{
    public FireAnt()
    {
        Name = "Fire Ant";
        Affinities.Add(CritterAffinity.Ant);
        Moves.Add(new Snip(){UserGUID = GUID});

        MaxHealth = 15;
        CurrentHealth = 15;
        HealthLevelIncrease = new Vector2Int(2, 3);

        MaxSpeed = 7;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 9;
        SharpAttackLevelIncrease = new Vector2Int(3, 5);

        MaxSharpDefense = 7;
        SharpDefenseLevelIncrease = new Vector2Int(3, 4);

        MaxBluntAttack = 7;
        BluntAttackLevelIncrease = new Vector2Int(2, 4);

        MaxBluntDefense = 7;
        BluntDefenseLevelIncrease = new Vector2Int(2, 3);
    }
}


public class GardenSnail : Critter
{
    public GardenSnail()
    {
        Name = "Garden Snail";
        Affinities.Add(CritterAffinity.Mollusc);
        Moves.Add(new Smother(){UserGUID = GUID});
        Moves.Add(new MysteriousSlime(){UserGUID = GUID});

        MaxHealth = 22;
        CurrentHealth = 22;
        HealthLevelIncrease = new Vector2Int(2, 4);

        MaxSpeed = 3;
        SpeedLevelIncrease = new Vector2Int(1, 3);

        MaxSharpAttack = 2;
        SharpAttackLevelIncrease = new Vector2Int(1, 4);

        MaxSharpDefense = 8;
        SharpDefenseLevelIncrease = new Vector2Int(2, 6);

        MaxBluntAttack = 3;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(3, 6);
    }
}



public class GhostAnt : Critter
{
    public GhostAnt()
    {
        Name = "Ghost Ant";
        Affinities.Add(CritterAffinity.Ant);
        Moves.Add(new Snip(){UserGUID = GUID});

        MaxHealth = 10;
        CurrentHealth = 10;
        HealthLevelIncrease = new Vector2Int(1, 3);

        MaxSpeed = 9;
        SpeedLevelIncrease = new Vector2Int(2, 5);

        MaxSharpAttack = 6;
        SharpAttackLevelIncrease = new Vector2Int(2, 4);

        MaxSharpDefense = 2;
        SharpDefenseLevelIncrease = new Vector2Int(1, 3);

        MaxBluntAttack = 6;
        BluntAttackLevelIncrease = new Vector2Int(1, 3);

        MaxBluntDefense = 2;
        BluntDefenseLevelIncrease = new Vector2Int(1, 3);
    }
}


public class GreyFieldSnail : Critter
{
    public GreyFieldSnail()
    {
        Name = "Grey Field Snail";
        Affinities.Add(CritterAffinity.Mollusc);
        Moves.Add(new Smother(){UserGUID = GUID});
        Moves.Add(new MysteriousSlime(){UserGUID = GUID});

        MaxHealth = 17;
        CurrentHealth = 17;
        HealthLevelIncrease = new Vector2Int(3, 4);

        MaxSpeed = 2;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 2;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 4;
        SharpDefenseLevelIncrease = new Vector2Int(1, 3);

        MaxBluntAttack = 2;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 7;
        BluntDefenseLevelIncrease = new Vector2Int(3, 6);
    }
}


public class HawaiianSmilingSpider : Critter
{
    public HawaiianSmilingSpider()
    {
        Name = "Hawaiian Smiling Spider";
        Affinities.Add(CritterAffinity.Spider);
        Moves.Add(new MenacingGrin(){UserGUID = GUID});
        Moves.Add(new Snip(){UserGUID = GUID});

        MaxHealth = 18;
        CurrentHealth = 18;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 7;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 8;
        SharpAttackLevelIncrease = new Vector2Int(3, 4);

        MaxSharpDefense = 6;
        SharpDefenseLevelIncrease = new Vector2Int(3, 4);

        MaxBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 6;
        BluntDefenseLevelIncrease = new Vector2Int(3, 4);
    }
}


public class HerculesBeetle : Critter
{
    public HerculesBeetle()
    {
        Name = "Hercules Beetle";
        Affinities.Add(CritterAffinity.Beetle);
        Moves.Add(new ShellBump(){UserGUID = GUID});
        Moves.Add(new Carapace(){UserGUID = GUID});

        MaxHealth = 25;
        CurrentHealth = 25;
        HealthLevelIncrease = new Vector2Int(2, 5);

        MaxSpeed = 3;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 7;
        SharpAttackLevelIncrease = new Vector2Int(1, 3);

        MaxSharpDefense = 8;
        SharpDefenseLevelIncrease = new Vector2Int(2, 4);

        MaxBluntAttack = 10;
        BluntAttackLevelIncrease = new Vector2Int(3, 5);

        MaxBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(1, 4);
    }
}


public class Honeybee : Critter
{
    public Honeybee()
    {
        Name = "Honeybee";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 20;
        CurrentHealth = 20;
        HealthLevelIncrease = new Vector2Int(2, 4);

        MaxSpeed = 5;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 10;
        SharpAttackLevelIncrease = new Vector2Int(2, 3);

        MaxSharpDefense = 5;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 4;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(2, 3);
    }
}


// public class Hornet : Critter
// {
//     public Hornet()
//     {
//         Name = "Hornet";
//         Affinities.Add(CritterAffinity.Bee);
//         Moves.Add(new Bonk(){UserGUID = GUID});
//         Moves.Add(new HoneyDrink(){UserGUID = GUID});

//         MaxHealth = 25;
//         CurrentHealth = 25;
//         HealthLevelIncrease = new Vector2Int(5, 7);

//         MaxSpeed = 3;
//         SpeedLevelIncrease = new Vector2Int(2, 3);

//         MaxSharpAttack = 15;
//         SharpAttackLevelIncrease = new Vector2Int(4, 6);

//         MaxSharpDefense = 2;
//         SharpDefenseLevelIncrease = new Vector2Int(2, 3);

//         MaxBluntAttack = 5;
//         BluntAttackLevelIncrease = new Vector2Int(1, 3);

//         MaxBluntDefense = 7;
//         BluntDefenseLevelIncrease = new Vector2Int(1, 4);
//     }
// }


public class IoMoth : Critter
{
    public IoMoth()
    {
        Name = "Io Moth";
        Affinities.Add(CritterAffinity.Butterfly);
        Moves.Add(new WingStrike(){UserGUID = GUID});
        Moves.Add(new Dazzle(){UserGUID = GUID});

        MaxHealth = 20;
        CurrentHealth = 20;
        HealthLevelIncrease = new Vector2Int(1, 6);

        MaxSpeed = 7;
        SpeedLevelIncrease = new Vector2Int(2, 5);

        MaxSharpAttack = 2;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 6;
        SharpDefenseLevelIncrease = new Vector2Int(1, 5);

        MaxBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(1, 6);

        MaxBluntDefense = 6;
        BluntDefenseLevelIncrease = new Vector2Int(1, 5);
    }
}


public class LeptopomaSnail : Critter
{
    public LeptopomaSnail()
    {
        Name = "Leptopoma";
        Affinities.Add(CritterAffinity.Mollusc);
        Moves.Add(new Smother(){UserGUID = GUID});
        Moves.Add(new MysteriousSlime(){UserGUID = GUID});

        MaxHealth = 20;
        CurrentHealth = 20;
        HealthLevelIncrease = new Vector2Int(3, 8);

        MaxSpeed = 1;
        SpeedLevelIncrease = new Vector2Int(1, 2);

        MaxSharpAttack = 1;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 10;
        SharpDefenseLevelIncrease = new Vector2Int(3, 8);

        MaxBluntAttack = 3;
        BluntAttackLevelIncrease = new Vector2Int(1, 2);

        MaxBluntDefense = 10;
        BluntDefenseLevelIncrease = new Vector2Int(5, 8);
    }
}


public class KerrySnail : Critter
{
    public KerrySnail()
    {
        Name = "Kerry Snail";
        Affinities.Add(CritterAffinity.Mollusc);
        Moves.Add(new Smother(){UserGUID = GUID});
        Moves.Add(new MysteriousSlime(){UserGUID = GUID});

        MaxHealth = 25;
        CurrentHealth = 25;
        HealthLevelIncrease = new Vector2Int(2, 6);

        MaxSpeed = 2;
        SpeedLevelIncrease = new Vector2Int(1, 8);

        MaxSharpAttack = 2;
        SharpAttackLevelIncrease = new Vector2Int(1, 8);

        MaxSharpDefense = 3;
        SharpDefenseLevelIncrease = new Vector2Int(1, 8);

        MaxBluntAttack = 2;
        BluntAttackLevelIncrease = new Vector2Int(1, 8);

        MaxBluntDefense = 6;
        BluntDefenseLevelIncrease = new Vector2Int(4, 8);
    }
}


public class Ladybird : Critter
{
    public Ladybird()
    {
        Name = "Ladybird";
        Affinities.Add(CritterAffinity.Beetle);
        Moves.Add(new ShellBump(){UserGUID = GUID});
        Moves.Add(new Carapace(){UserGUID = GUID});

        MaxHealth = 18;
        CurrentHealth = 18;
        HealthLevelIncrease = new Vector2Int(1, 5);

        MaxSpeed = 4;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 6;
        SharpAttackLevelIncrease = new Vector2Int(2, 4);

        MaxSharpDefense = 7;
        SharpDefenseLevelIncrease = new Vector2Int(1, 4);

        MaxBluntAttack = 5;
        BluntAttackLevelIncrease = new Vector2Int(1, 4);

        MaxBluntDefense = 4;
        BluntDefenseLevelIncrease = new Vector2Int(1, 4);
    }
}


public class LimeButterfly : Critter
{
    public LimeButterfly()
    {
        Name = "Lime Butterfly";
        Affinities.Add(CritterAffinity.Butterfly);
        Moves.Add(new WingStrike(){UserGUID = GUID});
        Moves.Add(new Dazzle(){UserGUID = GUID});

        MaxHealth = 10;
        CurrentHealth = 10;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 6;
        SpeedLevelIncrease = new Vector2Int(2, 4);

        MaxSharpAttack = 2;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 4;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 2;
        BluntAttackLevelIncrease = new Vector2Int(1, 2);

        MaxBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(1, 3);
    }
}


public class MammothButterfly : Critter
{
    public MammothButterfly()
    {
        Name = "Mammoth Butterfly";
        Affinities.Add(CritterAffinity.Butterfly);
        Moves.Add(new WingStrike(){UserGUID = GUID});
        Moves.Add(new Dazzle(){UserGUID = GUID});

        MaxHealth = 30;
        CurrentHealth = 30;
        HealthLevelIncrease = new Vector2Int(3, 5);

        MaxSpeed = 5;
        SpeedLevelIncrease = new Vector2Int(1, 3);

        MaxSharpAttack = 1;
        SharpAttackLevelIncrease = new Vector2Int(1, 1);

        MaxSharpDefense = 7;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 6;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 7;
        BluntDefenseLevelIncrease = new Vector2Int(2, 3);
    }
}


public class MonarchButterfly : Critter
{
    public MonarchButterfly()
    {
        Name = "Monarch Butterfly";
        Affinities.Add(CritterAffinity.Butterfly);
        Moves.Add(new WingStrike(){UserGUID = GUID});
        Moves.Add(new Dazzle(){UserGUID = GUID});

        MaxHealth = 16;
        CurrentHealth = 16;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 8;
        SpeedLevelIncrease = new Vector2Int(3, 5);

        MaxSharpAttack = 4;
        SharpAttackLevelIncrease = new Vector2Int(2, 3);

        MaxSharpDefense = 5;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 2;
        BluntAttackLevelIncrease = new Vector2Int(1, 3);

        MaxBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(1, 3);
    }
}


public class PandaAnt : Critter
{
    public PandaAnt()
    {
        Name = "Panda Ant";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 18;
        CurrentHealth = 18;
        HealthLevelIncrease = new Vector2Int(2, 4);

        MaxSpeed = 3;
        SpeedLevelIncrease = new Vector2Int(1, 3);

        MaxSharpAttack = 12;
        SharpAttackLevelIncrease = new Vector2Int(3, 4);

        MaxSharpDefense = 5;
        SharpDefenseLevelIncrease = new Vector2Int(1, 4);

        MaxBluntAttack = 3;
        BluntAttackLevelIncrease = new Vector2Int(2, 4);

        MaxBluntDefense = 5;
        BluntDefenseLevelIncrease = new Vector2Int(2, 4);
    }
}


public class PotatoBeetle : Critter
{
    public PotatoBeetle()
    {
        Name = "Potato Beetle";
        Affinities.Add(CritterAffinity.Beetle);
        Moves.Add(new ShellBump(){UserGUID = GUID});
        Moves.Add(new Carapace(){UserGUID = GUID});

        MaxHealth = 16;
        CurrentHealth = 16;
        HealthLevelIncrease = new Vector2Int(3, 5);

        MaxSpeed = 6;
        SpeedLevelIncrease = new Vector2Int(2, 4);

        MaxSharpAttack = 2;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 10;
        SharpDefenseLevelIncrease = new Vector2Int(3, 4);

        MaxBluntAttack = 6;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 8;
        BluntDefenseLevelIncrease = new Vector2Int(1, 2);
    }
}


public class RhinoBeetle : Critter
{
    public RhinoBeetle()
    {
        Name = "Rhino Beetle";
        Affinities.Add(CritterAffinity.Beetle);
        Moves.Add(new ShellBump(){UserGUID = GUID});
        Moves.Add(new Carapace(){UserGUID = GUID});

        MaxHealth = 20;
        CurrentHealth = 20;
        HealthLevelIncrease = new Vector2Int(2, 4);

        MaxSpeed = 4;
        SpeedLevelIncrease = new Vector2Int(1, 4);

        MaxSharpAttack = 5;
        SharpAttackLevelIncrease = new Vector2Int(1, 2);

        MaxSharpDefense = 6;
        SharpDefenseLevelIncrease = new Vector2Int(2, 3);

        MaxBluntAttack = 12;
        BluntAttackLevelIncrease = new Vector2Int(2, 5);

        MaxBluntDefense = 4;
        BluntDefenseLevelIncrease = new Vector2Int(2, 3);
    }
}


// public class ScarabBeetle : Critter
// {
//     public ScarabBeetle()
//     {
//         Name = "Scarab Beetle";
//         Affinities.Add(CritterAffinity.Beetle);
//         Moves.Add(new RollDung(){UserGUID = GUID});
//         Moves.Add(new ShellBump(){UserGUID = GUID});

//         MaxHealth = 25;
//         CurrentHealth = 25;
//         HealthLevelIncrease = new Vector2Int(4, 5);

//         MaxSpeed = 6;
//         SpeedLevelIncrease = new Vector2Int(2, 3);

//         MaxSharpAttack = 6;
//         SharpAttackLevelIncrease = new Vector2Int(2, 4);

//         MaxSharpDefense = 7;
//         SharpDefenseLevelIncrease = new Vector2Int(3, 5);

//         MaxBluntAttack = 5;
//         BluntAttackLevelIncrease = new Vector2Int(2, 3);

//         MaxBluntDefense = 8;
//         BluntDefenseLevelIncrease = new Vector2Int(3, 4);
//     }
// }


// public class Wasp : Critter
// {
//     public Wasp()
//     {
//         Name = "Wasp";
//         Affinities.Add(CritterAffinity.Bee);
//         Moves.Add(new Bonk(){UserGUID = GUID});
//         Moves.Add(new HoneyDrink(){UserGUID = GUID});

//         MaxHealth = 35;
//         CurrentHealth = 35;
//         HealthLevelIncrease = new Vector2Int(5, 8);

//         MaxSpeed = 2;
//         SpeedLevelIncrease = new Vector2Int(1, 2);

//         MaxSharpAttack = 10;
//         SharpAttackLevelIncrease = new Vector2Int(3, 5);

//         MaxSharpDefense = 3;
//         SharpDefenseLevelIncrease = new Vector2Int(2, 3);

//         MaxBluntAttack = 8;
//         BluntAttackLevelIncrease = new Vector2Int(2, 3);

//         MaxBluntDefense = 8;
//         BluntDefenseLevelIncrease = new Vector2Int(2, 4);
//     }
// }


public class YellowjacketWasp : Critter
{
    public YellowjacketWasp()
    {
        Name = "Yellowjacket Wasp";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 16;
        CurrentHealth = 16;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 7;
        SpeedLevelIncrease = new Vector2Int(2, 3);

        MaxSharpAttack = 12;
        SharpAttackLevelIncrease = new Vector2Int(2, 4);

        MaxSharpDefense = 4;
        SharpDefenseLevelIncrease = new Vector2Int(1, 3);

        MaxBluntAttack = 2;
        BluntAttackLevelIncrease = new Vector2Int(2, 3);

        MaxBluntDefense = 4;
        BluntDefenseLevelIncrease = new Vector2Int(1, 3);
    }
}