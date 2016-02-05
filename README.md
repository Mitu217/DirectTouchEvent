#DirectTouchEvent

## 概要
* タッチイベントの管理を簡単にするためのスクリプト郡です。
* UI部、ゲームロジック部のタッチイベントを個別に制御・管理できるので非常に便利です。
* Android, iOSで接触面積[radius]、圧力(フォースタッチ)[pressure]の値も取得可能です。
* このスクリプト郡を使用するとHierarchyに「DirectTouchEventSystem」というオブジェクトが生成されますが、絶対に削除しないでください。

## Future
* ドラッグ処理中の処理を軽量化
* Stationalyイベントへの対応
* ドラッグイベントの処理頻度を設定できるように

## インポート方法
* unitypackageファイルをクリックしてUnityプロジェクトへプラグインをインポートしてください。
* インポート時にファイルパスに関するエラーが出ても無視してください。

## 使い方
#### 現在のタッチ状況を取得する
* Unityの標準タッチと同様に、現在のタッチ情報を一括して取得できます。パラメータもレファレンスに合わせたはずなので問題ないかと思います。

```
void Update () 
{
	foreach (var touch in DirectTouch.touches) {
		switch (touch.phase) {
		case TouchPhase.Began:
			Debug.Log ("Began");
			break;
		case TouchPhase.Moved:
			Debug.Log ("Moved");
			break;
		case TouchPhase.Ended:
			Debug.Log ("Ended");
			break;
		}
	}
}
```

#### TouchEventHandler
1.Handler用クラスでTouchEventHandler用のInterfaceを継承します。

```
class Test : MonoBehaviour, IDirectTouchEventHandler { }
```

2.タッチイベントを受け取るHandlerをDirectTouchに登録します。

```
void Start () 
{
	DirectTouch.RegisterEventHandler (this);
}
```

3.各タッチイベントの処理を追加します。falseが返された場合は次のHandlerが実行され、trueが返された場合は以降のHandlerの処理がキャンセルされます。

```
public bool OnTouchEventBegan (DirectTouchInfo[] info)
{
	return false;
}

public bool OnTouchEventEnded (DirectTouchInfo[] info)
{
	return false;
}

public bool OnTouchEventMoved (DirectTouchInfo[] info)
{
	return false;
}

public bool OnTouchEventStationary (DirectTouchInfo[] info)
{
	return false;
}
```

4.Handlerの処理優先度を指定します。Orderの値が大きい順に処理が実行されます。同じOrder値の場合、先に登録されたHandlerが実行されます。

```
public int Order {
	get {
		return 0;
	}
}
```

5.タッチイベントの処理を行うかどうかを返すメソッドを追加します。特定の状況でタッチイベントを発火させたくない時はfalseを返してください

```
public bool Process {
	get {
		return true; //or false
	}
}
```

#### Android, iOS用プラグインの無効化
* AndroidやiOSの実機で、プラグインが原因で問題が発生していると思われる場合は、プラグインを無効化してください。
* また、問題発生時にはissueを上げてくださると助かります。

```
DirectTouch.EnableNativePlugin ();
DirectTouch.DisableNativePlugin ();
```

#### UnityのタッチイベントOFF
* 利益があるとは思えませんが、Unityのデフォルトタッチ機能をOFFにできます。

```
DirectTouch.EnableUnityDefaultTouch ();
DirectTouch.DisableUnityDefaultTouch ();
```

