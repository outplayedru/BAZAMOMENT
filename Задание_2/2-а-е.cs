using System;
using System.Linq;
using System.Collections.Generic;

struct Vec2i
{
	public int X { get; }
	public int Y { get; }

	public Vec2i(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public override string ToString()
	{
		return $"Vec2i({this.X}, {this.Y})";
	}
}

struct Vec2id
{
	public int X { get; }
	public double Y { get; }

	public Vec2id(int x, double y)
	{
		this.X = x;
		this.Y = y;
	}

	public override string ToString()
	{
		return $"Vec2id({this.X}, {this.Y})";
	}
}

struct Vec2di
{
	public double X { get; }
	public int Y { get; }

	public Vec2di(double x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public override string ToString()
	{
		return $"Vec2di({this.X}, {this.Y})";
	}
}

struct Vec3<T>
{
	public T X { get; }
	public T Y { get; }
	public T Z { get; }

	public Vec3(T x, T y, T z)
	{
		this.X = x;
		this.Y = y;
		this.Z = z;
	}

	public override string ToString()
	{
		return $"({this.X}, {this.Y}, {this.Z})";
	}
}

class Program
{
	public static void Main (string[] args)
	{
		int[] intArray = new int[] { 92, 97, 81, 60 };
		Console.WriteLine(aLinq(intArray));
		Console.WriteLine(bLinq(intArray));

		Console.WriteLine(cLinq(new Vec2i[] { new Vec2i(10, 20), new Vec2i(300, 543), new Vec2i(38, 21) }));
		
		Console.WriteLine(string.Join(", ", dLinq(new Vec2id[] { new Vec2id(10, 20.0), new Vec2id(300, 543.0), new Vec2id(38, 21.0) })));

		Console.WriteLine(string.Join(", ", e(new int[] { 5, 10, 13, 14 }, new int[] { 43, 60, 3 })));

		Console.WriteLine(string.Join(", ", fLinq(new string[] { "Кот", "собака", "тот", "слово", "вот" })));

		// Console.WriteLine(string.Join(", ", kLinq<string>(new string[] { "a", "b", "c" }, new string[] { "d", "e" }, new string[] { "f", "g" })));

		Console.WriteLine(kLinq<string>(new string[] { "a", "b", "c" }, new string[] { "d", "e" }, new string[] { "f", "g" }));
	}

	public static int a(int[] ints)
	{
		int max = ints[0];

		foreach(int e in ints)
		{
			if(e > max)
			{
				max = e;
			}
		}

		return max;
	}

	public static int aLinq(int[] ints)
	{
		return ints.Max();
	}

	public static int b(int[] ints)
	{
		int maxIndex = 0;

		for(int i = 0; i < ints.Length; ++i)
		{
			if(ints[i] > ints[maxIndex])
			{
				maxIndex = i;
			}
		}

		return maxIndex;
	}

	public static int bLinq(int[] ints)
	{
		return ints
			.Select((val, index) => new { val, index })
			.Aggregate((e1, e2) => e1.val > e2.val ? e1 : e2).index;
	}

	public static Vec2i c(Vec2i[] vecs)
	{
		Vec2i maxY = vecs[0];

		foreach(Vec2i v in vecs)
		{
			if(v.Y > maxY.Y)
			{
				maxY = v;
			}
		}

		return maxY;
	}

	public static Vec2i cLinq(Vec2i[] vecs)
	{
		return vecs
			.Aggregate((v1, v2) => v1.Y > v2.Y ? v1 : v2);
	}

	public static Vec2di[] d(Vec2id[] vecs)
	{
		Vec2di[] res = new Vec2di[vecs.Length];

		for(int i = 0; i < vecs.Length; ++i)
		{
			res[i] = new Vec2di(vecs[i].Y, vecs[i].X);
		}

		Array.Sort(res, (v1, v2) => v1.Y.CompareTo(v2.Y));

		return res;
	}

	public static Vec2di[] dLinq(Vec2id[] vecs)
	{
		return vecs
			.OrderBy(vec => vec.Y)
			.Select(vec => new Vec2di(vec.Y, vec.X))
			.ToArray();
	}

	public static Vec2i[] e(int[] ints1, int[] ints2)
	{
		List<Vec2i> res = new List<Vec2i>();

		foreach(int i1 in ints1)
		{
			foreach(int i2 in ints2)
			{
				if(i1 % 5 == 0 && i2 % 5 == 0)
				{
					res.Add(new Vec2i(i1, i2));
				}
			}
		}

		return res.ToArray();
	}

	public static Vec2i[] eLinq(int[] ints1, int[] ints2)
	{
		return (
			from i1 in ints1
			from i2 in ints2
			where i1 % 5 == 0
			where i2 % 5 == 0
			select new Vec2i(i1, i2)
		)
		//.Where(vec => vec.X % 5 == 0 && vec.Y % 5 == 0)
		.ToArray();
	}

	public static string[] f(string[] strs)
	{
		List<string> res = new List<string>();

		foreach(string s in strs)
		{
			if(s.ToLower().Contains("от"))
			{
				res.Add(s);
			}
		}

		res.Sort();

		return res.ToArray();
	}

	public static string[] fLinq(string[] strs)
	{
		return strs
			.Where(s => s.ToLower().Contains("от"))
			.OrderBy(s => s)
			.ToArray();
	}

	public static string kLinq<T>(T[] arr1, T[] arr2, T[] arr3)
	{
		return (
			from o1 in arr1
			from o2 in arr2
			from o3 in arr3
			select new Vec3<T>(o1, o2, o3)
		).Select(a => a.ToString())
		.Aggregate((a, b) => a + ", " + b);
	}
}