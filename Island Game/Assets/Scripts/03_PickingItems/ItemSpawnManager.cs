﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance;
    public Transform playerTransform;
    public string pickableLayerMask;

    public Transform itemsSpawnersParent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnAllItems());
    }

    private IEnumerator SpawnAllItems()
    {
        foreach(Transform itemSpawnerTransform in itemsSpawnersParent)
        {
            ItemSpawner spawner = itemSpawnerTransform.GetComponent<ItemSpawner>();
            if(spawner != null)
            {
                Vector3 randomPosition = GenerateRandomPosition(spawner.radius);
                if (spawner.singleObject && spawner.itemToSpawn.isStackable)
                {
                    CreateItemInPlace(itemSpawnerTransform.position + randomPosition, spawner.itemToSpawn, spawner.count);
                }
                else
                {
                    for (int i = 0; i < spawner.count; i++)
                    {
                        CreateItemInPlace(itemSpawnerTransform.position + randomPosition, spawner.itemToSpawn, 1);
                        randomPosition = GenerateRandomPosition(spawner.radius);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void CreateItemInPlace(Vector3 randomPosition, ItemSO itemToSpawn, int count)
    {
        var itemGameObject = Instantiate(itemToSpawn.model, randomPosition, Quaternion.identity, itemsSpawnersParent);
        PrepareItemGameObject(itemToSpawn.ID, count, itemGameObject);
    }

    private Vector3 GenerateRandomPosition(float radius)
    {
        return new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
    }

    public void CreateItemAtPlayersFeet(string itemID, int currentItemCount)
    {
        var itemPrefab = ItemDataManager.instance.GetItemPrefab(itemID);
        var itemGameObject = Instantiate(itemPrefab, playerTransform.position + Vector3.up, Quaternion.identity);
        PrepareItemGameObject(itemID, currentItemCount, itemGameObject);
    }

    private void PrepareItemGameObject(string itemID, int currentItemCount, GameObject itemGameObject)
    {
        itemGameObject.AddComponent<BoxCollider>();
        var rb = itemGameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        var pickableItem = itemGameObject.AddComponent<PickableItem>();
        pickableItem.SetCount(currentItemCount);
        pickableItem.dataSource = ItemDataManager.instance.GetItemData(itemID);
        itemGameObject.layer = LayerMask.NameToLayer(pickableLayerMask);
    }
}
