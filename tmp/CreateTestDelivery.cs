#nullable enable
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using IndiGame.Core;

public class CreateTestDelivery
{
    public static void Main()
    {
        string folder = "Assets/_Game/GameAssets/Data";
        if (!AssetDatabase.IsValidFolder(folder))
        {
            string parent = "Assets/_Game/GameAssets";
            if (!AssetDatabase.IsValidFolder(parent))
            {
                AssetDatabase.CreateFolder("Assets/_Game", "GameAssets");
            }
            AssetDatabase.CreateFolder(parent, "Data");
        }

        string assetPath = folder + "/TestDelivery_01.asset";
        DeliveryData asset = ScriptableObject.CreateInstance<DeliveryData>();
        asset.clientName = "Sr. Valdez";
        asset.address = "Av. Siempre Viva 742";
        asset.worldDestination = new Vector3(50, 0, 80);
        asset.estimatedTime = 8.5f;
        asset.status = DeliveryStatus.PENDING;

        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        Debug.Log("Created TestDelivery_01.asset at " + assetPath);
    }
}
