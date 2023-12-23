using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using BepInEx;



using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using BepInEx.Configuration;
using BepInEx.Logging;
using Microsoft.CodeAnalysis;
using Unity.Netcode;
using UnityEngine;

namespace norabug.Patches
{
    [HarmonyPatch(typeof(HoarderBugAI))]
    internal static class HoarderBugNameplatePatch
    {
        //public static string playerUsername = "eunora";

        //public static TextMeshProUGUI usernameBillboardText = new();
        //public static Transform usernameBillboard = new();
        //public static CanvasGroup usernameAlpha = new();
        //public static Canvas usernameCanvas = new();

        //public static EnemyAI enemyAI;
        public static Canvas canvas;
        public static GameObject canvasItem;
        public static CanvasGroup canvasItemAlpha;

        private static void PrintChildren(Transform parent, int depth)
        {
            // Iterate over each child transform
            foreach (Transform child in parent)
            {
                // Create indentation based on depth in the hierarchy
                string indent = new string('-', depth * 2);

                // Print the child's name, type, and other properties
                GameObject go = child.gameObject;
                Plugin.Log.LogInfo($"{indent}{go.name} (GameObject), Active: {go.activeSelf}, Layer: {LayerMask.LayerToName(go.layer)}");
                Plugin.Log.LogInfo($"{indent}  Position: {go.transform.position}, Rotation: {go.transform.rotation}, Scale: {go.transform.localScale}");

                // Print each component attached to this child
                foreach (Component component in child.GetComponents<Component>())
                {
                    string componentDetails = GetComponentDetails(component);
                    Plugin.Log.LogInfo($"{indent}  - {component.GetType()}, {componentDetails}");
                }

                // Recursively print this child's children
                PrintChildren(child, depth + 1);
            }
        }

        private static string GetComponentDetails(Component component)
        {
            if (component is Canvas canvas)
            {
                return $"RenderMode: {canvas.renderMode}, WorldCamera: {canvas.worldCamera}";
            }
            else if (component is CanvasGroup canvasGroup)
            {
                return $"Alpha: {canvasGroup.alpha}, Interactable: {canvasGroup.interactable}, BlocksRaycasts: {canvasGroup.blocksRaycasts}";
            }
            else if (component is TextMeshProUGUI tmp)
            {
                return $"Text: {tmp.text}, FontSize: {tmp.fontSize}, Color: {tmp.color}";
            }

            // Add more component types here if needed

            return "Details not specified for this component type";
        }

        private static void FUCKYOU()
        {
            // Find the GameObject named "HoarderBugModel"
            GameObject hoarderBugModel = GameObject.Find("HoarderBugModel");
            if (hoarderBugModel != null)
            {
                // Print details of the GameObject and its hierarchy
                PrintHierarchyDetails(hoarderBugModel.transform);
            }
            else
            {
                Plugin.Log.LogInfo("HoarderBugModel not found in the scene.");
            }
        }

        private static void PrintHierarchyDetails(Transform currentTransform)
        {
            while (currentTransform != null)
            {
                // Print current GameObject's details
                PrintDetails(currentTransform);

                // Print children details
                PrintChildrenDetails(currentTransform, 1);

                // Move to the parent for the next iteration
                currentTransform = currentTransform.parent;
            }
        }

        private static void PrintDetails(Transform transform)
        {
            Plugin.Log.LogInfo("GameObject: " + transform.gameObject.name);
            Plugin.Log.LogInfo("  Position: " + transform.position);
            Plugin.Log.LogInfo("  Rotation: " + transform.rotation);
            Plugin.Log.LogInfo("  Scale: " + transform.localScale);
        }

        private static void PrintChildrenDetails(Transform parent, int depth)
        {
            string indent = new string('-', depth * 2);
            foreach (Transform child in parent)
            {
                Plugin.Log.LogInfo($"{indent}Child: {child.gameObject.name}, Position: {child.position}, Rotation: {child.rotation}, Scale: {child.localScale}");
                PrintChildrenDetails(child, depth + 1);
            }
        }

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPostfix(HoarderBugAI __instance)
        {
            Transform bug = ((Component)__instance).transform.Find("HoarderBugModel");

            var billboard = StartOfRound.Instance.allPlayerScripts[0].usernameCanvas;
            canvas = GameObject.Instantiate(billboard);
            canvasItem = canvas.gameObject;
            canvasItem.layer = LayerMask.NameToLayer("Enemies");

            canvasItem.transform.SetParent(bug);
            canvasItem.transform.localScale = new Vector3(1, 1, 1);
            canvasItem.transform.localPosition = new Vector3(0, 0, 0); // Example position

            var canvasGroup = canvasItem.GetComponentInChildren<CanvasGroup>();
            canvasGroup.alpha = 1f;

            var textMesh = canvasItem.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = "eunora";
            textMesh.enabled = true;

            canvas.gameObject.SetActive(true);


            ////PrintChildren(canvasItem.transform, 0);

            //for (int i = 0; i < 32; i++) // Unity has a maximum of 32 layers
            //{
            //    string layerName = LayerMask.LayerToName(i);
            //    if (!string.IsNullOrEmpty(layerName)) // Check if the layer is defined
            //    {
            //        Plugin.Log.LogInfo("Layer " + i + ": " + layerName);
            //    }
            //}

            ////EnemyChecker();
            FUCKYOU();
        }

        [HarmonyPatch("LateUpdate")]
        [HarmonyPrefix]
        private static bool LateUpdatePrefix(HoarderBugAI __instance)
        {
            canvasItem.transform.LookAt(GameNetworkManager.Instance.localPlayerController.localVisorTargetPoint);

            return true;
        }
    }
}
