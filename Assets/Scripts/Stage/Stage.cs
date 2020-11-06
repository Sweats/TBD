using UnityEngine;
using System.Collections.Generic;

// NOTE: Not sure how this class will be used exactly. Maybe it will handle global events or something.
public class Stage : MonoBehaviour
{
    [SerializeField]
    private int batterySpawnChance;

    [SerializeField]
    private int forcedPathID = -1;

    [SerializeField]
    private int choosenKeyPath;

    private ObjectSpawnPoint[] _objectSpawnPoints;

    [SerializeField]
    private KeyObject keyObject;

    private void Start()
    {
        SetSpawnPoints();
        ChoosePath();
        SpawnKeys();
    }


    private void SetSpawnPoints()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tags.OBJECT_SPAWN_POINT);
        List<ObjectSpawnPoint> objectSpawnPoints = new List<ObjectSpawnPoint>();

        for (var i = 0; i < gameObjects.Length; i++)
        {
            ObjectSpawnPoint spawnPoint = gameObjects[i].GetComponent<ObjectSpawnPoint>();
            objectSpawnPoints.Add(spawnPoint);
        }

        _objectSpawnPoints = objectSpawnPoints.ToArray();
    }


    private void ChoosePath()
    {
        if (forcedPathID <= -1)
        {
            int maxPaths = GetMaxPaths();
            choosenKeyPath = Random.Range(0, maxPaths);
        }

        else
        {
            choosenKeyPath = forcedPathID;
        }

    }

    private void SpawnKeys()
    {
        List<int> ignoreMaskList = new List<int>();

        for (var i = 0; i < _objectSpawnPoints.Length; i++)
        {
            ObjectSpawnPoint spawnPoint = _objectSpawnPoints[i];
            Key[] keys = spawnPoint.Keys();

            for (var j = 0; j < keys.Length; j++)
            {
                Key key = keys[j];
                int keyPathID = key.PathID();
                int maskID = key.Mask();
		// Only keys with a unique mask can be spawned. Any duplicates will be ignored.
		// TODO: How should we make it so we can have multiple keys of the same mask on the stage at a time?
                bool ignore = ShouldIgnore(ignoreMaskList, maskID);

                if (keyPathID == choosenKeyPath && !ignore)
                {
                    ignoreMaskList.Add(maskID);
                    SpawnKey(key);

                }
            }
        }
    }


    private bool ShouldIgnore(List<int> ignoreMaskList, int maskID)
    {
        bool found = false;

        for (var i = 0; i < ignoreMaskList.Capacity; i++)
        {
            int foundIgnoreMaskID = ignoreMaskList[i];

            if (maskID == foundIgnoreMaskID)
            {
                found = true;
                break;
            }
        }


        return found;
    }

    private void SpawnKey(Key key)
    {
        Debug.Log($"Key spawned mask = {key.Mask()} ");
        ObjectSpawnPoint[] potentialSpawnPoints = GetSpawnPointsForKey(key);
        int randomNumber = Random.Range(0, potentialSpawnPoints.Length);
        ObjectSpawnPoint pickedSpawnPoint = potentialSpawnPoints[randomNumber];
        keyObject.SetKey(key);
        Instantiate(keyObject, pickedSpawnPoint.transform.position, Quaternion.identity);
    }

    private ObjectSpawnPoint[] GetSpawnPointsForKey(Key key)
    {
        List<ObjectSpawnPoint> objectSpawnPoints = new List<ObjectSpawnPoint>();

        for (var i = 0; i < _objectSpawnPoints.Length; i++)
        {
            ObjectSpawnPoint foundObjectSpawnPoint = _objectSpawnPoints[i];
            Key[] keys = _objectSpawnPoints[i].Keys();

            for (var j = 0; j < keys.Length; j++)
            {
                Key foundKey = keys[j];
                int foundKeyMaskID = foundKey.Mask();
                int foundKeyPathID = foundKey.PathID();

                int keyPathID = key.PathID();
                int keyMask = key.Mask();

                if (foundKeyMaskID == keyMask && keyPathID == foundKeyPathID)
                {
                    objectSpawnPoints.Add(foundObjectSpawnPoint);
                }
            }
        }

        return objectSpawnPoints.ToArray();

    }


    private int GetMaxPaths()
    {
        return 0;
    }
}
