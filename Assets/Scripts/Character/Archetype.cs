using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Archetype
{
    [InspectorName("Player/Warrior")]
    Warrior,
    [InspectorName("Player/Archer")]
    Archer,
    [InspectorName("Player/Sorcerer")]
    Sorcerer,
    [InspectorName("Enemy/Bandit")]
    Bandit,
}
