using UnityEngine;

public enum WeaponType
{
    knife,
    magic_single,
    magic_spread,
    magic_riffle,
}

public class weapon : MonoBehaviour
{

    public WeaponType weaponType;
}
