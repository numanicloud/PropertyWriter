# 高度な機能

## 継承関係のあるクラスのいずれかを選んで入力する

1つの基底クラスと、複数のその派生クラスがあるとき、その基底クラスのプロパティに対してどの実装クラスを入力するか選ぶドロップダウンリストを提供することができます。

基底クラスに`[PwSubtyping]`属性を付けてください。派生クラスに`[PwSubtype]`属性を付けてください。
いずれかのクラスが`[PwSubtyping]`が付いている型のプロパティを`[PwMember]`で指定したとき、
そこに必要な派生クラスを選んで入力することができます。

例：

```csharp
[PwSubtyping]
class SubtypingClass
{
    [PwMember]
    public int X { get; set; }
}

[PwSubtype]
class SubtypeA : SubtypingClass
{
    [PwMember]
    public int Y { get; set; }
}

[PwSubtype]
class SubtypeB : SubtypingClass
{
    [PwMember]
    public int Z { get; set; }
}

class MyData
{
    // このプロパティがドロップダウンリストになる
    [PwMember]
    public SubtypingClass Subtyping { get; set; }
}
```

## 複数行入力できるstring

string型のプロパティに、`[PwMember]`の代わりに`[PwMultiLineTextMember]`を付与すると、
複数行に渡る入力をサポートした文字列入力フィールドが表示できます。

## シリアライズの方式をカスタマイズ可能

デフォルトでは`DataContractJsonSerializer`によってjsonでデータが保存されますが、形式をカスタマイズできます。

プロジェクト型にシリアライズ用の静的メソッドを定義し、`[PwSerializer]`属性を付けてください。
同様にデシリアライズ用の静的メソッドを定義し、`[PwDeserializer]`属性を付けてください。
以下のような引数・戻り値にしてください。メソッド名は自由でかまいません。

```csharp
// 第一引数は自分自身の型
[PwSerializer]
public static async Task Serialize(MyProject data, Stream stream)
{
    // シリアライズ処理
}

[PwDeserializer]
public static async Task<MasterProject> Deserialize(Stream stream)
{
    // デシリアライズ処理
}
```

引数に渡されてくるStreamクラスに対して、データが格納されるファイルへの読み書き処理を行えます。

## プラグイン機能

ユーザーがプラグインを開発し、PropertyWriterの機能を拡張できます。詳細は後程。

* 特定の型に専用のUIをデザインすることができます。
