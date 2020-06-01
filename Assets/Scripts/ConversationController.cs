using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class QuestionEvent : UnityEvent<Question> { }
public class ConversationController : MonoBehaviour
{
    public Conversation conversation;
    public QuestionEvent questionEvent;


    public GameObject speakerLeft;
    public GameObject speakerRight;


    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;


    private int activeLineIndex;

    private bool conversationStarted = false;

    public void ChangeConversation(Conversation nextConversation)
    {
        conversationStarted = false;
        conversation = nextConversation;
        AdvanceLine();
    }
    private void Start()
    {
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();
    }
    private void Update()
    {
        if (Input.GetKeyDown("space"))
            AdvanceLine();

        else if (Input.GetKeyDown("x"))
            EndConversation();
    }

    private void EndConversation()
    {
        conversation = null;
        conversationStarted = false;
        speakerUILeft.Hide();
        speakerUIRight.Hide();
    }
    private void Initialize()
    {
        conversationStarted = true;
        activeLineIndex = 0;
        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
    }
    private void AdvanceLine()
    {
        if (conversation == null) return;
        if (!conversationStarted) Initialize();

        if (activeLineIndex < conversation.lines.Length)
            DisplayLine();
        else
            AdvanceConversation();
    }
    private void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;
        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
        activeLineIndex += 1;
    }
    private void AdvanceConversation()
    {
        // These are really three types of dialog tree node
        // and should be three different objects with a standard interface
        if (conversation.question != null)
            questionEvent.Invoke(conversation.question);

        else if (conversation.nextConversation != null)
            ChangeConversation(conversation.nextConversation);

        else
            EndConversation();
    }

    private void SetDialog(
        SpeakerUI activeSpeakerUI,
        SpeakerUI inactiveSpeakerUI,
        string text)
    {
        activeSpeakerUI.Dialog = text;
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

    }



  

}