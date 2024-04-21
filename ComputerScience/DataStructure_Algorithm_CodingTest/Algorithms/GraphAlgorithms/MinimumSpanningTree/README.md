# 최소 신장 트리(MST, Minimum Spanning Tree)

## 최소 신장 트리란?
 최소 신장 트리는 그래프 내의 모든 정점을 포함하면서 사이클이 존재하지 않는 부분 그래프입니다. 이때, 부분 그래프의 간선의 가중치 합이 최소가 되는 트리를 말합니다.

## 최소 신장 트리 특징
 ### 장점
 * 모든 노드를 연결하는데 필요한 간선의 가중치 합이 최소입니다.
 * 네트워크에서 모든 노드를 가장 적은 비용으로 연결할 수 있습니다. (최적화)
 * 최단 경로를 찾는 등 여러 그래프 문제에 활용 가능

 ### 단점
 * 그래프가 밀집되어 있을 경우에는 시간과 메모리를 많이 사용할 수 있음
 * 모든 노드가 최소 신장 트리에 포함되어 있지만, 일부 노드 사이의 직접적인 연결이 없을 수 있습니다.
 
## 최소 신장 트리의 종류

### 크루스칼(Kruskal) 알고리즘
#### 크루스칼 알고리즘이란?
 크루스칼 알고리즘은 간선의 가중치를 기준으로 정렬한 후, 가장 작은 가중치부터 시작하여 사이클을 형성하지 않는 간선을 추가해가며 최소 신장 트리를 구성하는 알고리즘입니다.

#### 과정
1. 간선을 가중치에 따라 오름차순으로 정렬한다.
2. 정렬된 간선 리스트에서 가장 가중치가 작은 간선을 선택한다.
3. 선택한 간선이 사이클을 형성하지 않으면 해당 간선을 최소 신장 트리에 추가한다.
4. 모든 정점이 연결될 때까지 2~3을 반복한다.

### 프림(Prim) 알고리즘
#### 프림 알고리즘이란?
 프림 알고리즘은 하나의 정점에서 시작하여 해당 정점과 연결된 간선 중 최소 가중치를 가진 간선을 선택하고, 선택된 간선의 도착점을 새로운 정점으로 추가해가며 최소 신장 트리를 구성하는 알고리즘입니다.

#### 과정
1. 임의의 시작 정점을 선택한다.
2. 선택한 정점과 연결된 간선 중 최소 가중치를 가진 간선을 선택하여 최소 신장 트리에 추가한다.
3. 추가된 정점과 연결된 간선 중 이미 선택된 정점과 연결되지 않은 간선 중 최소 가중치를 가진 간선을 선택하여 최소 신장 트리에 추가한다.
4. 모든 정점이 선택될 때까지 2~3을 반복한다.

## 최소 신장 트리의 시간복잡도
* 크루스칼 알고리즘: O(E log E), E는 간선의 개수
* 프림 알고리즘: O(V^2) 또는 O(E log V) (우선순위 큐를 사용할 경우), V는 정점의 개수
 
## 최소 신장 트리의 종류별 예시코드 c++

