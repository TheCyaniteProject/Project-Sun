using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIConstructItem : MonoBehaviour
{
    public Image image;
    public Slider slider;
    public int buildTime = 10;
    public int cost = 100; //TODO
    public bool isStructure = true;
    public UnityEvent callback;

    bool isBuilding = false;
    bool isReady = false;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
        {
            if (isBuilding)
            {
                isBuilding = false;
                PlayerData.Instance.AddMoney(cost);
                slider.value = 0;
                UIManager.Instance.EnableStructures();
                UIManager.Instance.voice.clip = UIManager.Instance.canceled;
                UIManager.Instance.voice.Play();
            }
        }

        if (isBuilding)
        {
            slider.value -= Time.deltaTime;
            if (slider.value <= 0)
            {
                isBuilding = false;
                if (isStructure)
                {
                    UIManager.Instance.voice.clip = UIManager.Instance.constructionEnd;
                    UIManager.Instance.voice.Play();
                    isReady = true;
                }
                else
                {
                    UIManager.Instance.voice.clip = UIManager.Instance.trainingEnd;
                    UIManager.Instance.voice.Play();
                    callback.Invoke();
                }
            }
        }
    }

    public void OnClick()
    {
        if (!isBuilding && !isReady)
        {
            if (PlayerData.Instance.money >= cost)
            {
                PlayerData.Instance.RemoveMoney(cost);

                slider.maxValue = buildTime;
                slider.value = buildTime;
                isBuilding = true;
                if (isStructure)
                {
                    UIManager.Instance.DisableStructures(gameObject.GetComponent<Button>());
                    UIManager.Instance.voice.clip = UIManager.Instance.constructionStart;
                    UIManager.Instance.voice.Play();
                }
                else
                {
                    UIManager.Instance.DisableStructures(gameObject.GetComponent<Button>());
                    UIManager.Instance.voice.clip = UIManager.Instance.trainingStart;
                    UIManager.Instance.voice.Play();
                }
            }
            else
            {
                UIManager.Instance.voice.clip = UIManager.Instance.lowMoney;
                UIManager.Instance.voice.Play();
            }
        }
        else if (isReady)
        {
            callback.Invoke();
            isReady = false;
        }
    }
}
