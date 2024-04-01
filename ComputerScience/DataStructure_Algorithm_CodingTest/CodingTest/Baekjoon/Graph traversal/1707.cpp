#include <iostream>
#include <string.h>
#include <vector>
#include <queue>
#include <algorithm>
using namespace std;

/*
Problem Number: 1707

Problem Description :
그래프의 정점의 집합을 둘로 분할하여, 각 집합에 속한 정점끼리는 서로 인접하지 않도록 분할할 수 있을 때, 
그러한 그래프를 특별히 이분 그래프 (Bipartite Graph) 라 부른다.

그래프가 입력으로 주어졌을 때, 이 그래프가 이분 그래프인지 아닌지 판별하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1707

Input:
입력은 여러 개의 테스트 케이스로 구성되어 있는데, 
첫째 줄에 테스트 케이스의 개수 K가 주어진다. 
각 테스트 케이스의 첫째 줄에는 그래프의 정점의 개수 V와 간선의 개수 E가 빈 칸을 사이에 두고 순서대로 주어진다. 
각 정점에는 1부터 V까지 차례로 번호가 붙어 있다. 
이어서 둘째 줄부터 E개의 줄에 걸쳐 간선에 대한 정보가 주어지는데, 각 줄에 인접한 두 정점의 번호 u, v (u ≠ v)가 빈 칸을 사이에 두고 주어진다.

Output:
K개의 줄에 걸쳐 입력으로 주어진 그래프가 이분 그래프이면 YES, 아니면 NO를 순서대로 출력한다.

Limit: none
*/

/*
이분 그래프 (Bipartite Graph) -> 인접한 정점끼리 다른 값을 가지고 있으면서 모든 정점이 두개의 그룹으로 나눠지는 경우
*/

#define MAXSIZE  20001

struct graph
{
    vector<int> value;
    int visited;
}arr[MAXSIZE];

int v, e;

void bfs(int v)
{
    arr[v].visited = 1;
    queue<int> que;
    que.push(v);

    while (!que.empty())
    {
        int temp = que.front();
        que.pop();
        
        for (int i = 0; i < arr[temp].value.size(); i++)
        {
            if (arr[arr[temp].value[i]].visited == 0)
            {
                que.push(arr[temp].value[i]);
                if (arr[temp].visited == 1) arr[arr[temp].value[i]].visited = 2;
                else  if (arr[temp].visited == 2) arr[arr[temp].value[i]].visited = 1;
            }
        }
    }
}

string result()
{
    for (int i = 1; i <= v; i++)
    {
        for (int j = 0; j < arr[i].value.size(); j++)
        {
            if (arr[i].visited == arr[arr[i].value[j]].visited)
                return "NO";
        }
    }
    return "YES";
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력
    int t;
    cin >> t;

    for (int T = 0; T < t; T++)
    {
        cin >> v >> e;

        for (int E = 0; E < e; E++)
        {
            int temp1, temp2;
            cin >> temp1 >> temp2;
            arr[temp1].value.push_back(temp2);
            arr[temp2].value.push_back(temp1);
        }

        for (int V = 1; V <= v; V++)
        {
            // 방문한적 없는 노드는 탐색
            if (arr[V].visited==0)
            {
                bfs(V);
            }
        }

        // 출력
        cout << result() << '\n';

        // 초기화
        memset(arr, 0, sizeof(arr));
    }
    return 0;
}