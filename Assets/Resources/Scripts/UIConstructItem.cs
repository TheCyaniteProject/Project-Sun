using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIConstructItem : MonoBehaviour
{
    public Image image;
    public UnityEvent callback;

    public void OnClick()
    {
        callback.Invoke();
    }
}
