using UnityEngine;

public class BattleResultsBox : MonoBehaviour
{
    public Sprite gold;
    public Sprite experience;

    public SpriteRenderer title;
    public TextMesh amountText;

    public void ShowGold(int amount)
    {
        title.sprite = gold;
        amountText.text = amount.ToString();
    }

    public void ShowExperience(int amount)
    {
        title.sprite = experience;
        amountText.text = amount.ToString();
    }
}
