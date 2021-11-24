using System;
using System.IO;
using System.Text;

class Program {

 	static string GeometryLog = "./geometry.log";
	static string TriAndQuadLog = "./tri-n-quad.log";
  
  public static void Log( GeometryException exc, string FileName ) {
  	
    StringBuilder sb = new StringBuilder();
		sb.Append( $"{exc.Timing} :\n\t{exc.Message},\n\tПараметры: {sb}" );
		sb.AppendJoin(",", exc.Parameters);
		sb.Append('\n');

		File.AppendAllText(FileName, sb.ToString(), Encoding.UTF8);
    }


  // базовый класс исключений
	public class GeometryException : ArgumentException
	{
    // задаем массив (ТЗ)
  	public Int32[] Parameters { get; private set; }
    public DateTime Timing { get; }
		
		// конструктор для исключения
    public GeometryException(string message, Int32[] arg) 
    : base(message)
      { 
        Parameters = arg; 
        Timing = DateTime.UtcNow; 
      }
	}



  // производные классы
	public class TriangleException :  GeometryException
	{
		public TriangleException(string message, params Int32[] arg) 
    : base(message, arg) {}
	}

	public class QuadrangleException :  GeometryException
	{
		public QuadrangleException(string message, params Int32[] arg) 
    : base(message, arg) {}
	}

	public class CircleException :  GeometryException
	{
		public CircleException(string message, params Int32[] arg) 
    : base(message, arg) {}
	}

  
  
  public class Triangle
	{
		public Triangle(Int32 a, Int32 b, Int32 c)
		{
			// проверяем данные, создаем исключение для ошибочных
			if (a <= 0 || b <= 0 || c <= 0)
			{
				throw new TriangleException("Длина каждой из сторон треугольника должна быть больше 0",
					a, b, c);
			}

			if (a + b <= c || a + c <= b || b + c <= a)
			{
				throw new TriangleException("Невозможно создать треугольник с заданными длинами сторон",
					a, b, c);
			}
		}
	}

	public class Quadrangle
	{
		public Quadrangle(Int32 a, Int32 b, Int32 c, Int32 d)
		{
			// проверяем данные, создаем исключение для ошибочных
      if (a <= 0 || b <= 0 || c <= 0 || d <= 0)
			{
				throw new QuadrangleException(
					"Длина каждой из сторон четырехугольника должна быть больше 0", a, b, c, d);
			}

			if (a + b + c <= d || a + b + d <= c || a + c + d <= b || b + c + d <= a)
			{
				throw new QuadrangleException(
					"Невозможно создать четырехугольник с заданными длинами сторон", a, b, c, d);
			}
		}
	}

	public class Circle
	{
		public Circle(Int32 r)
		{
			// проверяем данные, создаем исключение для ошибочных
			if (r <= 0)
			{
				throw new CircleException("Радиус окружности должен быть больше 0", r);
			}
		}
	}


		static Triangle CreateTriangle(Random rndm)
		{
			Int32 a = rndm.Next(-15, 15);
			Int32 b = rndm.Next(-15, 15);
			Int32 c = rndm.Next(-15, 15);
      try 
      {
        return new Triangle(a, b, c);
      }
      catch (TriangleException exc)  // перехватываем производное исключение
      {
        Log(exc, TriAndQuadLog);     // пишем в свой лог
        throw;                      // возвращаем в стек
      }
    }

		static Quadrangle CreateQuadrangle(Random rndm)
		{
      
      Int32 a = rndm.Next(-15, 15);
			Int32 b = rndm.Next(-15, 15);
			Int32 c = rndm.Next(-15, 15);
			Int32 d = rndm.Next(-15, 15);
      try
      {
			  return new Quadrangle(a, b, c, d);
      }
      catch (QuadrangleException exc)  // перехватываем производное исключение
      {
        Log(exc, TriAndQuadLog);       // пишем в свой лог
        throw;                        // возвращаем в стек
      }
    }

  	static Circle CreateCircle(Random rndm)
		{
      try
      {
        return new Circle(rndm.Next(-15, 15)); 
      }
      catch (CircleException)       // перехватываем
      {
        throw;                      // возвращаем в стек
      }
    }


		static void Main()
		{
			
			
			// генерация числа в нужном для нас диапазоне
      Random rndm = new Random();

			for (int i = 0; i < 50; i++)
			{
				try
				{
					var op = rndm.Next(0, 3);
					switch (op)
					{
						case 0:
							_ = CreateCircle(rndm);
							Console.WriteLine("Окружность создана.");
							break;
						case 1:
							_ = CreateTriangle(rndm);
							Console.WriteLine("Треугольник создан.");
							break;
						case 2:
							_ = CreateQuadrangle(rndm);
							Console.WriteLine("Четырехугольник создан.");
							break;
					}
				}
				catch (GeometryException exc)  // перехватываем базовое исключение
				{
          Log(exc, GeometryLog);       // пишем в базовый лог
				}
        
			}
		}

}