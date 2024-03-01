using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPos : MonoBehaviour
{
    [SerializeField] private ParticleSystem confetti1;
    [SerializeField] private ParticleSystem confetti2;
    //chest
    [SerializeField] private GameObject chestClose;
    [SerializeField] private GameObject chestOpen;
    private bool isConfetti = false;
    private bool isChest = false;

    void Start()
    {

    }
    public void ActiveConfetti()
    {
        if (isConfetti) return;
        isConfetti = true;
        confetti1.Play();
        confetti2.Play();
    }
    public void OpenChest()
    {
        if (isChest) return;
        isChest = true;
        chestClose.SetActive(false);
        chestOpen.SetActive(true);
        //

        //

    }
}
