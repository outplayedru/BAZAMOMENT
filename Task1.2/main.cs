using System;
using System.Collections.Generic;

class Program {
      
    public enum TreeTraversalType {
        dfs,
        bfs
    }

    public class MyGraph<T> {
   
        public MyGraph() {}

        public MyGraph(IEnumerable<T> Nodes, IEnumerable<Tuple<T,T>> edges) {
          // конструктор с инициализацией
            foreach(var Node in Nodes)
                AddNode(Node);

            foreach(var edge in edges)
                AddEdge(edge);
        }

        public Dictionary<T, HashSet<T>> NeighborList { get; } = new Dictionary<T, HashSet<T>>();
        

        public void AddNode(T Node) {
            NeighborList[Node] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T,T> edge) {
            
             if (NeighborList.ContainsKey(edge.Item1) && NeighborList.ContainsKey(edge.Item2)) {
                
                NeighborList[edge.Item1].Add(edge.Item2);
                NeighborList[edge.Item2].Add(edge.Item1);
            }
        }
    }
    
    
    public class Algorithms {
        
        public HashSet<T> DFS<T>(MyGraph<T> Mygraph, T start) {
            var visited = new HashSet<T>();

            if (!Mygraph.NeighborList.ContainsKey(start))
                return visited;
                
            var stack = new Stack<T>();
            stack.Push(start);

            while (stack.Count > 0) {
                var Node = stack.Pop();

                if (visited.Contains(Node))
                    continue;

                visited.Add(Node);

                foreach(var neighbor in Mygraph.NeighborList[Node])
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
            }

            return visited;
        }
    
        public HashSet<T> BFS<T>(MyGraph<T> Mygraph, T start) {
            var visited = new HashSet<T>();

            if (!Mygraph.NeighborList.ContainsKey(start))
                return visited;
                
            
            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0) {
                var Node = queue.Dequeue();

                if (visited.Contains(Node))
                    continue;

                visited.Add(Node);

                foreach(var neighbor in Mygraph.NeighborList[Node])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            return visited;
        }

        public HashSet<T> TreeTraversal<T>
                  (MyGraph<T> MyGraph, T start, TreeTraversalType flag) {
        
          if (flag==TreeTraversalType.dfs) 
              
              return DFS(MyGraph, start);
          else 
              
              return BFS(MyGraph, start);
        }       
    
    }

  public static void Main (string[] args) {

      var Nodes = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

      var edges = new[]{Tuple.Create(1,2), Tuple.Create(1,3),
      Tuple.Create(2,4), Tuple.Create(3,5), Tuple.Create(3,6),
      Tuple.Create(4,7), Tuple.Create(5,7), Tuple.Create(5,8),
      Tuple.Create(5,6), Tuple.Create(8,9), Tuple.Create(9,10)};

      var Mygraph = new MyGraph<int>(Nodes, edges);
      var algorithms = new Algorithms();
  
      
      foreach(var Node in algorithms.TreeTraversal(Mygraph, 1, TreeTraversalType.dfs))
        {
          Console.Write(Node + " ");
        }
      Console.WriteLine();

      foreach(var Node in algorithms.TreeTraversal(Mygraph, 1, TreeTraversalType.bfs))
        {
          Console.Write(Node + " ");
        }
      Console.WriteLine();

  }

}