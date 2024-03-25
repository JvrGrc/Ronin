using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class CharacterInfo : MonoBehaviour
{
    [SerializeField] OverlayTile standingOnTile;
    [SerializeField] string _name;
    [SerializeField] Vector2Int initialPos;
    [SerializeField] int movement;
    [SerializeField] bool enemy;
    [SerializeField] int damage;
    [SerializeField] int defense;
    [SerializeField] int sprite;
    [SerializeField] int maxLife;
    [SerializeField] int distance;
    [SerializeField] int speed;
    [SerializeField] CharacterInfo enemigo;
    [SerializeField] GameObject weaponObj;
    private GameObject hpbar;
    private GameObject inventory;
    private Weapon weapon;
    private bool selected = false;
    private int life;
    private bool weaponcheck = false;


    private void Start()
    {
        hpbar = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/HealthBar2"));
        hpbar.transform.SetParent(transform);


        inventory = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Inventory"));

        inventory.transform.SetParent(transform);
        this.gameObject.GetComponent<Inventory>();
        hpbar.transform.localPosition = new Vector2(-0.1f, 1f);
        if (this.weaponObj != null)
        {
            weapon = weaponObj.GetComponent<Weapon>();

        }
        if (this.sprite == 5)
        {
            Animator anim = this.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("Animations/Lancer/Character2") as RuntimeAnimatorController;
        }
        else if (this.sprite == 6)
        {
            Animator anim = this.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("Animations/Katana/Character") as RuntimeAnimatorController;
        }
        else if (this.sprite == 7)
        {
            Animator anim = this.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("Animations/Ninja/Character3") as RuntimeAnimatorController;
        }
        else if (this.sprite == 8)
        {
            Animator anim = this.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("Animations/KatanaB/Character4") as RuntimeAnimatorController;
        }
        else if (this.sprite == 9)
        {
            Animator anim = this.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("Animations/NinjaB/Character5") as RuntimeAnimatorController;
        }
        else if (this.sprite == 10)
        {
            Animator anim = this.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("Animations/LancerB/Character6") as RuntimeAnimatorController;
        }



    }
    private void Update()
    {
        if (selected)
        {
            hpbar.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            hpbar.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        if (this.weaponObj != null && weaponcheck == false)
        {
            this.damage = damage + this.weaponObj.GetComponent<Weapon>().GetDamage();
            this.speed = speed + this.weaponObj.GetComponent<Weapon>().GetSpeed();
            this.defense = defense + this.weaponObj.GetComponent<Weapon>().GetDefense();
            this.distance = this.weaponObj.GetComponent<Weapon>().GetDistance();
            weaponcheck = true;

        }
    }

    public OverlayTile GetCharacterStandingOnTile()
    {
        return standingOnTile;
    }

    public bool GetSelected()
    {
        return selected;
    }

    public void ChangeSelected(bool newSelected)
    {
        this.selected = newSelected;
    }

    public void SetCharacterStandingOnTile(OverlayTile newStandingOnTile)
    {
        this.standingOnTile = newStandingOnTile;
    }

    public void SetName(string name) { this._name = name; }
    public string GetName() { return this._name; }

    public Vector2Int GetInitialPos() { return initialPos; }
    public void SetInitialPos(Vector2Int pos) { this.initialPos = pos; }

    public int GetDefense() { return defense; }
    public void SetDefense(int defensa) { this.defense = defensa; }

    public int GetDamage() { return damage; }
    public void SetDamage(int ataque) { this.damage = ataque; }

    public bool IsEnemy() { return enemy; }
    public void SetEnemy(bool ene) { this.enemy = ene; }

    public int GetSprite() { return sprite; }
    public void SetSprite(int sprite) { this.sprite = sprite; }

    public void SetMovement(int movement) { this.movement = movement; }
    public int GetMovement() { return movement; }

    public int GetLife() { return life; }
    public void SetLife(int life) { this.life = life; }
    public int GetMaxLife() { return maxLife; }
    public void SetMaxLife(int maxLife) { this.maxLife = maxLife; }


    public int GetDistance() { return distance; }
    public void SetDistance(int distance) { this.distance = distance; }

    public int GetSpeed() { return speed; }
    public void SetSpeed(int speed) { this.speed = speed; }

    public CharacterInfo GetEnemigo() { return enemigo; }
    public void SetEnemigo(CharacterInfo enemi) { this.enemigo = enemi; }

    public void Attack()
    {
        if (weaponObj == null) AtackWithOutWeapon();
        else if (enemigo != null)
        {
            int priority;
            int realDefense;
            if (enemigo.GetWeaponObj() != null)
            {
                //type 1 -wins-> type2 -wins-> type3 -wins-> type4 -wins-> type1
                if (weapon.GetTypes() > enemigo.GetWeapon().GetTypes()) { priority = -1; }        //If the atack type is bigger, the priority is negative
                else if (weapon.GetTypes() < enemigo.GetWeapon().GetTypes()) { priority = 1; }    //else is positive
                else priority = 0;
                realDefense = enemigo.defense + enemigo.GetWeapon().GetDefense();
            }
            else
            {
                priority = 1;
                realDefense = enemigo.defense;
            }

            int realDamage;
            //If priority is positive the damage is reduced, if is negative increases
            if (priority != 0) { realDamage = (int)System.Math.Round(damage + (weapon.GetDamage() * (priority * 0.2))); }
            else { realDamage = damage + weapon.GetDamage(); }

            int hit = realDamage - realDefense;     //Calculate the damage that would receive the enemy
            if (hit < 0) { hit = 5; }               //Always hits at least 5 points of life

            if (hit >= enemigo.GetLife())             //If the hit is bigger than enemy's life, enemy dies
            {
                enemigo.SetLife(0);
                //DestroyImmediate(enemigo);
            }
            else                                    //Else enemy's life is reduced by hit value
            {
                enemigo.SetLife(enemigo.GetLife() - hit);
            }
        }
        else
        {
            Debug.Log("No hay enemigos contra los que luchar.");
        }
    }

    public void AtackWithOutWeapon()
    {
        if (enemigo != null)
        {
            int lifeEnemy = enemigo.GetLife();
            int realDamage = GetRealDamage();
            if (lifeEnemy - realDamage <= 0)
            {
                enemigo.SetLife(0);
            }
            else
            {
                enemigo.SetLife(lifeEnemy - realDamage);
            }
        }
        else
        {
            Debug.Log("No hay enemigos contra los que luchar.");
        }
    }
    public void Attackbuff()
    {
        Debug.Log("Ataque antiguo " + damage);
        this.damage += 10;
        Debug.Log("Ataque nuevo " + damage);
    }
    public void Heal()
    {
        if (life < maxLife)
        {
            if ((life + 30) < maxLife)
            {
                SetLife(life + 30);

            }
            else
            {
                SetLife(maxLife);

            }
        }
        else
        {
            Debug.Log("La salud ya est? al m?ximo.");
        }
    }
    private int GetRealDamage()
    {
        int x = UnityEngine.Random.Range(enemigo.GetDefense() - enemigo.GetDefense() * 25 / 100, enemigo.GetDefense());
        Debug.Log("Defensa: " + x);
        if (x >= damage) { return 5; }  //The minimum damage is always gonna be 5, even if the enemy defense is higher than the opponent's atack
        else { return damage - x; }     //The final damage is gonna be de damage less the enemy's defense. Ej: Damage: 30, Defense: 20 --> FinalDamage: 10
    }

    public GameObject GetWeaponObj() { return weaponObj; }
    public void SetWeaponObj(GameObject w) { this.weaponObj = w; this.weapon = w.GetComponent<Weapon>(); }

    public Weapon GetWeapon() { return weapon; }
}
