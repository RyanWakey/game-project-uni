using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AchievementNotification : MonoBehaviour
{
    public static AchievementNotification instance { get; private set; }

    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float displayTime = 4.0f;
    [SerializeField] private float fadeTime = 1.0f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowNotification(string notification)
    {
        StartCoroutine(ShowNotificationRoutine(notification));
    }

    private IEnumerator ShowNotificationRoutine(string notification)
    {
        notificationText.text = notification;


        float timer = 0;
        while (timer < fadeTime)
        {
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, timer / fadeTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);

        timer = 0;
        while (timer < fadeTime)
        {
            notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 1 - (timer / fadeTime));
            timer += Time.deltaTime;
            yield return null;
        }

        notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 0);
    }
}
