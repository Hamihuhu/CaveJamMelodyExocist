using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BTNodeView : UnityEditor.Experimental.GraphView.Node
{
    public BTNode node;
    public Port inputPort;
    public Port outputPort;

    public BTNodeView(BTNode node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPort();
        CreateOutputPort();
    }

    private void CreateInputPort()
    {
        if (node is TaskNode)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is CompositeNode)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }

        if (inputPort != null)
        {
            inputPort.portName = "";
            inputContainer.Add(inputPort);
        }
    }

    private void CreateOutputPort()
    {
        if (node is TaskNode)
        {
            
        }
        else if (node is CompositeNode)
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (outputPort != null)
        {
            outputPort.portName = "";
            outputContainer.Add(outputPort);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }
}
