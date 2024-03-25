using UnityEngine;

public class CharacterContainer : MonoBehaviour
{
    public CharacterInfo[] GetAllCharacters()
    {
        return GetComponentsInChildren<CharacterInfo>();
    }
}
