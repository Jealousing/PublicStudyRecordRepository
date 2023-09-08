#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 10986

Problem Description :
수 N개 A1, A2, ..., AN이 주어진다. 이때, 연속된 부분 구간의 합이 M으로 나누어 떨어지는 구간의 개수를 구하는 프로그램을 작성하시오.
즉, Ai + ... + Aj (i ≤ j) 의 합이 M으로 나누어 떨어지는 (i, j) 쌍의 개수를 구해야 한다.

Link: https://www.acmicpc.net/problem/10986

Input:
첫째 줄에 N과 M이 주어진다. (1 ≤ N ≤ 10^6, 2 ≤ M ≤ 10^3)
둘째 줄에 N개의 수 A1, A2, ..., AN이 주어진다. (0 ≤ Ai ≤ 10^9)

Output:
첫째 줄에 연속된 부분 구간의 합이 M으로 나누어 떨어지는 구간의 개수를 출력한다.

Limit: none
*/

/* 
5 3
1 2 3 1 2

누적합 1 3 6 7 9
나머지 1 0 0 1 0 -> 
3 :  cnt[0] = 1,2 / 1,2,3 / 1,2,3,1,2
4 : 6-3(arr[3] - arr[2]) = 3 / 9-3(arr[5]-arr[2]) =6 / 9-6(arr[5]-arr[3]) =3 / 7-1(arr[4]-arr[1]) =6;
     index 3-2 / 5-2 /5-3 / 4-1/

4+3 = 7

*/

long long * arr, cnt[1001];
int n, m;
long long addValue =0;

long long PrefixSum()
{
    cnt[arr[0] % m]++;
    for (int i = 1; i < n; i++)
    {
        arr[i] = arr[i - 1] + arr[i]; //누적합
        cnt[arr[i] % m]++;//나머지에 따른 횟수 추가
    }

    // 나머지 i 에서  2가지를 뽑는 경우의 수
    // nC2 = n * (n - 1) / 2;
    for (int i = 0; i < m; i++) 
    {
        addValue += cnt[i] * (cnt[i] - 1) / 2;
    }

    return cnt[0] + addValue;
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 첫째 줄에 N과 M이 주어진다
    cin >> n>>m; 
    arr = new long long[n];
    for (int i = 0; i < n; i++) 
    {
        cin >> arr[i];
    }
    cout << PrefixSum() << '\n';
    delete[] arr;
    return 0;
}