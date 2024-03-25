using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Slider slider;
    private CharacterInfo characterStatus;
    private int hp, maxhp;
    private void Start()
    {
       slider =this.transform.GetChild(0).gameObject.GetComponent<Slider>();
        characterStatus= this.transform.GetComponentInParent<CharacterInfo>();
        HPBar();
    }
    public void SetCharacterStatsWithStats(CharacterInfo character)
    {

        this.characterStatus = character;
        HPBar();

    }
    private void Update()
    {
        if( hp != characterStatus.GetLife())
        {
            HPBar();
        }
    }
    void HPBar()
    {
        hp = characterStatus.GetLife();
        maxhp = characterStatus.GetMaxLife();
        slider.minValue = 0;
        slider.maxValue = maxhp;
        slider.value = hp;
    }
   
    public void setCharacterStatus(CharacterInfo character)
    {
        character = this.characterStatus;
    }
}
