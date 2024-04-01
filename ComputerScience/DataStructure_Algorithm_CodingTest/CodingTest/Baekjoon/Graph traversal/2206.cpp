#include <iostream>
#include <queue>
#include <algorithm>
using namespace std;

/*
Problem Number: 2206

Problem Description :
N×M의 행렬로 표현되는 맵이 있다. 맵에서 0은 이동할 수 있는 곳을 나타내고, 1은 이동할 수 없는 벽이 있는 곳을 나타낸다. 
당신은 (1, 1)에서 (N, M)의 위치까지 이동하려 하는데, 이때 최단 경로로 이동하려 한다. 
최단경로는 맵에서 가장 적은 개수의 칸을 지나는 경로를 말하는데, 이때 시작하는 칸과 끝나는 칸도 포함해서 센다.

만약에 이동하는 도중에 한 개의 벽을 부수고 이동하는 것이 좀 더 경로가 짧아진다면, 벽을 한 개 까지 부수고 이동하여도 된다.
한 칸에서 이동할 수 있는 칸은 상하좌우로 인접한 칸이다.

맵이 주어졌을 때, 최단 경로를 구해 내는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2206

Input:
첫째 줄에 N(1 ≤ N ≤ 1,000), M(1 ≤ M ≤ 1,000)이 주어진다. 다음 N개의 줄에 M개의 숫자로 맵이 주어진다. (1, 1)과 (N, M)은 항상 0이라고 가정하자.

Output:
첫째 줄에 최단 거리를 출력한다. 불가능할 때는 -1을 출력한다.

Limit: none
*/

/*
입력:
6 4
0100
1110
1000
0000
0111
0000

출력:
15
*/
#define MAXSIZE 1001 

struct graph
{
    int value;
    bool visited;
}arr[MAXSIZE][MAXSIZE][2];

int m, n;
int add[4][2] = { {-1,0},{1,0},{0,1},{0,-1} };


struct temp
{
    int x;
    int y;
    int broke;
    int cnt;
};
queue<temp> que;

int bfs()
{
    que.push({ 0, 0,0,1 });
    arr[0][0][0].visited = true;

    while (!que.empty())
    {
        int x = que.front().x;
        int y = que.front().y;
        int broke = que.front().broke;
        int cnt = que.front().cnt;
        que.pop();

        // 도착
        if (x == n - 1 && y == m - 1)
        {
            return cnt;
        }

        for (int i = 0; i < 4; i++)
        {
            int newX = x + add[i][0];
            int newY = y + add[i][1];

            if (newX >= 0 && n > newX && newY >= 0 && m > newY && !arr[newX][newY][broke].visited)
            {

                if (arr[newX][newY][broke].value == 1 && broke == 0)
                {
                    arr[newX][newY][broke+1].visited = true;
                    que.push({ newX, newY,broke+1,cnt + 1 });

                }
                else if (arr[newX][newY][broke].value == 0 )
                {
                    arr[newX][newY][broke].visited = true;
                    que.push({ newX, newY,broke,cnt + 1 });
                }
                
            }
        }
    }
    return -1;
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력
    cin >> n >> m;
    for (int i = 0; i < n; i++)
    {
        string s;
        cin >> s;
        for (int j = 0; j < s.size(); j++)
        {
            arr[i][j][0].value = s[j] - '0';
            arr[i][j][1].value = s[j] - '0';
        }
    }
    
    // 탐색 및 출력
    cout << bfs() << '\n';
    return 0;
}