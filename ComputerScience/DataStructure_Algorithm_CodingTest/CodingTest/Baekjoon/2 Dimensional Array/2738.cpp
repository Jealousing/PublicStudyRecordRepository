#include <iostream>
using namespace std;

const int MAX_SIZE = 100;
/*
Problem Number: 2738

Problem Description :
N*M크기의 두 행렬 A와 B가 주어졌을 때, 두 행렬을 더하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2738

Input:
첫째 줄에 행렬의 크기 N 과 M이 주어진다. 둘째 줄부터 N개의 줄에 행렬 A의 원소 M개가 차례대로 주어진다. 
이어서 N개의 줄에 행렬 B의 원소 M개가 차례대로 주어진다. N과 M은 100보다 작거나 같고, 행렬의 원소는 절댓값이 100보다 작거나 같은 정수이다.

Output:
첫째 줄부터 N개의 줄에 행렬 A와 B를 더한 행렬을 출력한다. 행렬의 각 원소는 공백으로 구분한다.


Limit: none
*/

int main()
{
    int Array1[MAX_SIZE][MAX_SIZE];
    int Array2[MAX_SIZE][MAX_SIZE];

    int N, M;

    cin >> N >> M;

    for (int n = 0; n < N; n++)
    {
        for (int m = 0; m < M; m++)
        {
            cin >> Array1[n][m];
        }
    }

    for (int n = 0; n < N; n++)
    {
        for (int m = 0; m < M; m++)
        {
            cin >> Array2[n][m];
        }
    }

    for (int n = 0; n < N; n++)
    {
        for (int m = 0; m < M; m++)
        {
            Array1[n][m] += Array2[n][m];
            cout << Array1[n][m]<< " ";
        }
        cout << endl;
    }


    return 0;
}