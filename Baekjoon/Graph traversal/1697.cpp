#include <iostream>
#include <queue>
#include <algorithm>
using namespace std;

/*
Problem Number: 1697

Problem Description :
수빈이는 동생과 숨바꼭질을 하고 있다. 
수빈이는 현재 점 N(0 ≤ N ≤ 100,000)에 있고, 동생은 점 K(0 ≤ K ≤ 100,000)에 있다. 
수빈이는 걷거나 순간이동을 할 수 있다. 만약, 수빈이의 위치가 X일 때 걷는다면 1초 후에 X-1 또는 X+1로 이동하게 된다. 
순간이동을 하는 경우에는 1초 후에 2*X의 위치로 이동하게 된다.

수빈이와 동생의 위치가 주어졌을 때, 수빈이가 동생을 찾을 수 있는 가장 빠른 시간이 몇 초 후인지 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1697

Input:
첫 번째 줄에 수빈이가 있는 위치 N과 동생이 있는 위치 K가 주어진다. N과 K는 정수이다.

Output:
수빈이가 동생을 찾는 가장 빠른 시간을 출력한다.

Limit: none
*/


#define MAXSIZE 100001

struct graph
{
    int value;
    bool visited;
}arr[MAXSIZE];

int result;

void bfs(int n, int k)
{
    if (arr[n].visited) return;

    queue<int> que;
    que.push(n);
    arr[n].visited = true;

    while (!que.empty())
    {
        int point = que.front();
        int timer = arr[point].value;
        que.pop();

        if (point >= 0 && point<MAXSIZE)
        {
            // 정답
            if (point == k)
            {
                result = timer;
                break;
            }

            // 탐색
            if (point * 2 < MAXSIZE &&!arr[point * 2].visited)
            {
                arr[point * 2].visited = true;
                arr[point * 2].value = timer + 1;
                que.push(point * 2);
            }
            if (!arr[point + 1].visited)
            {
                arr[point +1].visited = true;
                arr[point +1].value = timer + 1;
                que.push(point +1);
            }
            if (!arr[point -1].visited)
            {
                arr[point -1].visited = true;
                arr[point -1].value = timer + 1;
                que.push(point -1);
            }
        }
    }

}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    int n, k;
    // 입력
    cin >> n >> k;

    // 탐색
    bfs(n, k);

    // 결과출력
    cout << result << '\n';

    return 0;
}