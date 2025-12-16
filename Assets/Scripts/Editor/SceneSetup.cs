using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class SceneSetup
{
    public static void Execute()
    {
        // 1. Setup GameManager
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        if (gm == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        GameObject playerCar = GameObject.Find("PlayerCar");
        GameObject pedestrian = GameObject.Find("Yaya");
        GameObject xrOrigin = GameObject.Find("XR Origin (VR)");
        GameObject entryPanel = GameObject.Find("Canvas/GirişPaneli");

        gm.playerCar = playerCar;
        gm.pedestrian = pedestrian;
        gm.xrOrigin = xrOrigin;
        gm.entryPanel = entryPanel;

        if (xrOrigin != null)
        {
            gm.cameraFollowScript = xrOrigin.GetComponent<XRCameraFollow>();
        }

        EditorUtility.SetDirty(gm);

        // 2. Setup Pedestrian Controller Input
        if (pedestrian != null)
        {
            // Raise Pedestrian to avoid sticking in ground
            pedestrian.transform.position = new Vector3(pedestrian.transform.position.x, 1.0f, pedestrian.transform.position.z);

            PedestrianController pc = pedestrian.GetComponent<PedestrianController>();
            if (pc == null) pc = pedestrian.AddComponent<PedestrianController>();

            // Load Input Action Asset and find Reference
            InputActionAsset actionAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Input/PedestrianActions.inputactions");
            if (actionAsset != null)
            {
                // We need to find the InputActionReference, which is a sub-asset
                // This is a bit tricky via API, but we can try to load all assets at path
                Object[] assets = AssetDatabase.LoadAllAssetsAtPath("Assets/Input/PedestrianActions.inputactions");
                InputActionReference moveRef = assets.OfType<InputActionReference>().FirstOrDefault(r => r.name == "Move" || r.name == "Gameplay/Move");
                
                if (moveRef != null)
                {
                    pc.moveInputSource = new InputActionProperty(moveRef);
                }
                else
                {
                    // Fallback: Create property from action (might not serialize reference correctly but works runtime if asset is loaded)
                    InputAction moveAction = actionAsset.FindActionMap("Gameplay").FindAction("Move");
                    if (moveAction != null)
                    {
                        pc.moveInputSource = new InputActionProperty(moveAction);
                    }
                }
            }
            
            // Assign Animator if missing
            if (pc.animator == null)
                pc.animator = pedestrian.GetComponentInChildren<Animator>();

            // Disable by default (GameManager will enable it)
            pc.enabled = false;

            EditorUtility.SetDirty(pc);
        }

        // 3. Setup UI Buttons
        if (entryPanel != null)
        {
            Transform carBtnTr = entryPanel.transform.Find("Araba Modu Seç");
            if (carBtnTr != null)
            {
                Button btn = carBtnTr.GetComponent<Button>();
                if (btn != null)
                {
                    UnityEditor.Events.UnityEventTools.RemovePersistentListener(btn.onClick, gm.StartCarMode);
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, gm.StartCarMode);
                }
            }

            Transform pedBtnTr = entryPanel.transform.Find("Yaya Modu Seç");
            if (pedBtnTr != null)
            {
                Button btn = pedBtnTr.GetComponent<Button>();
                if (btn != null)
                {
                    UnityEditor.Events.UnityEventTools.RemovePersistentListener(btn.onClick, gm.StartPedestrianMode);
                    UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, gm.StartPedestrianMode);
                }
            }
        }

        Debug.Log("Scene Setup Updated!");
    }
}
