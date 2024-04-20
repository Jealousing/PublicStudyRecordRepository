# A*(AStar) 알고리즘

## A* 알고리즘이란?
 A* 알고리즘은 그래프의 탐색 알고리즘으로 주로 게임에서 플레이어를 목표지점으로 이동시킬 때 사용하는 알고리즘입니다. A* 알고리즘은 가중치 그래프에서 시작 노드에서 목표 노드까지의 최단 경로만 구하려 하는 그리디 알고리즘입니다.
A* 알고리즘은 휴리스틱 함수(Heuristic Function)를 사용하여 탐색을 진행하며, 각 노드까지의 예상 비용을 계산하여 최적의 경로를 찾습니다.

### A* 알고리즘의 동작 원리
1. 초기화: 시작 노드를 선택하고, 시작 노드의 이동 비용과 휴리스틱 값을 초기화합니다. 이때, 휴리스틱 값은 목표 노드까지의 예상 최소 이동 비용을 나타냅니다.
2. 오픈 리스트 초기화: 시작 노드를 포함하는 오픈 리스트를 초기화합니다. 오픈 리스트는 아직 방문하지 않은 노드들의 집합을 나타냅니다.
3. 노드 선택: 오픈 리스트에서 가장 우선 순위가 높은(최소의 휴리스틱 값과 이동 비용을 가진) 노드를 선택합니다.
4. 노드 확장: 선택한 노드의 이웃 노드들을 확장합니다. 각 이웃 노드에 대해 다음을 수행합니다:
 	* 현재까지의 이동 비용과 휴리스틱 값을 사용하여 각 이웃 노드까지의 예상 총 비용을 계산합니다.
 	* 이웃 노드가 이미 오픈 리스트에 있는 경우, 현재까지의 비용과 비교하여 더 작은 비용으로 업데이트합니다.
 	* 이웃 노드가 아직 오픈 리스트에 없는 경우, 오픈 리스트에 추가합니다.
5. 목표 확인: 선택한 노드가 목표 노드인지 확인합니다. 목표 노드가 아닌 경우, 3단계로 돌아가서 다음 최우선 순위 노드를 선택합니다.
6. 경로 추적: 목표 노드에 도달한 경우, 목표 노드부터 시작하여 각 노드의 부모를 따라가면서 최단 경로를 추적합니다.
7. 결과 반환: 최단 경로와 이동 비용을 반환하거나 출력합니다.

## A* 알고리즘의 특징
 ### 장점
 * A* 알고리즘은 휴리스틱 함수를 사용하여 효율적으로 최단 경로를 찾습니다.
 * 휴리스틱 함수의 선택에 따라 알고리즘의 성능이 달라집니다.
 * A* 알고리즘은 최선 우선 탐색(Best-First Search)과 유사하지만, 휴리스틱 함수를 사용하여 탐색의 효율성을 향상시킵니다.

 ### 단점
 * A* 알고리즘은 가지 수가 유한하고 모든 동작에 고정 비용이 있는 경우에만 완전합니다.
 * A* 알고리즘은 최악의 경우 시간 복잡도가 높을 수 있습니다.
 * 휴리스틱 함수의 품질에 따라 최적의 경로를 찾지 못할 수 있습니다.
 
## 휴리스틱 함수의 종류
A* 알고리즘에서 사용되는 휴리스틱 함수는 목표 노드까지의 예상 최소 이동 비용을 계산하는 데 중요한 역할을 합니다. 다양한 휴리스틱 함수가 있으며, 그래프의 특성과 문제에 따라 적절한 함수를 선택할 수 있습니다.
* 맨해튼 거리(Manhattan Distance): 현재 노드와 목표 노드 간의 가로 방향 및 세로 방향 거리의 합입니다. 격자 형태의 그래프에서 유용하게 사용됩니다.
* 유클리드 거리(Euclidean Distance): 현재 노드와 목표 노드 간의 직선 거리입니다. 공간에서의 거리를 나타내는 데 사용됩니다.
* 체비세프 거리(Chebyshev Distance): 현재 노드와 목표 노드 사이의 최대 차이를 측정하는 거리입니다. 킹 이동 경로나 대각선 이동을 허용하는 그래프에서 유용합니다.
* 간단한 휴리스틱(Admissible Heuristic): 두 노드 사이의 최소 비용을 과소평가하지 않는 휴리스틱 함수입니다. 다익스트라 알고리즘에서 사용되는 거리 추정치를 A* 알고리즘에도 적용할 수 있습니다.