### 크루스칼
```cpp
#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

// 간선 구조체
struct Edge 
{
    int src, dest, weight;
    Edge(int s, int d, int w) : src(s), dest(d), weight(w) {}
};

// 부모 노드를 찾는 함수
int FindParent(int v, vector<int>& parent) 
{
    if (parent[v] == -1)
        return v;
    return FindParent(parent[v], parent);
}

// 두 부모 노드를 합치는 함수 
void UnionSets(int u, int v, vector<int>& parent) 
{
    int uSet = FindParent(u, parent);
    int vSet = FindParent(v, parent);
    parent[uSet] = vSet;
}

// 크루스칼 알고리즘 함수
void KruskalMST(vector<Edge>& edges, int V) 
{
    // 간선을 가중치에 따라 오름차순으로 정렬
    sort(edges.begin(), edges.end(), [](Edge& a, Edge& b) 
    {
        return a.weight < b.weight;
    });

    vector<int> parent(V, -1); // 각 정점의 부모 노드 배열
    vector<Edge> MST; // 최소 신장 트리를 저장할 배열

    for (int i = 0; i < edges.size(); ++i) 
    {
        Edge& edge = edges[i];
        // 사이클을 형성하지 않는 경우에만 간선을 추가
        if (FindParent(edge.src, parent) != FindParent(edge.dest, parent)) 
        {
            MST.push_back(edge);
            UnionSets(edge.src, edge.dest, parent);
        }
    }

    // 최소 신장 트리 출력
    cout << "최소 신장 트리 간선:" << endl;
    for (int i = 0; i < MST.size(); ++i) 
    {
        cout << MST[i].src << " - " << MST[i].dest << " : " << MST[i].weight << endl;
    }
}

int main() 
{
    int V = 6; // 정점의 수
    vector<Edge> edges; // 간선 리스트

    // 간선 추가
    edges.push_back(Edge(0, 1, 4));
    edges.push_back(Edge(0, 2, 2));
    edges.push_back(Edge(1, 2, 5));
    edges.push_back(Edge(1, 3, 10));
    edges.push_back(Edge(2, 4, 3));
    edges.push_back(Edge(3, 5, 1));
    edges.push_back(Edge(4, 3, 2));
    edges.push_back(Edge(4, 5, 5));

    // 크루스칼 알고리즘 호출
    KruskalMST(edges, V);

    return 0;
}
```

### 프림
```cpp
#include <iostream>
#include <vector>
#include <queue>

using namespace std;

// 정점과 간선을 나타내는 구조체
struct Node 
{
    int vertex, weight;
    Node(int v, int w) : vertex(v), weight(w) {}
};

// 우선순위 큐의 비교 함수 정의
struct CompareNode 
{
    bool operator()(const Node& a, const Node& b) 
    {
        return a.weight > b.weight;
    }
};

// 프림 알고리즘 함수
void PrimMST(vector<vector<pair<int, int>>>& graph, int V) 
{
    vector<bool> visited(V, false); // 정점의 방문 여부 배열
    priority_queue<Node, vector<Node>, CompareNode> pq; // 우선순위 큐
    int totalWeight = 0; // 최소 신장 트리의 가중치 합

    // 임의의 시작 정점을 0번으로 설정
    pq.push(Node(0, 0));

    while (!pq.empty()) 
    {
        Node curNode = pq.top();
        pq.pop();
        int u = curNode.vertex;
        int weight = curNode.weight;

        // 이미 방문한 정점은 무시
        if (visited[u])
            continue;

        visited[u] = true;
        totalWeight += weight;

        // 현재 정점과 연결된 간선을 우선순위 큐에 추가
        for (int i = 0; i < graph[u].size(); ++i)
        {
            int v = graph[u][i].first;
            int w = graph[u][i].second;
            if (!visited[v])
                pq.push(Node(v, w));
        }
    }

    // 최소 신장 트리의 가중치 합 출력
    cout << "최소 신장 트리의 가중치 합: " << totalWeight << endl;
}

int main() 
{
    int V = 6; // 정점의 수
    vector<vector<pair<int, int>>> graph(V); // 그래프

    // 간선 추가
    graph[0].push_back({1, 4});
    graph[0].push_back({2, 2});
    graph[1].push_back({0, 4});
    graph[1].push_back({2, 5});
    graph[1].push_back({3, 10});
    graph[2].push_back({0, 2});
    graph[2].push_back({1, 5});
    graph[2].push_back({4, 3});
    graph[3].push_back({1, 10});
    graph[3].push_back({5, 1});
    graph[4].push_back({2, 3});
    graph[4].push_back({3, 2});
    graph[4].push_back({5, 5});
    graph[5].push_back({3, 1});
    graph[5].push_back({4, 5});

    // 프림 알고리즘 호출
    PrimMST(graph, V);

    return 0;
}
```