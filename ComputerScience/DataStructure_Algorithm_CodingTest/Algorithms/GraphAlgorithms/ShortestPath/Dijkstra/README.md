# 다익스트라(Dijkstra) 알고리즘

## 다익스트라 알고리즘이란?
 다익스트라(Dijkstra) 알고리즘은 하나의 출발점으로부터 다른 모든 정점까지의 최단 경로를 찾는 알고리즘 중 하나입니다. 이 알고리즘은 가중치가 있는 그래프에서 사용되며, 음의 가중치를 가지는 간선이 없을 때에만 정확한 결과를 반환합니다.

### 다익스트라 알고리즘의 동작원리
1. 출발 정점을 설정합니다.
2. 출발 정점을 기준으로 각 정점까지의 거리를 저장하는 배열을 초기화합니다. 초기에는 출발 정점은 0, 나머지는 무한대의 값을 갖습니다.
3. 방문하지 않은 정점 중에서 가장 짧은 거리의 정점을 선택합니다.
4. 선택한 정점을 거쳐 다른 정점으로 가는 거리를 계산하여 거리 배열을 업데이트합니다.
5. 위 과정을 모든 정점을 방문할 때까지 반복합니다.

## 다익스트라 알고리즘 특징
 ### 장점
 * 출발점에서 각 정점까지의 최단 경로를 찾을 수 있습니다.
 * 음수 가중치가 없는 경우에만 사용할 수 있지만, 가중치가 같은 그래프에서도 잘 작동합니다.
 * 우선순위 큐를 활용하여 효율적으로 구현할 수 있습니다.
 ### 단점
 * 음수 가중치를 가지는 간선이 있는 경우에는 정확한 결과를 반환하지 않습니다.
 * 간선이 많고 가중치가 큰 경우에는 시간 복잡도가 높을 수 있습니다.

## 다익스트라 알고리즘의 시간복잡도
 * 이중 반복문을 사용하는 기본 구현의 경우, 시간 복잡도는 O(V^2)입니다. (V는 정점의 수)
 * 우선순위 큐를 사용하는 경우, 시간 복잡도는 O((V + E) log V)입니다. (V는 정점의 수, E는 간선의 수)

## 예시코드 c++
 
### 이중 반복문을 사용한 다익스트라
```cpp
#include <iostream>
#include <vector>

using namespace std;

#define INF 0x3f3f3f3f // 무한대를 표현하기 위한 값으로 충분히 큰 정수값 사용

vector<vector<pair<int, int>>> graph; // 각 노드에서 연결된 노드와 가중치를 저장하는 그래프
vector<int> dist; // 각 노드까지의 최단 거리를 저장하는 배열

void dijkstra(int start) 
{
    dist.assign(graph.size(), INF); // dist 배열을 무한대로 초기화
    dist[start] = 0; // 시작 노드의 최단 거리는 0

    vector<bool> visited(graph.size(), false); // 방문 여부를 저장하는 배열

    for (int i = 0; i < graph.size() - 1; ++i) // 모든 노드를 순회하며
    {
        int minDist = INF;
        int minIdx = -1;

        // 최단 거리가 현재까지의 최솟값인 노드를 찾습니다.
        for (int j = 0; j < graph.size(); ++j) 
        {
            if (!visited[j] && dist[j] < minDist) 
            {
                minDist = dist[j];
                minIdx = j;
            }
        }

        // 최솟값 노드를 방문하여 인접 노드들의 최단 거리를 업데이트합니다.
        visited[minIdx] = true;
        for (int k = 0; k < graph[minIdx].size(); ++k) 
        {
            int next = graph[minIdx][k].first;
            int weight = graph[minIdx][k].second;
            if (!visited[next] && dist[next] > dist[minIdx] + weight) 
            {
                dist[next] = dist[minIdx] + weight;
            }
        }
    }
}

int main() 
{
    int V, E; // 정점의 수와 간선의 수
    cin >> V >> E;

    // 그래프 초기화
    graph.resize(V);

    // 간선 정보 입력
    for (int i = 0; i < E; ++i) 
    {
        int u, v, w;
        cin >> u >> v >> w;
        graph[u].push_back({v, w}); // u에서 v로의 가중치 w인 간선 추가
    }

    int start; // 시작 노드
    cin >> start;

    // 다익스트라 알고리즘 호출
    dijkstra(start);

    // 각 노드까지의 최단 거리 출력
    for (int i = 0; i < V; ++i) 
    {
        if (dist[i] == INF) cout << "INF ";
        else cout << dist[i] << " ";
    }
    cout << endl;

    return 0;
}
```

### 우선순위 큐를 사용한 다익스트라 
```cpp
#include <iostream>
#include <vector>

using namespace std;

#define INF 0x3f3f3f3f // 무한대를 표현하기 위한 값으로 충분히 큰 정수값 사용

vector<vector<pair<int, int>>> graph; // 각 노드에서 연결된 노드와 가중치를 저장하는 그래프
vector<int> dist; // 각 노드까지의 최단 거리를 저장하는 배열

void dijkstra(int start) 
{
    dist.assign(graph.size(), INF); // dist 배열을 무한대로 초기화
    dist[start] = 0; // 시작 노드의 최단 거리는 0

    // 우선순위 큐를 사용하여 다익스트라 알고리즘 구현
    priority_queue<pair<int, int>, vector<pair<int, int>>, greater<pair<int, int>>> pq;
    pq.push({0, start}); // 시작 노드를 우선순위 큐에 삽입

    while (!pq.empty()) 
    {
        int curDist = pq.top().first;
        int cur = pq.top().second;
        pq.pop();

        // 현재 노드를 이미 처리한 적이 있는지 확인
        if (dist[cur] < curDist) continue;

        // 현재 노드와 연결된 모든 노드들을 순회
        for (int i = 0; i < graph[cur].size(); ++i)
        {
            int next = graph[cur][i].first;
            int weight = graph[cur][i].second;

            // 현재 노드를 통해 다음 노드로 가는 거리가 더 짧은 경우 업데이트
            if (dist[next] > curDist + weight) 
            {
                dist[next] = curDist + weight;
                pq.push({dist[next], next});
            }
        }
    }
}

int main() 
{
    int V, E; // 정점의 수와 간선의 수
    cin >> V >> E;

    // 그래프 초기화
    graph.resize(V);

    // 간선 정보 입력
    for (int i = 0; i < E; ++i) 
    {
        int u, v, w;
        cin >> u >> v >> w;
        graph[u].push_back({v, w}); // u에서 v로의 가중치 w인 간선 추가
    }

    int start; // 시작 노드
    cin >> start;

    // 다익스트라 알고리즘 호출
    dijkstra(start);

    // 각 노드까지의 최단 거리 출력
    for (int i = 0; i < V; ++i) 
    {
        if (dist[i] == INF) cout << "INF ";
        else cout << dist[i] << " ";
    }
    cout << endl;

    return 0;
}
```