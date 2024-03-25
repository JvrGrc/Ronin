using UnityEngine;
using UnityEngine.UI;

public class StatusMenu : MonoBehaviour
{
    [SerializeField] GameObject icon;
    [SerializeField] Text HP;
    [SerializeField] Text ATTACK;
    [SerializeField] Text DEFENSE;
    [SerializeField] Text MOVEMENT;
    [SerializeField] Text SPEED;
    [SerializeField] Text NAME;
    [SerializeField] Text ATTACKRANGE;
    [SerializeField] Slider slider;
    [SerializeField] CharacterInfo characterStatus;
    
    private int hp,maxhp, attack, defense, movement, speed,arange;

    public CharacterInfo GetCharacter() { return characterStatus; }

    public void testButton(CharacterInfo character)
    {
        Canvas menuParent = this.GetComponentInParent<Canvas>();
        menuParent.enabled = true;
        SetCharacterStatsWithStats( character);
    }
    public void SetCharacterStatsWithStats(CharacterInfo character)
    {
        
        this.characterStatus = character;
        setStats();

    }

    void setStats()
    {
        //  Life and maxLife
        hp = characterStatus.GetLife();
        maxhp = characterStatus.GetMaxLife();
        slider.minValue = 0;
        slider.maxValue = maxhp;
        slider.value = hp;
        HP.text = hp + "/" + maxhp;
        //  Damage
        attack =characterStatus.GetDamage();
        ATTACK.text = ""+attack;

        //  Defense
        defense = characterStatus.GetDefense();
        DEFENSE.text = "" + defense;
        //  Movement Range
        movement = characterStatus.GetMovement();
        MOVEMENT.text = "" + movement;
        //  Speed
        speed = characterStatus.GetSpeed();
        SPEED.text = "" + speed;

        //  Attack Range
        arange = characterStatus.GetDistance();
        ATTACKRANGE.text = "" + arange;
        //  Sprite
        icon.GetComponent<Image>().sprite = GetSprite(characterStatus.GetSprite());

        //  Name
        name = characterStatus.GetName();
        NAME.text = "" + name;
    }

    public void setCharacterStats(CharacterInfo character)
    {

        this.characterStatus = character;
        
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
        Sprite[] characters = Resources.LoadAll<Sprite>("Sprites/Characters");
        return characters[sprite];
    }
}
