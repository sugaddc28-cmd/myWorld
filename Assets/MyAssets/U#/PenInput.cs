using UdonSharp;
using UnityEngine;

// 入力と描画中の状態を、このプレイヤーの環境だけで扱う。
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PenInput : UdonSharpBehaviour
{
	// 描画設定
	[SerializeField] float minPointDistance = 0.005f;

	// 描画状態
	public bool IsDrawing => Trail.gameObject.activeSelf;

	// コンポーネント参照
	LineRenderer _line;
	LineRenderer Line
	{
		get
		{
			if (_line != null) return _line;

			_line = GetComponentInChildren<LineRenderer>(true);

			if (_line == null)
			{
				Debug.LogError(
						"[PenInput] 子にLineRendererが見つかりません。"
						+ "Penの子のStrokeにLineRendererを追加してください。");

				return null;
			}

			_line.useWorldSpace = true;
			_line.loop = false;

			return _line;
		}
	}

	TrailRenderer _trail;
	TrailRenderer Trail
	{
		get
		{
			if (_trail != null) return _trail;

			_trail = GetComponentInChildren<TrailRenderer>(true);

			if (_trail == null)
			{
				Debug.LogError(
						"[PenInput] 子にTrailRendererが見つかりません。"
						+ "PenTipにTrailRendererを追加してください。");

				return null;
			}

			_trail.emitting = true;
			_trail.minVertexDistance = minPointDistance;

			return _trail;
		}
	}

	// 描画開始
	public override void OnPickupUseDown(){

		// 前回の軌跡を消し、新しい一筆を開始する。
		Trail.Clear();
		Trail.gameObject.SetActive(true);
	}

	// 描画停止
	public override void OnPickupUseUp(){
		FinishDrawing();
	}

	public override void OnDrop(){
		FinishDrawing();
	}


	void FinishDrawing(){
		if(!IsDrawing)return;

		int count = Trail.positionCount;

		if(count >= 2){
			// 実際に記録された点数だけ配列を確保する
			Vector3[] positions = new Vector3[count];
			Trail.GetPositions(positions);

			// 一筆ごとにLineRendererを複製して残す
			GameObject stroke = Instantiate(Line.gameObject);
			stroke.transform.SetParent(transform,true);

			LineRenderer savedLine = stroke.GetComponent<LineRenderer>();

			savedLine.positionCount = count;
			savedLine.SetPositions(positions);
			stroke.SetActive(true);
		}

		Trail.gameObject.SetActive(false);
		Trail.Clear();
	}
}
