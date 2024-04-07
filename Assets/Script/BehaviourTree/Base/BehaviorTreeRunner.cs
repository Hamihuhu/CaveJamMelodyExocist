using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    [SerializeField] private BehaviorTree tree;


    // Start is called before the first frame update
    void Start()
    {
        tree = ScriptableObject.CreateInstance<BehaviorTree>();

        var debug1 = ScriptableObject.CreateInstance<DebugLogNode>();
        debug1.message = "Sup1";

        var debug2 = ScriptableObject.CreateInstance<DebugLogNode>();
        debug2.message = "Sup2";

        var debug3 = ScriptableObject.CreateInstance<DebugLogNode>();
        debug3.message = "Sup3";

        var sequence = ScriptableObject.CreateInstance<SequenceNode>();
        sequence.children.Add(debug1);
        sequence.children.Add(debug2);
        sequence.children.Add(debug3);

        var repeat = ScriptableObject.CreateInstance<RepeatNode>();

        tree.rootNode = sequence;
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
