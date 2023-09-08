#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 11659

Problem Description :
수 N개가 주어졌을 때, i번째 수부터 j번째 수까지 합을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11659

Input:
첫째 줄에 수의 개수 N과 합을 구해야 하는 횟수 M이 주어진다. 둘째 줄에는 N개의 수가 주어진다. 
수는 1,000보다 작거나 같은 자연수이다. 셋째 줄부터 M개의 줄에는 합을 구해야 하는 구간 i와 j가 주어진다.

Output:
총 M개의 줄에 입력으로 주어진 i번째 수부터 j번째 수까지 합을 출력한다.

Limit: 
1 ≤ N ≤ 100,000
1 ≤ M ≤ 100,000
1 ≤ i ≤ j ≤ N
*/

/* 
5 3
5 4 3 2 1
1 3
2 4
5 5

12
9
1

*/

#define MAXSIZE 100001
int arr[MAXSIZE];
int n,m;

void PrefixSum()
{
    for (int i = 1; i <= n; i++)
    {
        arr[i] = arr[i - 1] + arr[i];
    }
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    cin >> n >>m;

    for (int i = 1; i <= n; i++)
    {
        cin >> arr[i];
    }
    PrefixSum();
    for (int i = 0; i < m; i++)
    {
        int a, b;
        cin >> a >> b;
        cout << arr[b ] - arr[a -1] << '\n';
    }

    return 0;
}