A*에서 휴리스틱 함수의 가중치를 0으로 설정한다면 다익스트라와 동일하게 작동합니다.

## 시간복잡도
A* 알고리즘의 시간 복잡도는 일반적으로 휴리스틱 함수의 품질과 그래프의 구조에 따라 달라집니다. 

## 예시코드 c++
```cpp
#include <iostream>
#include <vector>
#include <queue>
#include <cmath>

using namespace std;

// 무한대를 표현하기 위한 값으로 충분히 큰 정수값 사용
#define INF 0x3f3f3f3f

// 정점 구조체
struct Vertex 
{
    int id; // 정점의 ID
    int heuristic; // 휴리스틱 값
    int distance; // 시작점으로부터의 거리
    Vertex(int id, int heuristic) : id(id), heuristic(heuristic), distance(INF) {}
};

// 간선 구조체
struct Edge 
{
    int dest; // 도착 정점의 ID
    int weight; // 간선의 가중치
    Edge(int dest, int weight) : dest(dest), weight(weight) {}
};

// 비교 함수 구현
struct CompareVertex 
{
    bool operator()(const Vertex& v1, const Vertex& v2) const 
    {
        return v1.distance + v1.heuristic > v2.distance + v2.heuristic;
    }
};

// 그래프 구조체
class Graph 
{
    int V; // 정점의 수
    vector<vector<Edge>> adj; // 인접 리스트

public:
    Graph(int V) : V(V) 
    {
        adj.resize(V);
    }

    // 간선 추가 함수
    void addEdge(int src, int dest, int weight) 
    {
        adj[src].emplace_back(dest, weight);
    }

    // 휴리스틱 함수
    int heuristic(int v, int goal) 
    {
        // 맨해튼 거리를 휴리스틱 함수로 사용
        return abs(v / sqrt(V) - goal / sqrt(V)) + abs(v % sqrt(V) - goal % sqrt(V));
    }

    // A* 알고리즘
    void AStar(int start, int goal) 
    {
        vector<bool> visited(V, false); // 방문 여부 배열
        vector<int> parent(V, -1); // 부모 노드 배열    
        vector<int> distance(V, INF); // 시작점으로부터의 거리 배열

        // 우선순위 큐를 사용한 최소 힙 구조
        priority_queue<Vertex, vector<Vertex>, CompareVertex> pq;

         // 시작점 초기화
        distance[start] = 0;
        pq.push(Vertex(start, heuristic(start, goal)));

        while (!pq.empty()) 
        {
            int u = pq.top().id;
            pq.pop();

            // 목표에 도달한 경우 탐색 종료
            if (u == goal)
                break;

            // 방문한 정점은 무시
            if (visited[u])
                continue;

            visited[u] = true;

            // 현재 정점의 인접한 정점들을 순회
            for (int i = 0; i < adj[u].size(); ++i)
            {
                int v = adj[u][i].dest;
                int weight = adj[u][i].weight;
 
                 // 시작점으로부터 현재 정점까지의 거리 갱신
                if (!visited[v] && distance[u] + weight < distance[v]) 
                {
                    parent[v] = u;
                    distance[v] = distance[u] + weight;
                    pq.push(Vertex(v, distance[v] + heuristic(v, goal)));
                }
            }
        }

        // 최단 경로 출력
        if (distance[goal] != INF) 
        {
            cout << "최단 경로 길이: " << distance[goal] << endl;
            cout << "최단 경로: ";
            int curr = goal;
            while (curr != -1) 
            {
                cout << curr << " ";
                curr = parent[curr];
            }
            cout << endl;
        } 
        else 
        {
            cout << "목표로 가는 경로가 없습니다." << endl;
        }
    }
};

int main() 
{
    int V = 6; // 정점의 수
    Graph graph(V);

    // 간선 추가
    graph.addEdge(0, 1, 4);
    graph.addEdge(0, 2, 2);
    graph.addEdge(1, 2, 5);
    graph.addEdge(1, 3, 10);
    graph.addEdge(2, 4, 3);
    graph.addEdge(3, 5, 1);
    graph.addEdge(4, 3, 2);
    graph.addEdge(4, 5, 5);

    int start = 0; // 시작점
    int goal = 5; // 목표점

    // A* 알고리즘 실행
    graph.AStar(start, goal);

    return 0;
}

```