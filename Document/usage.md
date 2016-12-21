# PropertyWriterの使い方

データ入力したいデータ型が定義されているプロジェクトに`[PwProject]`属性を付けた型を定義し、
そこにデータ入力したいデータ型をプロパティとして定義し、そのプロパティに`[PwMaster]`属性を付けてください。

```csharp
[PwProject]
class MyProject
{
    [PwMaster]
    public int[] Data { get; set; }

    [PwMaster(key: "Ref")]
    public Hoge[] Referencable { get; set; }
}
```

`[PwProject]`が付いた型を**プロジェクト型**、`[PwMaster]`が付いたプロパティを**マスター プロパティ**と呼びます。

## 対応型

* int
* float
* bool
* string
* 列挙型
* 配列
* 上記の型を組み合わせたクラス/構造体
    * 引数なしコンストラクタが定義されている必要があります
    * データ入力したいプロパティに`[PwMember]`属性を付ける必要があります

## 高度な機能

### 他のマスターをintのIDで参照する

リストで管理されているデータの中から１つをIDで参照する機能があります。

参照元のプロパティに`[PwReferenceMember]`属性を付与してください。
引数は以下のとおりです：

```csharp
[PwReferenceMember(masterKey: "参照先のMasterに設定したKey", IdFieldName: "参照先の型のIDを表すプロパティ名")]
```

例えば、PwMaster属性のkeyに"Ref"が指定されたマスターを参照するなら以下のようにしてください。

```csharp
class MySubData
{
    [PwReferenceMember(masterKey: "Ref", IdFieldName: nameof(Hoge.Id))]
    public int Reference { get; set; }
}
```

### 継承関係のあるクラスのいずれかを選んで入力する

1つの基底クラスと、複数のその派生クラスがあるとき、どの実装クラスを入力するか選ぶことができます。

基底クラスに`[PwSubtyping]`属性を付けてください。派生クラスに`[PwSubtyped]`属性を付けてください。
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

[PwSubtyped]
class SubtypedClassA
{
    [PwMember]
    public int Y { get; set; }
}

class SubtypedClassB
{
    [PwMember]
    public int Z { get; set; }
}

class MyData
{
    [PwMember]
    public SubtypingClass Subtyping { get; set; }
}
```

### 複数行入力できるstring

string型のプロパティに、`[PwMember]`の代わりに`[PwMultiLineTextMember]`を付与すると、
複数行に渡る入力をサポートした文字列入力フィールドが表示できます。

### シリアライズの方式をカスタマイズ可能

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
