using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfetiGate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem confetti1;
    [SerializeField] private ParticleSystem confetti2;
    private bool isActive = false;
    void Start()
    {
        
    }
    public void ActiveConfetti()
    {
        if (isActive) return;
        isActive = true;
        confetti1.Play();
        confetti2.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
