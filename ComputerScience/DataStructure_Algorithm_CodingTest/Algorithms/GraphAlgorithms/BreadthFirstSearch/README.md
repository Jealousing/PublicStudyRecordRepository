# 너비 우선 탐색(BFS, Breadth-First Search)

## 너비 우선 탐색이란?
 너비 우선 탐색(BFS, Breadth-First Search)은 그래프의 모든 정점을 방문하는 방법 중 하나로, 시작 정점에서 가까운 정점부터 방문하는 방식입니다.

### 너비 우선 탐색의 동작 원리
1. 시작 정점을 방문하고, 인접한 모든 정점들을 방문합니다.
2. 다음 단계로 이동하기 전에 해당 단계에 연결된 모든 정점을 방문합니다.
3. 방문하지 않은 정점이 남아있는 경우, 이러한 과정을 반복합니다.
4. 모든 정점을 방문할 때까지 이러한 과정을 반복합니다.

## 너비 우선 탐색의 특징
 ### 장점
* 최단 경로: BFS는 시작 노드에서 가장 가까운 노드부터 방문하기 때문에, 최단 경로를 찾는 문제에 적합합니다. 특히, 가중치가 없는 그래프에서의 최단 경로를 찾을 때 유용합니다.
* 직관성: 큐를 사용하여 구현하므로 직관적으로 이해하기 쉽습니다.
* 모든 노드 방문: BFS는 그래프의 모든 노드를 방문하므로, 그래프의 전체적인 구조를 파악하는 데 유용합니다.

 ### 단점
* 메모리 사용량: BFS는 방문할 노드를 큐에 저장하므로, 메모리 사용량이 높을 수 있습니다. 특히, 그래프의 노드 수가 많을 경우 메모리 부족 문제가 발생할 수 있습니다.
* 시간 복잡도: BFS의 시간 복잡도는 O(V+E)입니다(V는 정점의 수, E는 간선의 수). 따라서, 그래프의 크기가 크면 BFS의 수행 시간도 증가합니다.
* 가중치 있는 그래프: BFS는 가중치가 있는 그래프에서 최단 경로를 찾는 데에는 적합하지 않습니다. 이 경우, 다익스트라 알고리즘 등 다른 알고리즘이 더 적합합니다.
 
## 시간복잡도
 너비 우선 탐색의 시간 복잡도는 정점의 수를 V, 간선의 수를 E라고 할 때, O(V+E)입니다.

## 예시코드 c++
```cpp
#include <iostream>
#include <queue>
#include <vector>
using namespace std;

class Graph 
{
private:
    int V; // 정점의 개수
    vector<vector<int>> adj; // 인접 리스트

public:
    // 그래프 생성자
    Graph(int V) : V(V) 
    {
        adj.resize(V);
    }

    // 간선 추가
    void AddEdge(int v, int w) 
    {
        adj[v].push_back(w); // v의 인접 리스트에 w를 추가
    }

    // BFS 시작
    void BFS(int start) 
    {
        // 방문 여부를 저장하는 배열
        vector<bool> visited(V, false);

        // 큐를 이용한 BFS 구현
        queue<int> q;
        visited[start] = true;
        q.push(start);

        while (!q.empty()) 
        {
            int u = q.front();
            cout << u << " ";
            q.pop();

            // 인접한 정점들을 방문
            for (int i = 0; i < adj[u].size(); ++i) 
            {
                int v = adj[u][i];
                if (!visited[v]) 
                {
                    visited[v] = true;
                    q.push(v);
                }
            }
        }
    }
};

int main() 
{
    Graph g(4);
    g.AddEdge(0, 1);
    g.AddEdge(0, 2);
    g.AddEdge(1, 2);
    g.AddEdge(2, 0);
    g.AddEdge(2, 3);
    g.AddEdge(3, 3);

    cout << "너비 우선 탐색 순서: ";
    g.BFS(2); // 정점 2에서 너비 우선 탐색 시작

    return 0;
}
```
## 사용예시 & 코딩테스트 유형
너비 우선 탐색은 그래프 탐색 문제와 관련하여 코딩 테스트에서 자주 출제됩니다. 특히, 최단 경로를 찾거나 그래프의 구조를 분석하는 문제에 적합합니다.
또한, 큐를 사용하는 방식과 최단 경로를 보장한다는 특성으로 인해 BFS 관련 문제는 코딩 테스트에서도 자주 등장합니다.

