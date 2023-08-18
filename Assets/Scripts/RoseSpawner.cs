using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the roses on the field. Given the limitations of the screen real 
/// estate and no demand for scalability, most limits are constant. There are
/// five rows, so each row gets a "last x" counter to progress the position of
/// the roses along the x-axis.
/// </summary>
public class RoseSpawner : MonoBehaviour, AnimSceneEffect
{
    public GameObject rose;

    // number of fields predefined for efficiency; scalability deemed unnecessary
    public RoseSpawnField field1;
    public RoseSpawnField field2;
    public RoseSpawnField field3;
    public RoseSpawnField field4;
    public RoseSpawnField field5;

    private RoseSpawnField currentField;

    private GameObject currentRose;
    private List<GameObject> roses;

    // this is a counter that goes between 1-to-5, to traverse thru all fields
    private int currentFieldRow = 0;

    [Tooltip("How many roses to spawn during each iteration.")]
    public int groupSize = 12;

    [Tooltip("How many times to spawn a group of roses.")]
    public int iterations = 20;

    [Tooltip("The time to wait before spawning the next group (time between iterations).")]
    public float iterationPace = 0.1f;

    // how many times has groups of roses been spawned already
    private int count = 0;


    public void SpawnRoses()
    {
        count = 0;
        roses = new List<GameObject>();
        StartCoroutine(ChainSpawnRoses());
    }


    public void RemoveRoses()
    {
        StartCoroutine(ChainRemoveRoses());
    }

    private GameObject GetNewRose()
    {
        return Instantiate(rose);
    }

    /// <summary>
    /// Once a rose has been instantiated, it needs to be moved to a proper
    /// position, which is handled by this method.
    /// </summary>
    /// <param name="rose">A newly instantiated rose GameObject.</param>
    /// <param name="field">The field (or row) to which the rose is to be added.</param>
    private void MoveRose(Transform rose, int field)
    {
        currentField = GetCurrentField(field);
        rose.localPosition = new Vector2(currentField.GetNextX(), currentField.GetY());
        rose.GetComponent<SpriteRenderer>().sortingOrder = currentField.spriteOrder;
    }

    private RoseSpawnField GetCurrentField(int currentFieldRow)
    {
        switch (currentFieldRow)
        {
            case 1:
                return field1;
            case 2:
                return field2;
            case 3:
                return field3;
            case 4:
                return field4;
            case 5:
                return field5;
            default:
                return null;
        }
    }

    private void UpdateCurrentFieldRow()
    {
        if (currentFieldRow < 5)
            currentFieldRow++;
        else
            currentFieldRow = 1;
    }

    /// <summary>
    /// Spawn a group of roses and wait, then Spawn again as needed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChainSpawnRoses()
    {
        count++;
        for (int i = 0; i < groupSize; i++)
        {
            currentRose = GetNewRose();
            UpdateCurrentFieldRow();
            MoveRose(currentRose.transform, currentFieldRow);
            roses.Add(currentRose);
        }
        yield return new WaitForSeconds(iterationPace);
        if (count < iterations)
            StartCoroutine(ChainSpawnRoses());
    }

    /// <summary>
    /// Remove a group of roses and wait, then remove again as needed.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChainRemoveRoses()
    {
        count--;
        for (int i = 0; i < groupSize; i++)
        {
            currentRose = roses[roses.Count - 1];
            roses.Remove(currentRose);
            Destroy(currentRose);
        }
        yield return new WaitForSeconds(iterationPace);
        if (count > 0)
            StartCoroutine(ChainRemoveRoses());
    }


    /// <summary>
    /// Calls a method that populates roses on the scene. This is made to be 
    /// called by an animation event, as needed.
    /// </summary>
    /// <returns>Callback for spawn roses.</returns>
    public Action GetSceneEffect()
    {
        return SpawnRoses;
    }

    /// <summary>
    /// Invokes an effect that reverses the scene effect called previously. 
    /// This is made to be called by an animation event, as needed.
    /// </summary>
    /// <returns>Callback for removing the roses.</returns>
    public Action GetReverseSceneEffect()
    {
        return RemoveRoses;
    }
}
