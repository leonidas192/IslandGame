using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementStorage : MonoBehaviour, ISavable
{
    List<Structure> playerStructure = new List<Structure>();

    public string GetJsonDataToSave()
    {
        List<SavedStructureData> savedStructuresList = new List<SavedStructureData>();
        foreach (var structure in playerStructure)
        {
            var euler = structure.transform.rotation.eulerAngles;
            savedStructuresList.Add(new SavedStructureData
            {
                ID = structure.Data.ID,
                posX = structure.transform.position.x,
                posY = structure.transform.position.x,
                posZ = structure.transform.position.x,
                rotationX = euler.x,
                rotationY = euler.y,
                rotationZ = euler.z,
            });
        }
        string data = JsonConvert.SerializeObject(savedStructuresList);
        return data;
    }

    public void LoadJsonData(string jsonData)
    {
        List<SavedStructureData> savedStructuresList = JsonConvert.DeserializeObject<List<SavedStructureData>>(jsonData);
        foreach (var data in savedStructuresList)
        {
            var itemData = ItemDataManager.instance.GetItemData(data.ID);
            var structureToPlace = ItemSpawnManager.instance.CreateStructure((StructureItemSO)itemData);
            structureToPlace.PrepareForMovement();
            var structureReference = structureToPlace.PrepareForPlacement();
            Vector3 position = new Vector3(data.posX, data.posY, data.posZ);
            Quaternion rotation = Quaternion.Euler(data.rotationX, data.rotationY, data.rotationZ);
            structureReference.transform.position = position;
            structureReference.transform.rotation = rotation;
            structureReference.SetData((StructureItemSO)itemData);
            SaveStructureReference(structureReference);
        }
    }

    public void SaveStructureReference(Structure structure)
    {
        playerStructure.Add(structure);
    }
}

[Serializable]
public struct SavedStructureData
{
    //position Vector3
    public float posX, posY, posZ;
    //rotation Quaternion
    public float rotationX, rotationY, rotationZ;
    //ID
    public string ID;
}


