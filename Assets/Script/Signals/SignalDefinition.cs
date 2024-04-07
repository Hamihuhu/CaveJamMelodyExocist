using deVoid.Utils;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerRequestLockOn : ASignal<Transform> { }

public class OnPlayerRequestRush : ASignal<List<RushableTarget>> { }
