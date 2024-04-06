# 그래프 (Graph)

## 그래프란
 그래프는 각각의 요소들이 서로 '연결'되어 있는 자료 구조입니다. 이러한 연결은 정점(Vertex)과 간선(Edge)으로 이루어져 있습니다. 정점은 그래프의 노드를 나타내고, 간선은 정점 간의 관계를 나타냅니다.

### 주요 용어
* 정점(Vertex): 그래프의 노드로, 데이터를 저장하는 요소입니다.
* 간선(Edge): 정점과 정점을 연결하는 선으로, 두 정점 사이의 관계를 나타냅니다.
* 인접 정점(Adjacent Vertex): 간선으로 직접 연결된 정점을 의미합니다.
* 가중치(Weight): 간선에 할당된 값으로, 두 정점 간의 거리나 비용을 나타냅니다.

## 그래프 특징
 ### 장점
 * 네트워크, 경로, 흐름 등의 복잡한 관계를 표현할 수 있습니다.
 * 다양한 그래프 알고리즘을 적용하여 최적의 솔루션을 찾을 수 있습니다.

 ### 단점
 * 그래프는 크기가 크고 복잡할 경우 계산 비용이 매우 높을 수 있습니다.
 * 일부 그래프 알고리즘은 최적 솔루션을 찾지 못할 수 있습니다.

## 그래프의 종류
* 무방향 그래프(Undirected Graph): 간선에 방향이 없는 그래프로, 간선을 통해 양방향으로 이동할 수 있습니다.
* 방향 그래프(Directed Graph): 간선에 방향이 있는 그래프로, 간선은 한 방향으로만 이동할 수 있습니다.
* 가중치 그래프(Weighted Graph): 간선에 가중치가 할당된 그래프로, 간선의 가중치에 의미가 있습니다.
* 사이클 그래프(Cyclic Graph): 그래프 내에 사이클이 존재하는 경우를 말합니다.
* 비순환 그래프(Acyclic Graph): 그래프 내에 사이클이 없는 경우를 말합니다.

## 그래프 구현 방법
그래프는 주로 두 가지 방법으로 구현됩니다.
### 인접 리스트(Adjacency List)
인접 리스트는 그래프의 각 정점마다 인접한 정점들을 연결 리스트로 나타내는 방식입니다. 각 정점마다 해당 정점과 연결된 정점들의 리스트를 저장합니다. 이 방식은 각 정점에 연결된 간선의 수에 비례하는 공간을 차지하므로 희소 그래프에 유리합니다.

		희소 그래프(sparse graph)란 간선의 개수가 적은 그래프로 노드 개수보다 간선 개수가 적으면 희소 그래프라 합니다.

### 인접 행렬(Adjacency Matrix)
인접 행렬은 그래프의 각 정점을 행과 열로 나타내는 행렬을 사용하여 그래프를 표현하는 방식입니다. 행렬의 각 원소는 해당 정점 사이의 간선 여부를 나타냅니다. 이 방식은 두 정점 사이의 연결 여부를 상수 시간에 확인할 수 있지만, 밀집 그래프의 경우 많은 메모리를 차지할 수 있습니다.

		밀집 그래프(Dense Graph)란 간선의 개수가 많은 그래프로 노드 개수보다 간선 개수가 많으면 밀집 그래프라 합니다.

## 그래프의 활용
그래프는 다양한 문제를 해결하는 데 사용됩니다. 깊이 우선 탐색(DFS, Depth-First Search)과 너비 우선 탐색(BFS, Breadth-First Search), 최단 경로 알고리즘, 최소 신장 트리 알고리즘, 네트워크 플로우 등 다양한 알고리즘이 존재합니다.

## 그래프 구현 코드
### 인접 리스트 그래프
 각 정점마다 연결된 간선들을 리스트로 저장하는 방식
```cpp
#include <iostream>
#include <vector>

using namespace std;

class Graph 
{
private:
    int numVertices;
    vector<vector<int>> adjList;

public:
    Graph(int V) : numVertices(V), adjList(V) {}

    void addEdge(int src, int dest) 
    {
        adjList[src].push_back(dest);
        adjList[dest].push_back(src); // 무방향 그래프일 경우 추가
    }

    void display() 
    {
        for (int i = 0; i < numVertices; ++i) 
        {
            cout << i << " -> ";
            for (int j : adjList[i]) 
            {
                cout << j << " ";
            }
            cout << endl;
        }
    }
};

int main() 
{
    Graph g(3);
    g.addEdge(0, 1);
    g.addEdge(1, 2);
    g.display();
    return 0;
}
```

### 인접행렬 그래프
 각 정점을 행과 열로 나타내는 행렬을 사용하여 그래프를 표현
```cpp
#include <iostream>
#include <vector>

using namespace std;

class Graph 
{
private:
    int numVertices;
    vector<vector<int>> adjMatrix;

public:
    Graph(int V) : numVertices(V), adjMatrix(V, vector<int>(V, 0)) {}

    void addEdge(int src, int dest) 
    {
        adjMatrix[src][dest] = 1;
        adjMatrix[dest][src] = 1; // 무방향 그래프일 경우
    }

    void display() 
    {
        for (int i = 0; i < numVertices; ++i) 
        {
            for (int j = 0; j < numVertices; ++j) 
            {
                cout << adjMatrix[i][j] << " ";
            }
            cout << endl;
        }
    }
};

int main() 
{
    Graph g(3);
    g.addEdge(0, 1);
    g.addEdge(1, 2);
    g.display();
    return 0;
}
```

### 가중치 그래프
 간선에 가중치가 할당된 그래프
```cpp
#include <iostream>
#include <vector>

using namespace std;

class Graph 
{
private:
    int numVertices;
    vector<vector<pair<int, int>>> adjList;

public:
    Graph(int V) : numVertices(V), adjList(V) {}

    void addEdge(int src, int dest, int weight) 
    {
        adjList[src].push_back({dest, weight});
        adjList[dest].push_back({src, weight}); // 무방향 그래프일 경우
    }

    void display() 
    {
        for (int i = 0; i < numVertices; ++i) 
        {
            cout << i << " -> ";
            for (auto& edge : adjList[i]) 
            {
                cout << edge.first << "(" << edge.second << ") ";
            }
            cout << endl;
        }
    }
};

int main() 
{
    Graph g(3);
    g.addEdge(0, 1, 5);
    g.addEdge(1, 2, 7);
    g.display();
    return 0;
}
```