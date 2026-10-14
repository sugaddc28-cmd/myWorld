# 作業ログ

実施内容・確認結果・未確認事項を日付ごとに記録する。
今後の作業は [作業一覧](作業一覧.md)、継続して守るルールは [開発方針](開発方針.md) を参照。

## 2026-10-14

### ペンの土台

- `Assets/Scenes/VRCDefaultWorldScene.unity` にPen、PenTip、BoxCollider、Rigidbody、VRC PickupがあることをUnity上で確認。
- Penは2.5cm × 2.5cm × 20cmの細長いCube。PenTipを先端に配置。
- Rigidbodyは `Use Gravity = false`、`Is Kinematic = true`。
- ユーザー指定で、StrokeとLineRendererをPenの子に配置。作成時はワールド座標ON、点数0、ループOFF、線幅1cm、マテリアル未設定。
- VRChatでの拾う・動かす・手放す操作の確認は未完了。

### LineRendererの練習用サンプル

- `Assets/LineRendererExample.cs` を作成。Startで固定した3点をつなぐ折れ線を表示するサンプル。
- 当初はC#ファイルのみを作成し、対応するUdonSharp Program Assetの準備が不足していた。
- ユーザーが右クリックで作成した `aaa` とSDKの実装を確認。「Create → U# Script」は同名の `.cs` と `.asset` を作成し、`sourceCsScript` を設定することを確認。
- 既存の対応Assetを `Assets/LineRendererExample.asset` に移動・改名。参照用GUIDを維持し、Source ScriptのGUID一致を確認。
- Source Scriptが空でコンパイル全体を止めていた、未使用の `Stroke Udon C# Program Asset 1` を削除。
- ユーザーからコンパイルが通ったと思うとの報告あり。その後のUnityMCP復旧時にUdonSharpのエラー判定がfalseであることを確認。
- サンプル個別のコンパイル済みプログラム、Line参照・マテリアル、Play中の線の表示は確認未完了。
- ペン先の軌跡を記録する本番の描画スクリプトと、図形認識は未実装。

### UdonSharpの新規作成確認

- `Assets/USharpCreationCheck.cs`、同名の `.asset`、それぞれの `.meta` を作成。シーンには未取り付け。
- 初回はUnityMCPに接続できず、ファイルを直接生成していた。ユーザーから安全性について指摘があり、今後はUnity EditorのAPI経由でProgram Assetを作成する方針に変更。
- UnityMCP復旧後、Unity APIでAssetの認識、Source Script参照、コンパイル済みプログラムの存在を確認。
- `AnyUdonSharpScriptHasError()` がfalseであることを確認。シーン実行時のStartは未確認。

### Gitの生成物管理

- `Assets/SerializedUdonPrograms/` と `Assets/SerializedUdonPrograms.meta` を `.gitignore` に追加。
- ローカルファイルを残してGitの追跡を解除。83パスの追跡解除をステージし、対象の追跡ファイルが0件になったことと、ローカルファイルの存在を確認。
- この作業ではコミットを実施していない。

### UnityMCPの接続修復

- 修正前はTCP接続が通る一方、GETと認証付きidentity POSTが5秒でタイムアウトしていた。
- Unityの再コンパイル後、古い56873番ポートの占有が残っていることをログで確認。
- `Packages/com.claude.unity-mcp/Editor/UnityMCPConnection.cs` を修正。ポート共有を禁止し、占有時は別ポートへ切り替える。非同期Accept待機を `EditorApplication.update` からのPending確認へ変更。終了時にもサーバーを停止する。
- `.unity-mcp-vrc-source/UnityMCPPlugin/Editor/UnityMCPConnection.cs` にも同じ修正を反映。
- UnityのRefresh後に54152番ポートで復旧。GET・identity POSTとも200、MCPの一覧取得と読み取りコマンドの成功を確認。
- Restart Server相当の処理後も、再接続とコマンドの成功を確認。
- 上記パッケージとローカルソースは、このプロジェクトのGit除外対象。修正はローカルに保持されており、パッケージ更新時は再確認が必要。

### ドキュメントの整理

- TODOに混在していた作業履歴を、この作業ログへ分離。
- 開発方針・作業一覧・作業ログを日本語で整理し、`ドキュメント/` に集約。
- プロジェクト直下の `AGENTS.md` は日本語の参照案内に変更。従来のルールは開発方針に移し、入口から読み込むよう明記。
- 旧 `TODO.md` の内容を作業一覧・開発方針・作業ログへ振り分け、旧ファイルを削除。
