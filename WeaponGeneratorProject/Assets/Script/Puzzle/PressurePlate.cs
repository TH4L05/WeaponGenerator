using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public Animation anim;
    public UnityEvent OnTiggerEnterChangeReciverObject;
    public UnityEvent OnTiggerExitChangeReciverObject;
    private bool pressed;


    private void OnTriggerStay(Collider collider)
    {
        if (pressed) return;
        pressed = true;
        Debug.Log("ButtonEntered");
        anim.Play("buttonPressed");
        OnTiggerEnterChangeReciverObject?.Invoke();
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!pressed) return;
        pressed = false;
        Debug.Log("ButtonReleased");
        anim.Play("buttonRealeased");
        OnTiggerExitChangeReciverObject?.Invoke();
    } 
}
