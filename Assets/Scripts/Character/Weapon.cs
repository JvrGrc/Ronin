using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] string names;
    [SerializeField] int damage;
    [SerializeField] int distance;
    [SerializeField] int speed;
    [SerializeField] int defense;
    [SerializeField] [Range(1,3)] int type;

    public int GetDamage(){return damage;}
    public int GetDistance() { return distance;}
    public int GetDefense() {  return defense;}
    public int GetSpeed() { return speed;}
    public string GetNames() { return names;}
    public int GetTypes() {  return type;}
}
