using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GambaLogic : MonoBehaviour
{
    [Header("Ingridients")]
    [SerializeField] private float ButterQuality;
    [SerializeField] private float SugarQuality;
    [SerializeField] private float FlourQuality;

    [Header("Machines")]
    [SerializeField] private float OvenTemperature;
    [SerializeField] private float MixingSpeed;
    [SerializeField] private float RollerSpeed;

    [Header("Time, Quantity and Cost")]
    [SerializeField] private float BakingTime;
    [SerializeField] private float BakingQuantity;
    [SerializeField] private float BakingFailed;
    [SerializeField] private float GambaCost;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            GambaCalculation();
        }
    }

    private void GambaCalculation()
    {
        float QualityScore = (ButterQuality + SugarQuality + FlourQuality) / 3;

        GambaCost += (ButterQuality * 2 + SugarQuality * 1.5f + FlourQuality);

        if (OvenTemperature < 150)
        {
            QualityScore *= 0.7f;
            GambaCost += 1f;
            BakingTime += 10f;
        }
        else if (OvenTemperature > 250)
        {
            QualityScore *= 0.3f;
            GambaCost += 3f;
            BakingTime += 5f;
        }
        else
        {
            QualityScore *= 0.9f;
            GambaCost += 2f;
            BakingTime += 7f;
        }

        if (MixingSpeed == 1)
        {
            QualityScore *= 0.8f;
            GambaCost += 2f;
            BakingTime += 7f;
        }
        else
        {
            QualityScore *= 0.95f;
            GambaCost += 1f;
            BakingTime += 10f;
        }

        if (RollerSpeed == 1)
        {
            QualityScore *= 0.8f;
            GambaCost += 2f;
            BakingTime += 7f;
        }
        else
        {
            QualityScore *= 0.95f;
            GambaCost += 1f;
            BakingTime += 10f;
        }

        Debug.Log($"Gamba Quality Score: {QualityScore:F2}");
        Debug.Log($"Gamba Cost: ${GambaCost:F2}");
        Debug.Log($"Baking Time: {BakingTime} minutes");

        int randNum = UnityEngine.Random.Range(0, 100);
        Debug.Log($"Random Number: {randNum}");
        if (randNum <= QualityScore * 100)
        {
            BakingQuantity += 1f;
        }
        else
        {
            BakingFailed += 1f;
            Debug.Log("Gamba batch failed quality check!");
        }

        Debug.Log($"Gamba Quality Score: {QualityScore:F2}");
        Debug.Log($"Gamba Cost: ${GambaCost:F2}");
        Debug.Log($"Baking Time: {BakingTime} minutes");
        Debug.Log($"Baking Quantity: {BakingQuantity} gambas");
    }
}
