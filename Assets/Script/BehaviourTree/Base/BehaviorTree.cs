using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Behavior Tree")]
public class BehaviorTree : ScriptableObject
{
    public BTNode rootNode;
    public BTNode.State treeState = BTNode.State.RUNNING;
    public List<BTNode> nodes = new List<BTNode>();


    public BTNode.State Update()
    {
        if (rootNode.state == BTNode.State.RUNNING)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }

    public BTNode CreateNode(System.Type type)
    {
        BTNode node = ScriptableObject.CreateInstance(type) as BTNode;
        node.name = type.Name;
        node.guid = System.Guid.NewGuid().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();

        return node;
    }

    public void DeleteNode(BTNode node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(BTNode parent, BTNode child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            decoratorNode.child = child;
        }

        CompositeNode compositeNode = parent as CompositeNode;
        if (compositeNode)
        {
            compositeNode.children.Add(child);
        }
    }

    public void RemoveChild(BTNode parent, BTNode child)
    {
        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            decoratorNode.child = null;
        }

        CompositeNode compositeNode = parent as CompositeNode;
        if (compositeNode)
        {
            compositeNode.children.Remove(child);
        }
    }

    public List<BTNode> GetChildren(BTNode parent)
    {
        List<BTNode> children = new List<BTNode>();

        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode && decoratorNode.child != null)
        {
            children.Add(decoratorNode.child);
        }

        CompositeNode compositeNode = parent as CompositeNode;
        if (compositeNode)
        {
            return compositeNode.children;
        }

        return children;
    }
}
