# 벨만-포드(Bellman-Ford) 알고리즘

## 벨만-포드 알고리즘이란?
벨만-포드 알고리즘은 단일 출발 최단 경로 문제를 해결하는 알고리즘으로, 주어진 그래프에서 하나의 시작 정점에서 다른 모든 정점까지의 최단 경로를 찾습니다.   
 이 알고리즘은 음수 가중치를 포함한 그래프에서도 사용할 수 있으며, 음수 사이클을 감지할 수 있습니다.

### 벨만-포드 알고리즘의 동작원리
1. 초기화: 시작 정점을 제외한 모든 정점의 최단 거리를 무한대(infinity)로 설정하고, 시작 정점의 최단 거리를 0으로 설정합니다.
2. 간선 순회와 최단 거리 갱신: 각 정점에 대해 모든 간선을 순회하면서 시작 정점으로부터 해당 간선의 도착 정점까지의 거리를 갱신합니다. 이때, 이전 단계에서 갱신된 거리를 사용합니다.
3. 음수 사이클 검사: 모든 간선에 대해 정점의 수만큼 반복한 후, 만약 어떤 정점의 최단 거리가 여전히 갱신될 수 있다면, 그래프에 음수 사이클이 존재한다는 것을 의미합니다.

## 벨만-포드 알고리즘 특징
 
 ### 장점
* 음수 가중치 처리: 벨만-포드 알고리즘의 주요 장점은 음수 가중치를 가진 간선을 처리할 수 있다는 것입니다. 이는 다익스트라 알고리즘과 같은 다른 최단 경로 알고리즘이 할 수 없는 일입니다.
* 음수 사이클 탐지: 벨만-포드 알고리즘은 음수 사이클을 탐지할 수 있습니다. 음수 사이클이 존재하면 최단 경로 문제에는 해가 없습니다.
 ### 단점
*시간 복잡도: 벨만-포드 알고리즘의 시간 복잡도는 O(VE)로, V는 정점의 수, E는 간선의 수입니다. 이는 다익스트라 알고리즘과 같은 다른 최단 경로 알고리즘에 비해 상대적으로 높습니다.
*음수 사이클 제한: 벨만-포드 알고리즘은 음수 사이클이 존재하는 그래프에서는 최단 경로를 찾을 수 없습니다.
 
## 벨만-포드 알고리즘의 시간복잡도
벨만-포드 알고리즘의 시간 복잡도는 O(V * E)이며, 여기서 V는 정점의 수, E는 간선의 수를 나타냅니다.
 
## 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

struct Edge 
{
    // 간선의 출발 정점(src), 도착 정점(dest), 가중치(weight)
    int src, dest, weight; 
};

vector<int> bellmanFord(vector<Edge> edges, int V, int src) 
{
    vector<int> dist(V, INT_MAX); // 시작 정점으로부터의 최단 거리를 저장할 배열
    dist[src] = 0; // 시작 정점의 최단 거리를 0으로 설정

    // 모든 간선을 순회하면서 최단 거리 갱신
    for (int i = 0; i < V - 1; ++i) 
    {
        for (int j = 0; j < edges.size(); ++j) 
        {
            int u = edges[j].src;
            int v = edges[j].dest;
            int weight = edges[j].weight;
            if (dist[u] != INT_MAX && dist[u] + weight < dist[v]) 
            {
                dist[v] = dist[u] + weight;
            }
        }
    }

    // 음수 사이클 검사
    for (int i = 0; i < edges.size(); ++i) 
    {
        int u = edges[i].src;
        int v = edges[i].dest;
        int weight = edges[i].weight;
        if (dist[u] != INT_MAX && dist[u] + weight < dist[v]) 
        {
            cout << "음수 가중치 사이클이 존재합니다." << endl;
            return vector<int>(); // 음수 사이클이 존재할 경우 빈 벡터 반환
        }
    }

    return dist; // 시작 정점으로부터의 최단 거리 배열 반환
}

int main() 
{
    int V = 5; // 정점의 개수
    vector<Edge> edges = {{0, 1, -1}, {0, 2, 4}, {1, 2, 3}, {1, 3, 2}, {1, 4, 2}, {3, 2, 5}, {3, 1, 1}, {4, 3, -3}};
    int src = 0; // 시작 정점

    vector<int> distances = bellmanFord(edges, V, src);

    if (!distances.empty()) 
    {
        cout << "시작 정점 " << src << "으로부터의 최단 거리:" << endl;
        for (int i = 0; i < V; ++i) 
        {
            cout << "정점 " << i << ": " << distances[i] << endl;
        }
    }

    return 0;
}
```