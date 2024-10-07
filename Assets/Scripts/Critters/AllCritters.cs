using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BananaSlug : Critter
{
    public BananaSlug()
    {
        Name = "Banana Slug";
        Lore = "Grimy and shiny she will soak you in her slime. To the ground you will return, decaying before your time.";
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
        Lore = "The reputation precedes her. None dare come nearer. Evil avoids her. All fear the killer.";
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
        Lore = "Methodical, relentless, he is known and he’s ferocious. Silence follows where he goes, his sting beyond atrocious. On fire yet frozen - he paralyses his opponents.";
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
        Lore = "Hear the sirens, beware the fire. Feel his wrath, flee his ire. Gardens will burn, and bugs collapse. They still hope to escape, perhaps.";
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
        Lore = "He’ll be there - guaranteed. As sure as sure can be. There’s something they say about slow and steady. To win this race he’s willing to slaughter.";
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
        Lore = "Unseen little threat sowing havoc where they go. What harm can they cause? It is not for you to know. ’Til you’re falling apart though you never felt the blow.";
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


public class GreyFieldSlug : Critter
{
    public GreyFieldSlug()
    {
        Name = "Grey Field Slug";
        Lore = "“Disgusting”, they say, “revolting, unnecessary.” Rejection is her origin story, and everyone is her enemy. Revenge on her mind, a chip on her shoulder, she’ll come out at night, she’s ready to murder.";
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
        Lore = "Some like the good life... the slow pace... smiles on a baby’s face - but when backed into a corner and thrust into the ring, good men go to war; spiders learn to sting.";
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
        Lore = "With his legendary name, this beetle casts a shadow. He has strength like no other, and his enemies, they cower.";
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
        Lore = "Sweeter than sweet and forever jolly. He never wished to join the rally. But he must pay for his Queen’s folly - and he’ll fight to the death for family.";
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
        Lore = "Blink twice if you see how this moth seeks to deceive you. Beware, steer clear or death will ensue! Venomous to the touch - every single hair will sting you.";
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
        Lore = "Mesmerising beauty, and no natural animosity - how can a snail get by without ferocity? She breaks no sweat and asserts no effort. She seeks to make peace - she tries - bless her.";
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


public class KerrySlug : Critter
{
    public KerrySlug()
    {
        Name = "Kerry Slug";
        Lore = "Esta é Espanhola e Portuguesa! Quem é que sabia? Esta quer calma e quer uma sabedoria. Que pena, porque hoje, esquece, népia - hoje so há luta e só temos lutaria!";
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
        Lore = "Graceful, demure, she is mindful of her demeanour. You let down your defenses and she gets a little meaner. Beloved by all, but be careful she’s quite the eater.";
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
        Lore = "She’s got a week to live and hell is her next stop. She’ll take a hundred companions before her name is forgot.";
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


public class MammothMoth : Critter
{
    public MammothMoth()
    {
        Name = "Mammoth Moth";
        Lore = "What was that?! Did you spot... a snake upon this moth? Well you better be gone, don’t test your luck on this one.";
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
        Lore = "An icon of of the trade. Can’t forget the face, won’t forget the name. With beautiful wings the colour of flame, it’s the high and mighty Monarch Butterfly.";
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
        Lore = "Not a panda nor an ant - what else is he hiding? Friendless and aloof, forever solitary. A wingless wasp but never sedentary. Destroying lives wherever he goes wandering.";
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
        Lore = "Ten stripes, ten spears, ten reasons to fear. A threat? You bet! Guard your kids and guard your pets. She’ll come to collect.";
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
        Lore = "Strength without a trade, for his speed is never compromised. Staring down this opponent you will know your demise.";
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


public class TarantulaHawkWasp : Critter
{
    public TarantulaHawkWasp()
    {
        Name = "Tarantula Hawk Wasp";
        Lore = "Looking up at a beast that looms so much bigger strikes no terror in this special creature, she smiles to herself and cracks out her stinger, rubbing her legs she whispers, “dinner”.";
        Affinities.Add(CritterAffinity.Bee);
        Moves.Add(new Bonk(){UserGUID = GUID});
        Moves.Add(new HoneyDrink(){UserGUID = GUID});

        MaxHealth = 12;
        CurrentHealth = 12;
        HealthLevelIncrease = new Vector2Int(1, 4);

        MaxSpeed = 8;
        SpeedLevelIncrease = new Vector2Int(1, 5);

        MaxSharpAttack = 15;
        SharpAttackLevelIncrease = new Vector2Int(3, 6);

        MaxSharpDefense = 2;
        SharpDefenseLevelIncrease = new Vector2Int(1, 2);

        MaxBluntAttack = 2;
        BluntAttackLevelIncrease = new Vector2Int(1, 2);

        MaxBluntDefense = 2;
        BluntDefenseLevelIncrease = new Vector2Int(1, 2);
    }
}


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
        Lore = "She hates for the sake of hate. She’s desperate to retaliate.She loathes the world and drips with malice. Her only pleasure, is at her leisure, spewing venom and drilling talons";
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