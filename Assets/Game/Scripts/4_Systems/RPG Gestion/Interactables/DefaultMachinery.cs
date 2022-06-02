using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DefaultMachinery : MonoBehaviour
{
    public Feedbacks feedbacks;

    public void Execute()
    {
        feedbacks.OnBeginExecute.Invoke();
        Character.TrackInput(false);
        OnBeginExecute();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Character.TrackInput(true);
            feedbacks.OnBeginExecute.Invoke();
            OnEndExecute();
        }
    }

    public void EnterSelection() => feedbacks.OnEnter.Invoke();
    public void ExitSelection() => feedbacks.OnExit.Invoke();
    protected abstract void OnBeginExecute();
    protected abstract void OnEndExecute();

    [System.Serializable]
    public class Feedbacks
    {
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        public UnityEvent OnBeginExecute;
        public UnityEvent OnEndExecute;
    }

}
