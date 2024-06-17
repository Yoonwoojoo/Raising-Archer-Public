using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Raising-Archer/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public Sprite weaponSprite;
    public string weaponName;
    public int weaponAttackDamage;
    public float weaponAttackDelay;
    public int weaponMoveSpeed;
    public float gachaRate;
}