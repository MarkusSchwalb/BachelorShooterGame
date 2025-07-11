using System;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    public event Action FadeOutComplete;
    public event Action FadeInComplete;
    Animator animator;
    int black = Animator.StringToHash("Black");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(black, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    internal void StartFade()
    {
        animator.SetBool(black, true);
    }

    public void FadeOut()
    {
        Debug.Log("FadeOutInvoke");
        FadeOutComplete?.Invoke();
    }
    public void FadeIn()
    {
        Debug.Log("FadeInInvoke");
        FadeInComplete?.Invoke();
    }

    
}
