#include <iostream>
#include <algorithm>
#include <string.h>
using namespace std;

/*
Problem Number: 1012

Problem Description :
차세대 영농인 한나는 강원도 고랭지에서 유기농 배추를 재배하기로 하였다. 
농약을 쓰지 않고 배추를 재배하려면 배추를 해충으로부터 보호하는 것이 중요하기 때문에, 한나는 해충 방지에 효과적인 배추흰지렁이를 구입하기로 결심한다.
이 지렁이는 배추근처에 서식하며 해충을 잡아 먹음으로써 배추를 보호한다. 
특히, 어떤 배추에 배추흰지렁이가 한 마리라도 살고 있으면 이 지렁이는 인접한 다른 배추로 이동할 수 있어, 
그 배추들 역시 해충으로부터 보호받을 수 있다. 한 배추의 상하좌우 네 방향에 다른 배추가 위치한 경우에 서로 인접해있는 것이다.

한나가 배추를 재배하는 땅은 고르지 못해서 배추를 군데군데 심어 놓았다.
배추들이 모여있는 곳에는 배추흰지렁이가 한 마리만 있으면 되므로 서로 인접해있는 배추들이 몇 군데에 퍼져있는지 조사하면
총 몇 마리의 지렁이가 필요한지 알 수 있다. 예를 들어 배추밭이 아래와 같이 구성되어 있으면 최소 5마리의 배추흰지렁이가 필요하다. 
0은 배추가 심어져 있지 않은 땅이고, 1은 배추가 심어져 있는 땅을 나타낸다.

Link: https://www.acmicpc.net/problem/1012

Input:
입력의 첫 줄에는 테스트 케이스의 개수 T가 주어진다. 
그 다음 줄부터 각각의 테스트 케이스에 대해 첫째 줄에는 배추를 심은 배추밭의 가로길이 M(1 ≤ M ≤ 50)과 세로길이 N(1 ≤ N ≤ 50), 
그리고 배추가 심어져 있는 위치의 개수 K(1 ≤ K ≤ 2500)이 주어진다. 
그 다음 K줄에는 배추의 위치 X(0 ≤ X ≤ M-1), Y(0 ≤ Y ≤ N-1)가 주어진다. 두 배추의 위치가 같은 경우는 없다.

Output:
각 테스트 케이스에 대해 필요한 최소의 배추흰지렁이 마리 수를 출력한다.

Limit: none
*/


#define MAXSIZE 51

struct graph
{
    bool value;
    bool visited;
}arr[MAXSIZE][MAXSIZE];

int N,M,K, cnt = 0;
int add[4][2] = { {-1,0},{1,0},{0,1},{0,-1} };

void dfs(int x, int y)
{
    for (int i = 0; i < 4; i++)
    {
        int newX = x + add[i][0];
        int newY = y + add[i][1];

        if (newX >= 0 && newX < N && newY >= 0 && newY < M &&
            !arr[newX][newY].visited && arr[newX][newY].value)
        {
            arr[newX][newY].visited = true;
            dfs(newX, newY);
        }
    }
}


int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력
    int T;
    cin >> T;
    for (int t = 0; t < T; t++)
    {
        cin >> M >> N >> K;
        for (int k = 0; k < K; k++)
        {
            int x, y;
            cin >> y>>x;
            arr[x][y].value = true;
        }

        // 2차원배열 순회
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                // 방문한적이 없고 집이 있는 배열이면 
                if (!arr[i][j].visited && arr[i][j].value)
                {
                    arr[i][j].visited = true;
                    cnt++;
                    dfs(i, j);
                }
            }
        }

        // 결과출력
        cout << cnt << '\n';
        
        // 추가진행을 해야되는 경우
        if (t < T-1)
        {
            cnt = 0;
            memset(arr, 0, sizeof(arr));
        }
    }

    return 0;
}