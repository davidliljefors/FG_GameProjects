using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTurn : ITurn
{
    public Action OnStateFinished { get ; set ; }
    public static Action OnDialogueFinished { get; private set; } = delegate { };

    private DialogueManager m_DialogueManager = null;

    public DialogueTurn(DialogueManager dm)
    {
        m_DialogueManager = dm;
    }

    public void Enter()
    {
        m_DialogueManager.StartDialogue();
        OnDialogueFinished = delegate { OnStateFinished.Invoke(); };
    }




    public void Exit()
    {
        Debug.Log("Dialogue turn :: exit");
    }

    public void Tick()
    {
        
    }
}
