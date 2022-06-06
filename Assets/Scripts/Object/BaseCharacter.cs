using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    struct info
    {
        public string name;
        public System.Action action;
        public int Hp;
        public int MaxHp;
    }

    public GameObject body;
    public Rigidbody2D rigidBody2D;

    protected virtual void init() => rigidBody2D = (body = gameObject).GetComponent<Rigidbody2D>();

    List<System.Action<Collision2D>> onColEnterAction = new List<System.Action<Collision2D>>();
    public void setEnterAction(System.Action<Collision2D> action) => onColEnterAction.Add(action);
    public void clearEnterAction() => onColEnterAction.Clear();

    List<System.Action<Collision2D>> onColExitAction = new List<System.Action<Collision2D>>();
    public void setExitAction(System.Action<Collision2D> action) => onColExitAction.Add(action);
    public void clearExitAction() => onColExitAction.Clear();

    void OnCollisionEnter2D(Collision2D other) 
    {
        foreach (System.Action<Collision2D> action in onColEnterAction) 
            action(other);
    }

    void OnCollisionExit2D(Collision2D other) {
        foreach (System.Action<Collision2D> action in onColExitAction) 
            action(other);
    }

    List<System.Action<Collider2D>> onTriggerEnterAction = new List<System.Action<Collider2D>>();
    public void setEnterAction(System.Action<Collider2D> action) => onTriggerEnterAction.Add(action);
    public void clearTriggerEnterAction() => onTriggerEnterAction.Clear();

    List<System.Action<Collider2D>> onTriggerExitAction = new List<System.Action<Collider2D>>();
    public void setExitAction(System.Action<Collider2D> action) => onTriggerExitAction.Add(action);
    public void clearTriggerExitAction() => onTriggerExitAction.Clear();



    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (System.Action<Collider2D> action in onTriggerEnterAction)
            action(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        foreach (System.Action<Collider2D> action in onTriggerExitAction)
            action(other);
    }
}
