#include <iostream>
#include <vector>
#include <queue>
#include <algorithm>
using namespace std;

/*
Problem Number: 1260

Problem Description :
그래프를 DFS로 탐색한 결과와 BFS로 탐색한 결과를 출력하는 프로그램을 작성하시오. 
단, 방문할 수 있는 정점이 여러 개인 경우에는 정점 번호가 작은 것을 먼저 방문하고, 더 이상 방문할 수 있는 점이 없는 경우 종료한다. 
정점 번호는 1번부터 N번까지이다.

Link: https://www.acmicpc.net/problem/1260

Input:
첫째 줄에 정점의 개수 N(1 ≤ N ≤ 1,000), 간선의 개수 M(1 ≤ M ≤ 10,000), 탐색을 시작할 정점의 번호 V가 주어진다.
다음 M개의 줄에는 간선이 연결하는 두 정점의 번호가 주어진다. 어떤 두 정점 사이에 여러 개의 간선이 있을 수 있다. 입력으로 주어지는 간선은 양방향이다.

Output:
첫째 줄에 DFS를 수행한 결과를, 그 다음 줄에는 BFS를 수행한 결과를 출력한다. V부터 방문된 점을 순서대로 출력하면 된다.

Limit: none
*/


#define MAXSIZE 100001

struct graph
{
    vector<int> value;
    bool visited;
}arr[MAXSIZE];

int cnt = 1;

void dfs(int R)
{
    if (arr[R].visited) return;

    arr[R].visited = true;
    cout << R << ' ';

    for (int i = 0; i < arr[R].value.size(); i++)
    {
        dfs(arr[R].value[i]);
    }
}


void bfs(int R)
{
    if (arr[R].visited) return;

    queue<int> que;
    que.push(R);

    arr[R].visited = true;
    cout << R << ' ';

    while (que.size() != 0)
    {
        int temp1 = que.front();
        que.pop();

        for (int i = 0; i < arr[temp1].value.size(); i++)
        {
            int temp2 = arr[temp1].value[i];
            if (!arr[temp2].visited)
            {
                arr[temp2].visited = true;
                cout << temp2 << ' ';
                que.push(temp2);
            }
        }
    }

}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    int N, M, R;

    // 입력
    cin >> N >> M >> R;
    for (int i = 0; i < M; i++)
    {
        int tempA, tempB;
        cin >> tempA >> tempB;

        arr[tempA].value.push_back(tempB);
        arr[tempB].value.push_back(tempA);
    }
   
    // 정렬
    for (int i = 1; i <= N; i++)
    {
        sort(arr[i].value.begin(), arr[i].value.end());
    }

    //dfs
    // 순회
    dfs(R);

    //초기화
    cnt = 1;
    for (int i = 1; i <= N; i++)
    {
        arr[i].visited = false;
    }
    cout << '\n';
  
    //bfs
    // 순회
    bfs(R);

    return 0;
}