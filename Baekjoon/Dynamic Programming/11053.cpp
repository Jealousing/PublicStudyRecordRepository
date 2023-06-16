#include <iostream>
#include <algorithm>
using namespace std;

/*
Problem Number: 11053

Problem Description :
수열 A가 주어졌을 때, 가장 긴 증가하는 부분 수열을 구하는 프로그램을 작성하시오.

예를 들어, 수열 A = {10, 20, 10, 30, 20, 50} 인 경우에 가장 긴 증가하는 부분 수열은 A = {10, 20, 10, 30, 20, 50} 이고, 길이는 4이다.

Link: https://www.acmicpc.net/problem/11053

Input:
첫째 줄에 수열 A의 크기 N (1 ≤ N ≤ 1,000)이 주어진다.

둘째 줄에는 수열 A를 이루고 있는 Ai가 주어진다. (1 ≤ Ai ≤ 1,000)

Output:
첫째 줄에 수열 A의 가장 긴 증가하는 부분 수열의 길이를 출력한다.

Limit: none
*/

/* 
6
10 20 10 30 20 50
4

dp[0] = 1   -> 10
dp[1] = 1 -> 10 , 2 -> 10,20   1, 1+1
dp[2] = 2 -> 10,20,             1.2+1
dp[3] = 3 -> 10,20,30

dp[5] = 4 -> 10,20,30,50 
dp[5] 에서 50이전 가장 큰수 dp[3]+1 

*/

#define MAXSIZE 1001
int arr[MAXSIZE],dp[MAXSIZE];
int n, maximum = 0;

int DynamicProgramming()
{
    for (int i = 0; i < n; i++)
    {
        dp[i] = 1;
        for (int j = 0; j < i; j++)
        {
            if (arr[i] > arr[j])
            {
                dp[i] = max(dp[i], dp[j] + 1);
            }
        }
        maximum = max(maximum, dp[i]);
    }

    return maximum;
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    cin >> n;
    for (int i = 0; i < n; i++)
    {
        cin >> arr[i];
    }
    cout << DynamicProgramming() << '\n';

    return 0;
}