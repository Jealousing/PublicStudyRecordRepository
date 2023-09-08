/*
Problem Number: 11725

Problem Description :
루트 없는 트리가 주어진다. 이때, 트리의 루트를 1이라고 정했을 때, 각 노드의 부모를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11725

Input:
첫째 줄에 노드의 개수 N (2 ≤ N ≤ 100,000)이 주어진다. 둘째 줄부터 N-1개의 줄에 트리 상에서 연결된 두 정점이 주어진다.

Output:
첫째 줄부터 N-1개의 줄에 각 노드의 부모 노드 번호를 2번 노드부터 순서대로 출력한다.

Limit: none
*/


#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

#define MAXSIZE 100001

struct graph
{
    vector<int> value;
    int visited;
}arr[MAXSIZE];

void dfs(int N)
{
    for (int i = 0; i < arr[N].value.size(); i++)
    {
        int temp = arr[N].value[i];
        if (arr[temp].visited ==0)
        {
            arr[temp].visited = N;
            dfs(temp);
        }
    }

}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    int n;
    cin >> n;

    // 입력
    for (int i = 0; i < n; i++)
    {
        int x, y;
        cin >> x >> y;
        arr[x].value.push_back(y);
        arr[y].value.push_back(x);
    }

    // 탐색
    dfs(1);

    // 출력
    for (int i = 2; i <= n; i++) 
    {
        cout << arr[i].visited << '\n';
    }

  
    return 0;
}