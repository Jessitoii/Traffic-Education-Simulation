using UnityEngine;
using UnityEditor;

public class DisableConflictingScripts
{
    public static void Execute()
    {
        GameObject mainCam = GameObject.FindWithTag("MainCamera");
        if (mainCam != null)
        {
            VRThirdPersonCam camScript = mainCam.GetComponent<VRThirdPersonCam>();
            if (camScript != null)
            {
                camScript.enabled = false;
                EditorUtility.SetDirty(camScript);
                Debug.Log("Disabled VRThirdPersonCam on Main Camera.");
            }
        }
    }
}
