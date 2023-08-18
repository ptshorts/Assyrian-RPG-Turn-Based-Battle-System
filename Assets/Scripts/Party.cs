using UnityEngine;

/// <summary>
/// This class holds all of the characters in a party, along with their 
/// associated UI components, such as the HP and MP bars, and the character 
/// selection arrows.
/// </summary>
public class Party : MonoBehaviour
{
    /// <summary>
    /// The characters in a party. This is hidden from inspector, because it's
    /// meant to be set programmatically and not from the Unity editor (by the
    /// BattleManager).
    /// </summary>
    [HideInInspector]
    public Character[] characters;

    /// <summary>
    /// The physical spots in which the characters will be placed.
    /// </summary>
    public GameObject[] slots;

    public SpriteRenderer[] arrows;

    public StatusBar[] hpBars;
    public StatusBar[] mpBars;

    private bool isEnemy;

    public void Init(Character[] characters, bool isEnemy = true, StatusBar[] hpBars = null, StatusBar[] mpBars = null)
    {
        this.characters = characters;
        this.isEnemy = isEnemy;
        this.hpBars = hpBars;
        this.mpBars = mpBars;
        if (isEnemy || (characters.Length == hpBars.Length && characters.Length == mpBars.Length))
        {
            Setup();
            UpdateArrows();
        }
    }

    private void Setup()
    {
        int count = 0;
        foreach (var character in characters)
        {
            if (count < slots.Length) {
                character.transform.parent = slots[count].transform;
                character.transform.localPosition = new Vector3(0f, 0f, 0f);
                character.memberId = count;
                if (isEnemy)
                    character.Init(RemoveFromParty);
                else
                    character.Init(RemoveFromParty, isEnemy, hpBars[count], mpBars[count]);
                count++;
            }
        }
    }

    /// <summary>
    /// Update the list of arrows that this party contains, so that only the 
    /// ones from the remaining characters (in the party) will show. The list
    /// is made by getting the reference to each character's arrow from the
    /// character object, itself.
    /// </summary>
    public void UpdateArrows()
    {
        if (characters == null)
        {
            arrows = null;
        }
        else
        {
            arrows = new SpriteRenderer[characters.Length];
            int count = 0;
            foreach (var member in characters)
            {
                if (count < arrows.Length)
                {
                    arrows[count] = member.GetComponent<Attackable>().selectionArrow;
                    count++;
                }
            }
        }
    }

    /// <summary>
    /// Remove the character from the party so that it can't be selected 
    /// anymore.
    /// </summary>
    /// <param name="index"></param>
    private void RemoveFromParty(int index)
    {
        // if characters.Length == 1, then this is the last one in the party,
        // so it hasn't been removed yet
        if (characters.Length > 1)
        {
            int i = 0;
            int j = 0;
            Character[] newParty = new Character[characters.Length - 1];
            foreach (var character in characters)
            {
                if (i != index)
                {
                    newParty[j] = characters[i];
                    character.memberId = j;
                    j++;
                }
                i++;
            }
            characters = newParty;
        }
        else
        {
            characters = null;
        }
        UpdateArrows();
    }
}
