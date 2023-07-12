using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header ("References")]
    public Slider EnergySlider;
    public Slider GravitySlider;

    public static bool IsGravityReady;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        FindObjectOfType<GravityEventManager>().onGravityInvert.AddListener(HandleGravityInvert);
    }

    private void Start()
    {
        EnergySlider.maxValue = playerController.Energy;

        EnergySlider.direction = Slider.Direction.LeftToRight;
    }

    private void Update()
    {
        EnergySlider.value = playerController.Energy;

        if (!IsGravityReady && GravitySlider.value <= GravitySlider.maxValue)
        {
            GravitySlider.value += Time.deltaTime * 10;
        }
        
        IsGravityReady = GravitySlider.value >= GravitySlider.maxValue;
    }

    void HandleGravityInvert(bool isInverted)
    {
        GravitySlider.value = 0;
    }
}
