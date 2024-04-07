using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviorTreeEditor : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    private BehaviorTreeView m_BehaviorTreeView;
    InspectorView m_InspectorView;

    [MenuItem("Window/BehaviorTreeEditor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        m_BehaviorTreeView = root.Query<BehaviorTreeView>().First();
        m_InspectorView = root.Query<InspectorView>().First();

        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        BehaviorTree behaviorTree = Selection.activeObject as BehaviorTree;
        if (behaviorTree)
        {
            m_BehaviorTreeView.PopulateView(behaviorTree);
        }
    }
}
