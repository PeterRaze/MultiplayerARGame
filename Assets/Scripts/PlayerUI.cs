using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private Image hpImg;

    private float hp;

    public void SetTotalHP(float hp)
    {
        this.hp = hp;
    }

    public void ReduceHP(float damage)
    {
        var reduceValue = Mathf.InverseLerp(0, hp, damage);
        hpImg.fillAmount -= reduceValue;
    }
}
