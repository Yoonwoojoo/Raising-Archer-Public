using UnityEngine;

[CreateAssetMenu(fileName = "WeaponList", menuName = "Raising-Archer/WeaponList", order = 2)]
public class WeaponList : ScriptableObject
{
    public WeaponData[] weapons;
}