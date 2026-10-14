using UdonSharp;
using UnityEngine;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class LineRendererExample : UdonSharpBehaviour
{
    public LineRenderer line;

    private void Start()
    {
        if (line == null) return;

        // ワールド座標を使い、線を空中の同じ位置に残す。
        line.useWorldSpace = true;
        line.loop = false;
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;

        // 点の数を決めてから、0番・1番・2番の座標を設定する。
        line.positionCount = 3;
        line.SetPosition(0, new Vector3(-0.3f, 1.3f, 1f));
        line.SetPosition(1, new Vector3(0f, 1.6f, 1f));
        line.SetPosition(2, new Vector3(0.3f, 1.3f, 1f)); 
    }
}
