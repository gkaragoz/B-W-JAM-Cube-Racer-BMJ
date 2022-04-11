using System.Collections.Generic;
using UnityEngine;

public class RescuePlayer : MonoBehaviour
{
    private Dictionary<string, Transform> _rescueTransformsDict = new Dictionary<string, Transform>();

    private void Start()
    {
        GameManager.OnGameStart += OnGameStart;
    }

    private void OnGameStart()
    {
        var rescueObjects = GameObject.FindGameObjectsWithTag("Rescue");
        
        foreach (var item in rescueObjects)
            _rescueTransformsDict.Add(item.name, item.transform);
    }

    public void Rescue()
    {
        var playerObject = GameObject.Find("Player");

        if (playerObject == null)
            return;

        if (SideCalculator.Instance == null)
            return;
        
        var currentArea = SideCalculator.Instance.Area;

        playerObject.transform.position = _rescueTransformsDict[currentArea].position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Rescue();
    }
}
