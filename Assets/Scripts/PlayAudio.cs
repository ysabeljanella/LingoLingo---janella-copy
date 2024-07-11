using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip consVowsClip;
   public void PlayAudioMethod(AudioClip clips)
    {
        audioSource.clip = clips;
        audioSource.Play();
    }

    public void PlayConsVows()
    {
        audioSource.clip = consVowsClip;
        audioSource.Play();
    }

    public void CloseConsVow()
    {
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
        DialogueLua.SetVariable("FlashcardPlay", true);
        Destroy(transform.parent.gameObject);
    }

    public void StopAllConversations()
    {
        DialogueLua.SetVariable("FlashcardPlay", false);
        PixelCrushers.DialogueSystem.Sequencer.Message("CloseGame");
        //for (int i = DialogueManager.instance.activeConversations.Count - 1; i >= 0; i--)
        //{
        //    DialogueManager.instance.activeConversations[i].conversationController.Close();
        //}
        Destroy(transform.parent.gameObject);
    }
}
