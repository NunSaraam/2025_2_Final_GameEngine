using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{
    //블록
    Dirt,
    Grass,
    Water,
    Cloud,

    //장비
    Sword,
    Axe,

    //건축물
    Furnace,
    Generator,

    //자원
    Wood,
    Stone,
    Iron,
    Crystal,
    Mushroom,
}

public enum ItemCategory
{
    Block,
    Tool,
    Structure,
    Resource
}