#include <iostream>
using namespace std;

/*
Problem Number: 10830

Problem Description :
크기가 N*N인 행렬 A가 주어진다. 이때, A의 B제곱을 구하는 프로그램을 작성하시오. 수가 매우 커질 수 있으니, A^B의 각 원소를 1,000으로 나눈 나머지를 출력한다.

Link: https://www.acmicpc.net/problem/10830

Input:
첫째 줄에 행렬의 크기 N과 B가 주어진다. (2 ≤ N ≤  5, 1 ≤ B ≤ 100,000,000,000)
둘째 줄부터 N개의 줄에 행렬의 각 원소가 주어진다. 행렬의 각 원소는 1,000보다 작거나 같은 자연수 또는 0이다.

Output: 
첫째 줄부터 N개의 줄에 걸쳐 행렬 A를 B제곱한 결과를 출력한다.

Limit: none

*/

/*
2 5
1 2
3 4

69 558
337 406

지수법칙과 모듈러성질 이용

짝수
a^8 = a^4 * a^4
       = a^2 * a^2 * a^2 ...
홀수
a^9 = a^4 * a^4 * a

*/

#define MAXSIZE 5
#define DIVIDEVALUE  1000
long long arr[MAXSIZE][MAXSIZE], temp[MAXSIZE][MAXSIZE],result[MAXSIZE][MAXSIZE];
long long n, m;

// 행렬 곱
void DivideAndConquer(long long arr1[MAXSIZE][MAXSIZE], long long arr2[MAXSIZE][MAXSIZE])
{
    for (int i = 0; i < n; i++) 
    {
        for (int j = 0; j < n; j++) 
        {
            temp[i][j] = 0; // 초기화
            for (int k = 0; k < n; k++) 
            {
                temp[i][j] += (arr1[i][k] * arr2[k][j]);
            }
            temp[i][j] %= DIVIDEVALUE;
        }
    }
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            arr1[i][j] = temp[i][j];
        }
    }
}


int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력
    cin >> n >> m;
    for (int i = 0; i < n; i++) 
    {
        for (int j = 0; j < n; j++)
        {
            cin >> arr[i][j];
        }
        result[i][i] = 1;
    }
   
    for (m; m > 0; m/= 2)
    {
        if (m % 2 == 1)
        {
            DivideAndConquer(result, arr);
        }
        DivideAndConquer(arr, arr);
    }

    // 출력
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++) 
        {
            cout << result[i][j] << " ";
        }
        cout << '\n';
    }

    return 0;
}