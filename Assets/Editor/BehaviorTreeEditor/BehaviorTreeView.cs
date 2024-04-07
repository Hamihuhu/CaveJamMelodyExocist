using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BehaviorTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }

    private BehaviorTree behaviorTree;

    public BehaviorTreeView()
    {
        
        Insert(0, new GridBackground());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviorTreeEditor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    public void PopulateView(BehaviorTree behaviorTree)
    {
        this.behaviorTree = behaviorTree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        behaviorTree.nodes.ForEach(node => CreateNodeView(node));
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        this.focusable = true;
        this.Focus();

        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var element in graphViewChange.elementsToRemove)
            {
                if (element is BTNodeView nodeView)
                {
                    behaviorTree.DeleteNode(nodeView.node);
                }

                Edge edge = element as Edge;
                if (edge != null)
                {
                    BTNodeView inputNodeView = edge.input.node as BTNodeView;
                    BTNodeView outputNodeView = edge.output.node as BTNodeView;

                    BTNode inputNode = inputNodeView.node;
                    BTNode outputNode = outputNodeView.node;

                    behaviorTree.RemoveChild(outputNode, inputNode);
                }
            }
        }

        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                Port input = edge.input;
                Port output = edge.output;

                BTNodeView inputNodeView = input.node as BTNodeView;
                BTNodeView outputNodeView = output.node as BTNodeView;

                BTNode inputNode = inputNodeView.node;
                BTNode outputNode = outputNodeView.node;

                behaviorTree.AddChild(outputNode, inputNode);
            }
        }

        return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(port =>
        {
            if (startPort.direction == port.direction)
                return false;

            if (startPort.node == port.node)
                return false;

            return true;
        }).ToList();
    }

    private void CreateNodeView(BTNode node)
    {
        BTNodeView nodeView = new BTNodeView(node);
        AddElement(nodeView);
    }

    override public void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<TaskNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    private void CreateNode(Type type)
    {
        BTNode node = behaviorTree.CreateNode(type);
        CreateNodeView(node);
    }
}
