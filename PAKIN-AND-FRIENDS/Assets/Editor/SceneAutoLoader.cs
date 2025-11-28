using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

// สคริปต์นี้จะเพิ่มเมนู Tools ด้านบน
public class SceneAutoLoader : EditorWindow
{
    private const string MENU_NAME = "Tools/Always Start From Scene 0"; // ชื่อเมนู
    private const string SETTING_KEY = "SceneAutoLoader_Enabled"; // คีย์สำหรับจำค่า

    // เช็คว่าเปิดใช้งานอยู่ไหม
    private static bool IsEnabled
    {
        get { return EditorPrefs.GetBool(SETTING_KEY, false); }
        set { EditorPrefs.SetBool(SETTING_KEY, value); }
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อกดเมนู
    [MenuItem(MENU_NAME)]
    private static void ToggleAction()
    {
        IsEnabled = !IsEnabled; // สลับ เปิด/ปิด
        SetPlayModeStartScene(IsEnabled);
    }

    // ฟังก์ชันนี้ทำให้มีเครื่องหมายถูก (Checkmark) หน้าเมนู
    [MenuItem(MENU_NAME, true)]
    private static bool ToggleActionValidate()
    {
        Menu.SetChecked(MENU_NAME, IsEnabled);
        return true;
    }

    private static void SetPlayModeStartScene(bool enable)
    {
        if (enable)
        {
            // ตรวจสอบว่ามีการใส่ Scene ใน Build Settings หรือยัง
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.LogError("Error: คุณยังไม่ได้ใส่ Scene ใน Build Settings เลย! ไปที่ File > Build Settings แล้วลากซีนใส่ก่อนนะครับ");
                IsEnabled = false;
                return;
            }

            // ดึงซีนเบอร์ 0 มาตั้งเป็นจุดเริ่ม
            string scenePath = EditorBuildSettings.scenes[0].path;
            SceneAsset myScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            EditorSceneManager.playModeStartScene = myScene;
            Debug.Log($"<color=green>Auto-Load Enabled:</color> กด Play จะเริ่มที่ {scenePath} เสมอ");
        }
        else
        {
            // ยกเลิกการบังคับเริ่ม (กลับไปเริ่มซีนปัจจุบันที่เปิดอยู่)
            EditorSceneManager.playModeStartScene = null;
            Debug.Log("<color=yellow>Auto-Load Disabled:</color> กด Play จะเริ่มที่ซีนปัจจุบัน");
        }
    }
}