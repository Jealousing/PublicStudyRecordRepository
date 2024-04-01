#include <iostream>
#include <queue>
#include <algorithm>
using namespace std;

/*
Problem Number: 2178

Problem Description :
N×M크기의 배열로 표현되는 미로가 있다.
1	0	1	1	1	1
1	0	1	0	1	0
1	0	1	0	1	1
1	1	1	0	1	1
미로에서 1은 이동할 수 있는 칸을 나타내고, 0은 이동할 수 없는 칸을 나타낸다. 
이러한 미로가 주어졌을 때, (1, 1)에서 출발하여 (N, M)의 위치로 이동할 때 지나야 하는 최소의 칸 수를 구하는 프로그램을 작성하시오. 
한 칸에서 다른 칸으로 이동할 때, 서로 인접한 칸으로만 이동할 수 있다.
위의 예에서는 15칸을 지나야 (N, M)의 위치로 이동할 수 있다. 칸을 셀 때에는 시작 위치와 도착 위치도 포함한다.

Link: https://www.acmicpc.net/problem/2178

Input:
첫째 줄에 두 정수 N, M(2 ≤ N, M ≤ 100)이 주어진다. 다음 N개의 줄에는 M개의 정수로 미로가 주어진다. 각각의 수들은 붙어서 입력으로 주어진다.

Output:
첫째 줄에 지나야 하는 최소의 칸 수를 출력한다. 항상 도착위치로 이동할 수 있는 경우만 입력으로 주어진다.

Limit: none
*/


#define MAXSIZE 101

struct graph
{
    int value;
    bool visited;
}arr[MAXSIZE][MAXSIZE];

int N,M,minValue = MAXSIZE;
int add[4][2] = { {-1,0},{1,0},{0,1},{0,-1} };

// dfs 시간초과
void dfs(int x, int y, int cnt)
{
    if (x == N - 1 && y == M - 1)
    {
        if (cnt < minValue) minValue = cnt;
        return;
    }
    for (int i = 0; i < 4; i++)
    {
        int newX = x + add[i][0];
        int newY = y + add[i][1];

        if (newX >= 0 && newX < N && newY >= 0 && newY < M &&
            !arr[newX][newY].visited && arr[newX][newY].value)
        {
            arr[newX][newY].visited = true;
            dfs(newX, newY,cnt+1);
            arr[newX][newY].visited = false;
        }
    }
}
void bfs(int x, int y)
{
    if (arr[x][y].visited) return;

    queue<pair<int, int>> que;
    que.push(make_pair(x, y));

    arr[x][y].visited = true;

    while (que.size() != 0)
    {
        int frontX = que.front().first;
        int frontY = que.front().second;

        que.pop();

        for (int i = 0; i < 4; i++)
        {
            int newX = frontX + add[i][0];
            int newY = frontY + add[i][1];

            if (newX >= 0 && newX < N && newY >= 0 && newY < M &&
                !arr[newX][newY].visited && arr[newX][newY].value >0)
            {
                arr[newX][newY].value = arr[frontX][frontY].value + 1;
                arr[newX][newY].visited = true;
                que.push(make_pair(newX, newY));
            }
        }
    }

    minValue = arr[N - 1][M - 1].value;
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력
    cin >> N >> M;
    for (int i = 0; i < N; i++)
    {
        string s;
        cin >> s;
        for (int j = 0; j < s.size(); j++)
        {
            arr[i][j].value = s[j] - '0';
        }
    }

    //dfs(0, 0,1);
    bfs(0, 0);

    // 결과출력
    cout << minValue << '\n';

    return 0;
}