## 최단 경로찾기 예시 코드

```cpp
#include <iostream>
#include <queue>
#include <vector>
using namespace std;

class Graph 
{
private:
    int V; // 정점의 개수
    vector<vector<int>> adj; // 인접 리스트

public:
    // 그래프 생성자
    Graph(int V) : V(V) 
    {
        adj.resize(V);
    }

    // 간선 추가
    void AddEdge(int v, int w) 
    {
        adj[v].push_back(w); // v의 인접 리스트에 w를 추가
    }

    // 최단 경로 탐색을 위한 BFS
    vector<int> ShortestPath(int start) 
    {
        // 방문 여부를 저장하는 배열
        vector<bool> visited(V, false);
        vector<int> distance(V, -1); // 최단 거리를 저장하는 배열
        vector<int> parent(V, -1); // 최단 경로를 역추적하기 위한 배열

        // 큐를 이용한 BFS 구현
        queue<int> q;
        visited[start] = true;
        distance[start] = 0;
        q.push(start);

        while (!q.empty()) 
        {
            int u = q.front();
            q.pop();

            // 인접한 정점들을 방문
            for (int i = 0; i < adj[u].size(); ++i) 
            {
                int v = adj[u][i];
                if (!visited[v]) 
                {
                    visited[v] = true;
                    distance[v] = distance[u] + 1;
                    parent[v] = u;
                    q.push(v);
                }
            }
        }

        return distance;
    }

    // 최단 경로 출력
    void PrintShortestPath(int start, int end) 
    {
        vector<int> distance = ShortestPath(start);
        if (distance[end] == -1) 
        {
            cout << "No path" << endl;
            return;
        }

        vector<int> path;
        int current = end;
        while (current != start) 
        {
            path.push_back(current);
            current = parent[current];
        }
        path.push_back(start);

        // 역순으로 출력
        for (int i = path.size() - 1; i >= 0; --i) 
        {
            cout << path[i];
            if (i != 0) cout << " -> ";
        }
        cout << endl;
    }
};

int main() 
{
    Graph g(6);
    g.AddEdge(0, 1);
    g.AddEdge(0, 2);
    g.AddEdge(1, 3);
    g.AddEdge(2, 3);
    g.AddEdge(2, 4);
    g.AddEdge(3, 5);
    g.AddEdge(4, 5);
    g.PrintShortestPath(0, 5);
    return 0;
}

```

### 코드 진행과정
1. 그래프를 생성하고 간선을 추가합니다.
2. 정점 0에서 정점 5까지의 최단 경로를 찾습니다.
3. BFS를 시작합니다. 이 때, 방문 여부를 저장하는 배열 visited와 최단 거리를 저장하는 배열 distance, 그리고 최단 경로를 역추적하기 위한 배열 parent를 초기화합니다.
4. 시작 정점을 큐에 넣고, 방문한 것으로 표시합니다. 그리고 시작 정점까지의 거리를 0으로 설정합니다.
5. 큐에서 정점을 하나씩 꺼내면서 방문합니다. 이 때, 꺼낸 정점과 인접한 모든 정점에 대해, 그 정점이 아직 방문되지 않았다면 해당 정점을 큐에 추가하고, 방문한 것으로 표시합니다. 그리고 해당 정점까지의 거리를 distance[u] + 1로 설정하고, parent[v]를 u로 설정합니다.
6. 이 과정을 큐가 빌 때까지 반복합니다.
7. 최단 경로를 출력하기 위해, 끝 정점에서 시작하여 parent 배열을 이용해 역추적하며 경로를 구합니다. 이 때, 경로는 역순으로 구해지므로, 출력할 때는 역순으로 출력합니다.

따라서, 이 코드를 실행하면 "0 -> 1 -> 3 -> 5"라는 결과를 얻게 됩니다. 