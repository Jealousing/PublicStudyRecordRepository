#include <iostream>
#include <algorithm>
using namespace std;

/*
Problem Number: 10844

Problem Description :
45656이란 수를 보자.
이 수는 인접한 모든 자리의 차이가 1이다. 이런 수를 계단 수라고 한다.
N이 주어질 때, 길이가 N인 계단 수가 총 몇 개 있는지 구해보자. 0으로 시작하는 수는 계단수가 아니다.

Link: https://www.acmicpc.net/problem/10844

Input:
첫째 줄에 N이 주어진다. N은 1보다 크거나 같고, 100보다 작거나 같은 자연수이다.

Output:
첫째 줄에 정답을 1,000,000,000으로 나눈 나머지를 출력한다.

Limit: none
*/

/*
n      
1      1~9                                                                                                         
2     10 12 21 23 32 34 43 45 54 56 65 67 76 78 87 89 98                           
3      101 121 123 210 212 232 ...                 
*/

#define MAXSIZE 101
int arr[MAXSIZE][10];
int n;

int dp()
{
    // 한자리수 1~9
    for (int i = 1; i <= 9; i++) arr[1][i] = 1;

    for (int i = 2; i <= n; i++)
    {
        for (int j = 0; j <= 9; j++)
        {
            // 0으로 끝나는 계단의 수는 전 자리수가 1로 끝나는것만 가능
            if (j == 0 ) arr[i][j] = arr[i - 1][1] % 1000000000;
            // 9로 끝나는 계단의 수는 전 자리수가 8로 끝나는 것만 가능
            else if (j == 9) arr[i][j] = arr[i - 1][8] % 1000000000;
            // 1~8 전 자리수에 j-1 or j+1이오면 올수있다.
            else arr[i][j] = (arr[i - 1][j - 1] + arr[i - 1][j + 1]) % 1000000000;
        }
    }

    int Answer = 0;
    for (int i = 0; i <= 9; i++)
    {
        Answer = (Answer + arr[n][i])% 1000000000;
    }
    return Answer;
    

}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    cin >> n;
    cout << dp() << '\n';

    return 0;
}