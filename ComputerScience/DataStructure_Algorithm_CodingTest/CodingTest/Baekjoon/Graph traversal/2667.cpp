#include <iostream>
#include <vector>
#include <queue>
#include <algorithm>
using namespace std;

/*
Problem Number: 2667

Problem Description :
<그림 1>과 같이 정사각형 모양의 지도가 있다. 1은 집이 있는 곳을, 0은 집이 없는 곳을 나타낸다.
철수는 이 지도를 가지고 연결된 집의 모임인 단지를 정의하고, 단지에 번호를 붙이려 한다.
여기서 연결되었다는 것은 어떤 집이 좌우, 혹은 아래위로 다른 집이 있는 경우를 말한다.
대각선상에 집이 있는 경우는 연결된 것이 아니다. <그림 2>는 <그림 1>을 단지별로 번호를 붙인 것이다.
지도를 입력하여 단지수를 출력하고, 각 단지에 속하는 집의 수를 오름차순으로 정렬하여 출력하는 프로그램을 작성하시오.
https://www.acmicpc.net/upload/images/ITVH9w1Gf6eCRdThfkegBUSOKd.png

Link: https://www.acmicpc.net/problem/2667

Input:
첫 번째 줄에는 지도의 크기 N(정사각형이므로 가로와 세로의 크기는 같으며 5≤N≤25)이 입력되고, 그 다음 N줄에는 각각 N개의 자료(0혹은 1)가 입력된다.

Output:
첫 번째 줄에는 총 단지수를 출력하시오. 그리고 각 단지내 집의 수를 오름차순으로 정렬하여 한 줄에 하나씩 출력하시오.

Limit: none
*/


#define MAXSIZE 26

struct graph
{
    int value;
    bool visited;
}arr[MAXSIZE][MAXSIZE];

int N, cnt = 1;
int add[4][2] = { {-1,0},{1,0},{0,1},{0,-1} };

void dfs(int x, int y)
{
    for (int i = 0; i < 4; i++)
    {
        int newX = x + add[i][0];
        int newY = y + add[i][1];

        if (newX < N && newY < N &&
            !arr[newX][newY].visited && arr[newX][newY].value == 1)
        {
            arr[newX][newY].visited = true;
            cnt++;
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
    cin >> N;
    vector<int> vec;
    for (int i = 0; i < N; i++)
    {
        string s;
        cin >> s;
        for (int j = 0; j < s.size(); j++)
        {
            arr[i][j].value = s[j] - '0';
        }
    }

    // 2차원배열 순회
    for (int i = 0; i < N; i++)
    {
        for (int j = 0; j < N; j++)
        {
            // 방문한적이 없고 집이 있는 배열이면 
            if (!arr[i][j].visited && arr[i][j].value == 1)
            {
                arr[i][j].visited = true;
                dfs(i, j);
                vec.push_back(cnt);
                cnt = 1;
            }
        }
    }


    // 정렬
    sort(vec.begin(), vec.end());

    // 결과출력
    cout << vec.size() << '\n';
    for (int i = 0; i < vec.size(); i++)
    {
        cout << vec[i] << '\n';
    }


    return 0;
}