/*
Problem Number: 1753

Problem Description :
방향그래프가 주어지면 주어진 시작점에서 다른 모든 정점으로의 최단 경로를 구하는 프로그램을 작성하시오. 단, 모든 간선의 가중치는 10 이하의 자연수이다.

Link: https://www.acmicpc.net/problem/1753

Input:
첫째 줄에 정점의 개수 V와 간선의 개수 E가 주어진다. (1 ≤ V ≤ 20,000, 1 ≤ E ≤ 300,000) 모든 정점에는 1부터 V까지 번호가 매겨져 있다고 가정한다. 
둘째 줄에는 시작 정점의 번호 K(1 ≤ K ≤ V)가 주어진다. 셋째 줄부터 E개의 줄에 걸쳐 각 간선을 나타내는 세 개의 정수 (u, v, w)가 순서대로 주어진다. 
이는 u에서 v로 가는 가중치 w인 간선이 존재한다는 뜻이다. u와 v는 서로 다르며 w는 10 이하의 자연수이다. 서로 다른 두 정점 사이에 여러 개의 간선이 존재할 수도 있음에 유의한다.

Output:
첫째 줄부터 V개의 줄에 걸쳐, i번째 줄에 i번 정점으로의 최단 경로의 경로값을 출력한다. 시작점 자신은 0으로 출력하고, 경로가 존재하지 않는 경우에는 INF를 출력하면 된다.

Limit: none
*/

#include <iostream>
#include <vector>
#include <queue>
#include <algorithm>
using namespace std;

#define MAX_VERTICES 20001  // 최대 정점 개수
#define MAX_EDGE 300001 // 최대 간선 개수
#define INF 1000000  //무한대값

int dist[MAX_VERTICES];
vector<pair<int, int> > edge[MAX_EDGE];

void Dijkstra(int v, int startVertex)
{
    // 비용 0 처리 ( 시작위치 )
    dist[startVertex] = 0;

    // 우선순위 큐 활용 
    priority_queue<pair<int, int>> que;
    que.push(make_pair(0, startVertex));

    while (!que.empty())
    {
        // 도착위치, 비용
        int curPoint = que.top().second;
        int curDist = -que.top().first;
        que.pop();

        // 더 저렴한 방법이 있으므로 확인 x 
        if (dist[curPoint] < curDist) continue;

        for (int i = 0; i < edge[curPoint].size(); i++)
        {
            int nextPoint = edge[curPoint][i].second;
            int nextDist = curDist + edge[curPoint][i].first;

            if (dist[nextPoint] > nextDist)
            {
                // 최소 비용 갱신 및 큐에 입력
                dist[nextPoint] = nextDist;
                que.push(make_pair(-nextDist, nextPoint));
            }
        }
    }

    // 출력
    for (int i = 1; i <= v; i++)
    {
        if (dist[i] == INF) cout << "INF" << '\n';
        else cout << dist[i] << '\n';

    }
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력 ( 정점, 간선, 시작 정점)
    int v, e, startVertex;
    cin >> v >> e >> startVertex;

    // 간선데이터 저장 ( 시작,도착 위치 및 비용 )
    for (int i = 0; i < e; i++)
    {
        int start, end, cost;
        cin >> start >> end >> cost;

        edge[start].push_back(make_pair(cost, end));
    }

    // 초기화
    for (int i = 0; i <= v; i++) dist[i] = INF;

    // Dijkstra's Algorithm (Dijkstra)
    Dijkstra(v,startVertex);

    return 0;
}