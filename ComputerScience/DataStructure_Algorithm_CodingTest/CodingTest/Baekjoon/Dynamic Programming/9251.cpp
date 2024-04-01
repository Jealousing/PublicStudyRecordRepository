#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 9251

Problem Description :
LCS(Longest Common Subsequence, 최장 공통 부분 수열)문제는 두 수열이 주어졌을 때, 모두의 부분 수열이 되는 수열 중 가장 긴 것을 찾는 문제이다.
예를 들어, ACAYKP와 CAPCAK의 LCS는 ACAK가 된다.

Link: https://www.acmicpc.net/problem/9251

Input:
첫째 줄과 둘째 줄에 두 문자열이 주어진다. 문자열은 알파벳 대문자로만 이루어져 있으며, 최대 1000글자로 이루어져 있다.

Output:
첫째 줄에 입력으로 주어진 두 문자열의 LCS의 길이를 출력한다.

Limit: none
*/

/* 
ACAYKP
CAPCAK
4

   A C A Y K P
C 0 1  1  1  1 1 
A 1  1  2 2 2 2  
P 1  1  2 2 2 3
C 1  2  2 2 2 3
A 1  2  3 3 3 3
K 1  2  3 3 4 4 

ACAK

*/

#define MAXSIZE 1001
int dp[MAXSIZE][MAXSIZE];
string arr[2];
int maximum = 0;

int DynamicProgramming()
{
    for (int i = 1; i <= arr[0].size(); i++)
    {
        for (int j = 1; j <= arr[1].size(); j++)
        {
            if (arr[0][i-1]== arr[1][j-1])
            {
                dp[i][j] = dp[i - 1][j - 1] + 1;
            }
            else
            {
                dp[i][j] = max(dp[i - 1][j], dp[i][j - 1]);
            }
        }
    }	
    return  dp[arr[0].size()][arr[1].size()];
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    for (int i = 0; i < 2; i++)
    {
        cin >> arr[i];
    }
    cout << DynamicProgramming() << '\n';

    return 0;
}