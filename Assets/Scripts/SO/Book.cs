using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Book")]
public class Book : Item, IUsable {

    public BookType bookType;
    public int power;

    public void Consume() {
        switch(bookType) {

        }
    }
}

public enum BookType {
    Strength, Agility, Dexterity, Intelligence, Faith, Wisdom, Vitality, Charisma, Flame, Fire, Aqua, Sea,
    Wind, Air, Druidry, Earth, Shock, Thunder, Sheen, Light, Dark, Void, Cunning, Venom, Brutality, Blood, Spirit,
    Souls, Grounding, Nature, Fireproofing, Warmth, Snake, Vigor, Luck, Skill, Thrust, Toughness, Bash, Elasticity,
    Slash, Muscle, Pyromancy, Hydromancy, Aerothurgy, Geomancy, Electromancy, Photomancy, Necromancy
}
