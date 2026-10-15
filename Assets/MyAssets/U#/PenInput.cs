using UdonSharp;
using UnityEngine;

// 入力と描画中の状態を、このプレイヤーの環境だけで扱う。
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PenInput : UdonSharpBehaviour
{
    // 次の段階で、軌跡を記録するかどうかの判断に使う。
    [HideInInspector] public bool isDrawing;

    public override void OnPickupUseDown()
    {
        isDrawing = true;
        Debug.Log("[PenInput] 描画開始（Use Down）");
    }

    public override void OnPickupUseUp()
    {
        isDrawing = false;
        Debug.Log("[PenInput] 描画終了（Use Up）");
    }

    public override void OnDrop()
    {
        // ボタンを押したまま手放しても、描画中の状態を残さない。
        isDrawing = false;
        Debug.Log("[PenInput] ペンを手放した（Drop）");
    }
}
