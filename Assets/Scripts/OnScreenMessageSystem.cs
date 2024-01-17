using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnScreenMessage
{
    public GameObject go;
    public float timeToLive;

    public OnScreenMessage(GameObject go)
    {
        this.go = go;
    }
}

public class OnScreenMessageSystem : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;
    [SerializeField] GameObject critTextPrefab;

    [SerializeField] float timeForAnimation = 0.4f;

    List<OnScreenMessage> messageList;
    List<OnScreenMessage> openList;

    [SerializeField] float horizontalScatter = 0.5f;
    [SerializeField] float verticalScatter = 1f;

    private void Awake()
    {
        messageList = new List<OnScreenMessage>();
        openList = new List<OnScreenMessage>();
    }

    private void Update()
    {
        for(int i = messageList.Count - 1; i >= 0; i--)
        {
            messageList[i].timeToLive -= Time.deltaTime;
            if (messageList[i].timeToLive < 0)
            {
                messageList[i].go.SetActive(false);
                openList.Add(messageList[i]);
                messageList.RemoveAt(i);
            }
        }
    }

    public void PostMessage(Vector3 worldPosition, string message, bool isCritical)
    {
        worldPosition.z = -1f;
        worldPosition.x += Random.Range(-horizontalScatter, horizontalScatter);
        worldPosition.y += Random.Range(-verticalScatter, verticalScatter);

        if (openList.Count > 0)
        {
            ReuseObjectOpenList(worldPosition, message, isCritical);
        }
        else
        {
            CreateNewOnScreenMessageObject(worldPosition, message, isCritical);
        }
    }

    private void ReuseObjectOpenList(Vector3 worldPosition, string message, bool isCritical)
    {
        OnScreenMessage osm = openList[0];
        osm.go.SetActive(true);
        osm.timeToLive = timeForAnimation;
        osm.go.GetComponent<DamageText>().SetText(message, isCritical);
        osm.go.transform.position = worldPosition;
        openList.RemoveAt(0);
        messageList.Add(osm);

        LeanTween.moveY(osm.go, osm.go.transform.position.y + 1, timeForAnimation);
    }

    private void CreateNewOnScreenMessageObject(Vector3 worldPosition, string message, bool isCritical)
    {
        GameObject textGO = Instantiate(textPrefab, transform);
        textGO.transform.position = worldPosition;

        textGO.GetComponent<DamageText>().SetText(message, isCritical);

        OnScreenMessage onScreenMessage = new OnScreenMessage(textGO);
        onScreenMessage.timeToLive = timeForAnimation;

        messageList.Add(onScreenMessage);

        LeanTween.moveY(textGO, textGO.transform.position.y + 1, timeForAnimation);
    }
}
