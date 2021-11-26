using System;

interface IToolKit
{
	string[] GetTools();
}

interface IParts
{
	string[] GetParts();
}

class Chair : IToolKit, IParts
{
	public string[] GetTools()
	{
		return new string[] { "Hammer", "Screwdriver" };
	}

	public string[] GetParts()
	{
		return new string[] { "Legs", "Seat", "Back", "Screws" };
	}
}

class Table : IToolKit, IParts
{
	public string[] GetTools()
	{
		return new string[] { "Hammer", "Screwdriver" };
	}

	public string[] GetParts()
	{
		return new string[] { "Legs", "Top", "Screws" };
	}
}

abstract class FurnitureKit<T> where T : IToolKit, IParts
{
	private T item;

	public FurnitureKit(T item)
	{
		this.item = item;
	}

	public abstract string GetName();

	public abstract string GetColor();

	public void ShowTools()
	{
		Console.WriteLine("Tools: " + string.Join(", ", this.item.GetTools()));
	}

	public void ShowParts()
	{
		Console.WriteLine("Parts: " + string.Join(", ", this.item.GetParts()));
	}
}

class ChairKit : FurnitureKit<Chair>
{
	public ChairKit() : base(new Chair()) {}

	public override string GetName()
	{
		return "Chair";
	}

	public override string GetColor()
	{
		return "White";
	}
}

class TableKit : FurnitureKit<Table>
{
	public TableKit() : base(new Table()) {}

	public override string GetName()
	{
		return "Table";
	}

	public override string GetColor()
	{
		return "Dark Brown";
	}
}

class Program
{
	public static void Main (string[] args)
	{
		ChairKit chair = new ChairKit();
		Console.WriteLine("Item: " + chair.GetColor() + " " + chair.GetName());
		chair.ShowTools();
		chair.ShowParts();

		Console.WriteLine();

		TableKit table = new TableKit();
		Console.WriteLine("Item: " + table.GetColor() + " " + table.GetName());
		table.ShowTools();
		table.ShowParts();
	}
}