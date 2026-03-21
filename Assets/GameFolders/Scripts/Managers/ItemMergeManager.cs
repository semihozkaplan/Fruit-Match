using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemMergeManager : MonoBehaviour
{
    [Header("Merge Settings")]
    [SerializeField] private float _moveUpDist;
    [SerializeField] private float _moveUpDuration = 0.2f;
    [SerializeField] private LeanTweenType _moveUpEase;
    [SerializeField] private float _splashDuration;
    [SerializeField] private LeanTweenType _splashEase;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _splashParticles;

    private void Awake()
    {
       ItemSpotManager.OnMergeStarted += HandleMergeStarted; 
    }

    private void OnDestroy()
    {
       ItemSpotManager.OnMergeStarted -= HandleMergeStarted;
    }

    private void HandleMergeStarted(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            Vector3 targetPosition = item.transform.position + item.transform.up * _moveUpDist;

            Action completeCallback = null;

            if (i == 2)
                completeCallback = () => SplashItems(items);

            LeanTween.move(item.gameObject, targetPosition, _moveUpDuration)
                .setEase(_moveUpEase)
                .setOnComplete(completeCallback);
        }
    }

    private void SplashItems(List<Item> items)
    {
        // We sorted items, in this way we can find the left, middle and right items
        items.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        float mergeTargetPosX = items[1].transform.position.x;

        LeanTween.moveX(items[0].gameObject, mergeTargetPosX, _splashDuration)
            .setEase(_splashEase)
            .setOnComplete(() => FinishMerge(items));

        LeanTween.moveX(items[2].gameObject, mergeTargetPosX, _splashDuration)
            .setEase(_splashEase);
    }

    private void FinishMerge(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }

        // Pooling can be used, we can add generic pooling to this project
        ParticleSystem particles = Instantiate(_splashParticles, items[1].transform.position, Quaternion.identity, transform);
        particles.Play();
    }
}