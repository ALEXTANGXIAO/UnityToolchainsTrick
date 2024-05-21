using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
/*
 * ���Ĵ�����Ⱥ�� ��С��ؼK ���ṩ
 *  ʵս�ֿ⣺https://github.com/Bian-Sh/TinyWebServer 
 */
public class TinyWebServerWindow : EditorWindow
{
    private static EditorWindow window;

    public bool IsListening { get; private set; }
    private bool isTitleChangeRequired = true;


    [MenuItem("Tools/Example_79_EnableTitleContentRichText", priority = 79)]
    private static void Init()
    {
        window = GetWindow<TinyWebServerWindow>();
        window.minSize = new Vector2(300, 180);
        var icon = EditorGUIUtility.IconContent("d_BuildSettings.Web.Small");
        window.titleContent = new GUIContent("Tiny WebServer", icon.image);
        window.Show();
    }

    private void TryRepaintTitleContent()
    {
        window ??= GetWindow<TinyWebServerWindow>();
        var assembly = typeof(AssetDatabase).Assembly;
        var styleType = assembly.GetType("UnityEditor.DockArea+Styles");
        var style = styleType.GetField("tabLabel", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null) as GUIStyle;
        window.titleContent.text = "Tiny WebServer<*****=###>��</*****>";
        var calcSize = style.CalcSize(window.titleContent);
        styleType.GetField("tabMaxWidth", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).SetValue(null, calcSize.x);
        var contents = assembly.GetType("UnityEditor.DockArea").GetField("s_GUIContents", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);
        (contents as IDictionary).Clear();
        window.titleContent.text = "Tiny WebServer <color=" + (IsListening == true ? "green" : "red") + ">��</color>";
        window.titleContent.tooltip = IsListening == true ? "Server is running" : "Server is stoped";
    }

    long cachedticks = 0l;
    private void Update()
    {
        //����ÿ1500���뷭ת Listening �� isTitleChangeRequired ״̬
        // Toggle the Listening and isTitleChangeRequired states every 1500 milliseconds for testing
        // ǰ�߾���С����ɫ�����߾����Ƿ���Ҫˢ�±���
        // The former determines the color of the dot, and the latter determines if the title needs to be refreshed
        if (DateTime.Now.Ticks - cachedticks <= 15000000)
        {
            return;
        }
        IsListening = !IsListening;
        isTitleChangeRequired = true;
        Repaint();
        cachedticks = DateTime.Now.Ticks;
    }

    private void OnGUI()
    {
        if (isTitleChangeRequired)
        {
            TryRepaintTitleContent();
            isTitleChangeRequired = false;
        }
    }
}
