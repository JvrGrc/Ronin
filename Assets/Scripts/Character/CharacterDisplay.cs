using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharacterDisplay : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TextAsset characterData;
    [SerializeField] GameObject characterPrefab;

    public List<CharacterInfo> CreateCharacters(Dictionary<Vector2Int, OverlayTile> overlay)
    {
        List<GameObject> characters = LoadData(this.characterData.text);
        return PlaceCharactersInTilemap(characters, overlay);
    }

    public List<GameObject> LoadData(string charactersData)
    {
        List<GameObject> characters = new List<GameObject>();

        StringReader reader = new StringReader(charactersData);

        while (true)
        {
            string line = reader.ReadLine();

            if (line == null) break;

            if (line.StartsWith("//")) { line = reader.ReadLine(); }        //  If the string starts with '//' it means it´s a comment

            line = line.Trim();

            if (line.Length == 0) continue;

            string[] values = line.Split(',');
            if (values.Length == 12)
            {
                GameObject o = Instantiate(characterPrefab);
                CharacterInfo p = o.GetComponent<CharacterInfo>();
                int x = int.Parse(values[2]);
                int y = -int.Parse(values[3]);

                p.SetName(values[0]);
                p.SetMaxLife(int.Parse(values[1]));
                p.SetInitialPos(new Vector2Int(x, y));
                p.SetEnemy(bool.Parse(values[4]));
                p.SetDamage(int.Parse(values[5]));
                p.SetDefense(int.Parse(values[6]));
                p.SetSprite(int.Parse(values[7]));
                p.SetMovement(int.Parse(values[8]));
                p.SetSpeed(int.Parse(values[9]));
                p.SetDistance(int.Parse(values[10]));
                p.SetLife(p.GetMaxLife());
                if (int.Parse(values[11]) != 0)
                {
                    p.SetWeaponObj(GetWeapon(int.Parse(values[11])));
                }

                characters.Add(o);
            }
            else
            {
                Debug.LogWarning($"Ignorando línea incorrecta: {line}");
            }
        }

        return characters;
    }

    public List<CharacterInfo> PlaceCharactersInTilemap(List<GameObject> personajes, Dictionary<Vector2Int, OverlayTile> overlay)
    {
        CharacterContainer container = FindAnyObjectByType<CharacterContainer>();
        List<CharacterInfo> list = new List<CharacterInfo>();
        foreach (GameObject objt in personajes)
        {
            CharacterInfo p = objt.GetComponent<CharacterInfo>();
            p.SetCharacterStandingOnTile(overlay[p.GetInitialPos()]);
            objt.transform.position = overlay[p.GetInitialPos()].transform.position;

            SpriteRenderer spriteRenderer = objt.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GetSprite(p.GetSprite());
            if (p.IsEnemy() && p.GetSprite() != 4) { objt.AddComponent<EnemyAI>(); }
            spriteRenderer.sortingOrder = 1;

            if (container != null) { objt.transform.parent = container.transform; }
            list.Add(p);
        }

        return list;
    }

    private Sprite GetSprite(int sprite)
    {
        if (sprite == 4) { return Resources.Load<Sprite>("Sprites/dojoEnemy"); }
        if (sprite == 5) { return Resources.Load<Sprite>("Sprites/samurai2"); }
        if (sprite == 6) { return Resources.Load<Sprite>("Sprites/samurai1"); }
        if (sprite == 7) { return Resources.Load<Sprite>("Sprites/Ninja2"); }
        if (sprite == 8) { return Resources.Load<Sprite>("Sprites/samurai1B"); }
        if (sprite == 9) { return Resources.Load<Sprite>("Sprites/Ninja2B"); }
        if (sprite == 10) { return Resources.Load<Sprite>("Sprites/Samurai2B"); }
        if (sprite == 11) { return Resources.Load<Sprite>("Sprites/Takeshi"); }
        Sprite[] characters = Resources.LoadAll<Sprite>("Sprites/Characters");
        return characters[sprite];
    }

    private GameObject GetWeapon(int weapon)
    {
        if (weapon == 1) { return Resources.Load<GameObject>("Prefabs/Weapon/Katana"); }
        if (weapon == 2) { return Resources.Load<GameObject>("Prefabs/Weapon/Lanza"); }
        if (weapon == 3) { return Resources.Load<GameObject>("Prefabs/Weapon/Hacha"); }
        if (weapon == 4) { return Resources.Load<GameObject>("Prefabs/Weapon/Arco"); }
        else { return null; }
    }
}
