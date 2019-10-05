using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer spriteRenderer;

    private bool isRolling;
    private int randomDiceSide = 0;
    private int previousRandomDiceSide = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("Graphics/Dice");
    }

    private void OnMouseDown()
    {
        if (!isRolling)
        {
            StartCoroutine("RollTheDice");
        }
    }

    private IEnumerator RollTheDice()
    {
        isRolling = true;
        var rollSpeed = 0.05f;

        for (int i = 0; i <= 20; i++)
        {
            if(i % 2 == 0)
            {
                rollSpeed += 0.01f;
            }

            yield return Roll(rollSpeed);
        }

        var finalSide = randomDiceSide + 1;
        isRolling = false;
        Debug.Log(finalSide);
    }

    private WaitForSeconds Roll(float rollSpeed)
    {
        while(previousRandomDiceSide == randomDiceSide)
        {
            randomDiceSide = Random.Range(0, 6);
        }   
        spriteRenderer.sprite = diceSides[randomDiceSide];
        previousRandomDiceSide = randomDiceSide;
        return new WaitForSeconds(rollSpeed);
    }
}
