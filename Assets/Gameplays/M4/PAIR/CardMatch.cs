using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMatch : MonoBehaviour
{
    public int cardIndex;
    public Image Myrenderer;
    public Sprite[] image;
    private bool isFlipped = false; // Track if the card is flipped
    private static CardMatch firstFlippedCard; // Static reference to the first flipped card
    private static bool isChecking = false; // Flag to prevent checking during animation
   
    private void Start()
    {
        // Set the default image (back image)
        Myrenderer.sprite = image[1];
    }

    // Function to handle card click
    public void OnCardClick()
    {
        // Check if another card is already being checked or if this card is already flipped
        if (isChecking || isFlipped)
            return;

        // Flip the card
        FlipCard();

        // Check if this is the first flipped card
        if (firstFlippedCard == null)
        {
            firstFlippedCard = this; // Set this as the first flipped card
        }
        else
        {
            // Another card has been flipped, compare indices
            if (firstFlippedCard.cardIndex == this.cardIndex)
            {
                
                // Indices match, keep both cards flipped
                firstFlippedCard = null; // Reset the first flipped card
                PairManager.Instance.currentScore++;
                PairManager.Instance.coinReward += 50;
                if (PairManager.Instance.currentScore >= PairManager.Instance.maxScore)
                {
                    PairManager.Instance.FinalizeQuiz();
                }
            }
            else
            {
                // Indices don't match, flip both cards back
                isChecking = true;
                StartCoroutine(FlipBack());
            }
        }
        SoundHandler.instance.Click();
    }

    // Coroutine to flip the card back after a short delay
    IEnumerator FlipBack()
    {
        yield return new WaitForSeconds(.5f); // Adjust the delay time if needed

        // Flip both cards back
        firstFlippedCard.FlipCard();
        FlipCard();

        // Reset the first flipped card reference
        firstFlippedCard = null;

        isChecking = false; // Allow checking again
    }

    // Function to flip the card
    private void FlipCard()
    {
        isFlipped = !isFlipped; // Toggle flipped state
        Myrenderer.sprite = isFlipped ? image[0] : image[1]; // Set sprite based on flipped state
    }

}
