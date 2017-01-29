using System;
using System.IO;
using Newtonsoft.Json;
using PropertyWriter.Annotation;

[PwProject]
public class MyProject
{
	[PwMaster]
	public int Data { get; set; }
	[PwMaster]
	public Hoge Hoge { get; set; }
}

public class Hoge
{
	[PwMember]
	public int X { get; set; }
	[PwMember]
	public string Message { get; set; }
	[PwMember]
	public bool Check { get; set; }
}

class Program
{
	static void Main(string[] args)
	{
		string json;
		using (var file = new StreamReader("BasicSample.json"))
		{
			json = file.ReadToEnd();
		}
		var obj = JsonConvert.DeserializeObject<MyProject>(json);

		Console.WriteLine(obj.Data);
		Console.WriteLine(obj.Hoge.X);
		Console.WriteLine(obj.Hoge.Message);
		Console.WriteLine(obj.Hoge.Check);
		Console.ReadKey();
	}
}
