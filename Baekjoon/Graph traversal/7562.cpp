#include <iostream>
#include <queue>
#include <string.h>
#include <algorithm>
using namespace std;

/*
Problem Number: 7562

Problem Description :
체스판 위에 한 나이트가 놓여져 있다. 나이트가 한 번에 이동할 수 있는 칸은 아래 그림에 나와있다. 나이트가 이동하려고 하는 칸이 주어진다. 나이트는 몇 번 움직이면 이 칸으로 이동할 수 있을까?
https://www.acmicpc.net/upload/images/knight.png

Link: https://www.acmicpc.net/problem/7562

Input:
입력의 첫째 줄에는 테스트 케이스의 개수가 주어진다.
각 테스트 케이스는 세 줄로 이루어져 있다.
첫째 줄에는 체스판의 한 변의 길이 l(4 ≤ l ≤ 300)이 주어진다. 체스판의 크기는 l × l이다. 체스판의 각 칸은 두 수의 쌍 {0, ..., l-1} × {0, ..., l-1}로 나타낼 수 있다.
둘째 줄과 셋째 줄에는 나이트가 현재 있는 칸, 나이트가 이동하려고 하는 칸이 주어진다.

Output:
각 테스트 케이스마다 나이트가 최소 몇 번만에 이동할 수 있는지 출력한다.

Limit: none
*/


#define MAXSIZE 301

struct graph
{
    int value;
    bool visited;
}arr[MAXSIZE][MAXSIZE];

int n;
int add[8][2] = { {-1,2},{-2,1},{1,2},{2,1},{-1,-2},{-2,-1},{1,-2},{2,-1} };

void bfs(int x, int y)
{
    if (arr[x][y].visited) return;

    queue<pair<int, int>> que;
    que.push(make_pair(x, y));
    arr[x][y].visited = 1;

    while (!que.empty())
    {
        int x = que.front().first;
        int y = que.front().second;
        que.pop();

        for (int i = 0; i < 8; i++)
        {
            int newX = x + add[i][0];
            int newY = y + add[i][1];

            if (newX >= 0 && n > newX && newY >= 0 && n > newY && !arr[newX][newY].visited)
            {
                que.push(make_pair(newX, newY));
                arr[newX][newY].visited = true;
                arr[newX][newY].value = arr[x][y].value + 1;
            }
        }
    }
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    int t;

    cin >> t;

    for (int i = 0; i < t; i++)
    {
        // 한 변의 길이
        cin >> n;

        // 입력 ( 현재 있는 칸 , 이동할 좌표 )
        int curX, curY, moveX, moveY;
        cin >> curX >> curY >> moveX >> moveY;
        arr[curX][curY].value = 1;

        // 탐색
        bfs(curX, curY);

        // 결과출력
        cout << arr[moveX][moveY].value - 1 << '\n';

        //초기화
        memset(arr, 0, sizeof(arr));
    }

    return 0;
}