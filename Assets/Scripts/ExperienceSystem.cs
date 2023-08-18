using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Experience System
 * 
 * All characters get their stats from this system, based on their experience
 * points and archetype.
 * 
 * level = experience points * archetype rate for leveling up
 * HP = level * archetype rate for HP
 * MP = level * archetype rate for MP
 */
public class ExperienceSystem
{
    // the HP and MP rates are also the initial HP and MP amounts
    // the XP rate is also the amount of XP required to advance to level 2
    private const int WARRIOR_HP_RATE = 110;
    private const int WARRIOR_MP_RATE = 0;
    private const int WARRIOR_XP_RATE = 1200;
    private const int ARCHER_HP_RATE = 90;
    private const int ARCHER_MP_RATE = 0;
    private const int ARCHER_XP_RATE = 1400;
    private const int SORCERER_HP_RATE = 76;
    private const int SORCERER_MP_RATE = 30;
    private const int SORCERER_XP_RATE = 1500;

    private const float HP_GROWTH_RATE = 0.17f;
    private const float LEVEL_GROWTH_RATE = 0.17f;

    public int GetNextLevel(int xp, Archetype type)
    {
        float levelUpRate = GetLevelUpRate(type);
        int baseRate = (int)(xp / levelUpRate);
        // prevent linear growth rate
        int growthFactor = baseRate * (int)(levelUpRate * LEVEL_GROWTH_RATE);
        return (int)((baseRate + 1) * levelUpRate) + growthFactor;
    }

    public int GetHp(int xp, Archetype type)
    {
        float hpRate = GetHpRate(type);
        int baseRate = (int)(xp / hpRate);
        // prevent linear growth rate
        int growthFactor = baseRate * (int)(hpRate * HP_GROWTH_RATE);
        return baseRate + growthFactor;
    }

    public int GetMp(int xp, Archetype type)
    {
        float mpRate = GetMpRate(type);
        int baseRate = (int)(xp / mpRate);
        // prevent linear growth rate
        int growthFactor = baseRate * (int)(mpRate * HP_GROWTH_RATE);
        return baseRate + growthFactor;
    }

    private float GetLevelUpRate(Archetype type)
    {
        switch (type)
        {
            case Archetype.Warrior:
                return WARRIOR_XP_RATE;
            case Archetype.Archer:
                return ARCHER_XP_RATE;
            case Archetype.Sorcerer:
                return SORCERER_XP_RATE;
        }
        return 0;
    }

    private float GetHpRate(Archetype type)
    {
        switch (type)
        {
            case Archetype.Warrior:
                return WARRIOR_HP_RATE;
            case Archetype.Archer:
                return ARCHER_HP_RATE;
            case Archetype.Sorcerer:
                return SORCERER_HP_RATE;
        }
        return 0;
    }

    private float GetMpRate(Archetype type)
    {
        switch (type)
        {
            case Archetype.Warrior:
                return WARRIOR_MP_RATE;
            case Archetype.Archer:
                return ARCHER_MP_RATE;
            case Archetype.Sorcerer:
                return SORCERER_MP_RATE;
        }
        return 0;
    }
}
