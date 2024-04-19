# 플로이드-워셜(Floyd-Warshall) 알고리즘

## 플로이드-워셜 알고리즘이란?
 플로이드-워셜 알고리즘은 그래프 상의 모든 정점 쌍 사이의 최단 경로를 찾는 알고리즘입니다. 이 알고리즘은 음수 가중치를 가진 그래프에서도 사용할 수 있지만, 음수 사이클이 존재하면 정확한 결과를 보장하지 않습니다.

### 플로이드-워셜 알고리즘의 동작 원리
1. 초기화: 그래프의 인접 행렬을 초기화합니다. 시작 정점에서 다른 정점으로의 간선이 존재하면 해당 가중치를, 존재하지 않으면 무한대로 설정합니다. 또한 자기 자신으로의 경로는 0으로 설정합니다.
2. 거쳐가는 정점 고려: 모든 정점을 순회하면서 시작 정점부터 해당 정점까지의 최단 거리를 갱신합니다. 이때, 해당 정점을 거쳐가는 모든 경로를 고려하여 최단 거리를 갱신합니다.
3. 거쳐가는 정점 업데이트: 모든 정점 쌍을 순회하면서 현재 정점 쌍의 최단 거리를 갱신합니다. 이때, 이전 단계에서 갱신된 거리를 사용합니다.

## 플로이드-워셜 알고리즘의 특징
 
 ### 장점
 * 모든 정점 쌍 사이의 최단 경로를 찾을 수 있다.
 * 음수 가중치를 가진 그래프에서도 사용할 수 있다.

 ### 단점
 * 시간 복잡도가 크다.
 * 음수 사이클이 존재할 경우, 정확한 결과를 보장하지 않는다.
 
## 시간복잡도
플로이드-워셜 알고리즘의 시간 복잡도는 O(V^3)이며, 여기서 V는 정점의 수를 나타냅니다.
 
## 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

#define INF 0x3f3f3f3f // 무한대를 표현하기 위한 값으로 충분히 큰 정수값 사용

void floydWarshall(vector<vector<int>>& graph) 
{
    int V = graph.size();

    // 초기화: 자기 자신으로의 경로는 0으로, 경로가 없는 경우는 무한대로 설정
    for (int i = 0; i < V; ++i) 
    {
        for (int j = 0; j < V; ++j) 
        {
            if (i == j) 
            {
                graph[i][j] = 0;
            } 
            else if (graph[i][j] == 0) 
            {
                graph[i][j] = INF;
            }
        }
    }

    // 모든 정점을 순회하면서 최단 거리를 갱신
    for (int k = 0; k < V; ++k) 
    {
        for (int i = 0; i < V; ++i) 
        {
            for (int j = 0; j < V; ++j) 
            {
                // 정점 k를 거쳐가는 경로를 고려하여 최단 거리를 갱신
                if (graph[i][k] != INT_MAX && graph[k][j] != INT_MAX && graph[i][k] + graph[k][j] < graph[i][j]) 
                {
                    graph[i][j] = graph[i][k] + graph[k][j];
                }
            }
        }
    }
}

int main() 
{
    int V = 4; // 정점의 개수
    vector<vector<int>> graph = 
                                  {{0, 5, INF, 10},
                                  {INF, 0, 3, INF},
                                  {INF, INF, 0, 1},
                                  {INF, INF, INF, 0}};

    floydWarshall(graph);

    // 최단 경로 출력
    cout << "모든 정점 쌍 사이의 최단 경로:" << endl;
    for (int i = 0; i < V; ++i) 
    {
        for (int j = 0; j < V; ++j) 
        {
            if (graph[i][j] == INF) 
            {
                cout << "INF ";
            } 
            else 
            {
                cout << graph[i][j] << " ";
            }
        }
        cout << endl;
    }

    return 0;
}
```