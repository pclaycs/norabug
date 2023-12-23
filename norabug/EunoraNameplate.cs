using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace norabug
{
    public class EunoraNameplate : MonoBehaviour
    {
        public EnemyAI enemyAI;
        public GameObject canvasItem;
        public CanvasGroup canvasItemAlpha;

        public void Awake()
        {
            canvasItem = new GameObject("EunoraNameplateCanvasItem");
            canvasItem.transform.SetParent(enemyAI.eye, false);
            canvasItem.AddComponent<CanvasRenderer>();
            canvasItemAlpha = canvasItem.AddComponent<CanvasGroup>();
            canvasItemAlpha.alpha = 1f;
            canvasItem.transform.localPosition = new Vector3(0f, 60f, 0f);

        }
    }
}
