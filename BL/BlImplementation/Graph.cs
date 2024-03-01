namespace BlImplementation;

class Graph
{
    private int V; // number of vertices
    private List<int>[] adj; // adjacency list

    public Graph(int V)
    {
        this.V = V;
        adj = new List<int>[V];
        for (int i = 0; i < V; i++)
        {
            adj[i] = new List<int>();
        }
    }

    public void AddEdge(int v, int w) { adj[v].Add(w); }

    private bool IsCycle(int v, bool[] visited, bool[] recStack)
    {
        visited[v] = true;
        recStack[v] = true;

        foreach (var neighbor in adj[v])
        {
            if (!visited[neighbor])
            {
                if (IsCycle(neighbor, visited, recStack))
                    return true; // Cross edge found in the subtree rooted at 'neighbor'
            }
            else if (recStack[neighbor])
                return true; // Cross edge found between 'v' and 'neighbor'
        }

        recStack[v] = false;
        return false;
    }

    // Function to detect cross edges in the graph
    public bool DetectCycle()
    {
        bool[] visited = new bool[V];
        bool[] recStack = new bool[V];

        for (int i = 0; i < V; ++i)
            if (!visited[i])
                if (IsCycle(i, visited, recStack))
                    return true; // Cross edge found

        return false; // No cross edges found
    }

    private void TopologicalSortUtil(int v, bool[] visited, List<int> result)
    {
        // Mark the current node as visited
        visited[v] = true;

        // Recur for all the vertices adjacent to this vertex
        foreach (var i in adj[v])
            if (!visited[i])
                TopologicalSortUtil(i, visited, result);

        // Push current vertex to stack which stores result
        result.Add(v);
    }

    // The function to do Topological Sort. It uses recursive TopologicalSortUtil
    public List<int> TopologicalSort()
    {
        List<int> result = new List<int>();

        // Mark all the vertices as not visited
        bool[] visited = new bool[V];
        for (int i = 0; i < V; i++)
            visited[i] = false;

        // Call the recursive helper function to store Topological Sort starting from all vertices one by one
        for (int i = 0; i < V; i++)
            if (!visited[i])
                TopologicalSortUtil(i, visited, result);

        return result;
    }
}