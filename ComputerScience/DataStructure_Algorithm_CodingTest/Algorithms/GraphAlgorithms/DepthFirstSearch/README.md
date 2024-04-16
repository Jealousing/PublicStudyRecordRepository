# 깊이 우선 탐색(DFS, Depth-First Search)

## 깊이 우선 탐색이란?
 깊이 우선 탐색(DFS, Depth-First Search)은 그래프의 모든 정점을 방문하는 방법 중 하나로, 시작 정점에서 다음 분기로 넘어가기 전에 해당 분기를 완전히 탐색하는 방법입니다.

### 깊이 우선 탐색의 동작 원리
1. 시작 정점을 방문하고, 이웃한 정점들 중 하나를 선택하여 깊이 우선 탐색을 재귀적으로 수행합니다.
2. 선택한 정점이 더 이상 방문할 수 있는 이웃이 없을 때까지 반복합니다.
3. 방문하지 않은 정점이 남아있는 경우, 그 중 하나를 선택하여 위 과정을 반복합니다.
4. 모든 정점을 방문할 때까지 이러한 과정을 반복합니다.

## 깊이 우선 탐색의 특징
 
 ### 장점
* 모든 노드를 방문 하고자 하는 경우에 유용합니다.
* 너비 우선 탐색(BFS)보다 조금 더 간단합니다.
* 스택 또는 재귀 함수를 이용하여 간단하게 구현할 수 있습니다.

 ### 단점
* 무한히 깊은 경로에 빠질 수 있습니다.
* 최단 경로를 찾는 문제에는 적합하지 않을 수 있습니다.
 
## 시간복잡도
 깊이 우선 탐색의 시간 복잡도는 정점의 수를 V, 간선의 수를 E라고 할 때, O(V+E)입니다.
 
## 예시코드 c++
```cpp
#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

class Graph 
{
private:
    int V; // 정점의 개수
    vector<vector<int>> adj; // 인접 리스트

    // 깊이 우선 탐색을 위한 재귀 함수
    void DFSUtil(int v, vector<bool>& visited) 
    {
        // 현재 정점을 방문한 것으로 표시하고 출력
        visited[v] = true;
        cout << v << " ";

        // 모든 이웃한 정점을 방문
        for (int u : adj[v]) 
        {
            if (!visited[u])
                DFSUtil(u, visited);
        }
    }

public:
    // 그래프 생성자
    Graph(int V) : V(V) 
    {
        adj.resize(V);
    }

    // 간선 추가
    void addEdge(int v, int w) 
    {
        adj[v].push_back(w); // v의 인접 리스트에 w를 추가
    }

    // DFS 시작
    void DFS(int v) 
    {
        // 방문 여부를 저장하는 배열
        vector<bool> visited(V, false);

        // DFSUtil 호출
        DFSUtil(v, visited);
    }
};

int main() 
{
    Graph g(4);
    g.addEdge(0, 1);
    g.addEdge(0, 2);
    g.addEdge(1, 2);
    g.addEdge(2, 0);
    g.addEdge(2, 3);
    g.addEdge(3, 3);

    cout << "깊이 우선 탐색 순서: ";
    g.DFS(2); // 정점 2에서 깊이 우선 탐색 시작

    return 0;
}
```
## 사용예시 & 코딩테스트 유형
* 깊이 우선 탐색은 그래프 탐색 문제와 관련하여 코딩 테스트에서 자주 출제됩니다. 특히, 그래프의 연결 여부나 경로의 존재 여부를 확인하는 문제에 적합합니다.
* 또한, 재귀적인 호출을 사용하는 깊이 우선 탐색은 재귀에 대한 이해를 요구하는 문제로도 출제될 수 있습니다.
