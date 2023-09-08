#include <iostream>
using namespace std;

const int MAX_SIZE = 100;
/*
Problem Number: 2566

Problem Description :
<그림 1>과 같이 9×9 격자판에 쓰여진 81개의 자연수 또는 0이 주어질 때, 이들 중 최댓값을 찾고 
그 최댓값이 몇 행 몇 열에 위치한 수인지 구하는 프로그램을 작성하시오.
예를 들어, 다음과 같이 81개의 수가 주어지면
이들 중 최댓값은 90이고, 이 값은 5행 7열에 위치한다.

Link: https://www.acmicpc.net/problem/2566

Input:
첫째 줄부터 아홉 번째 줄까지 한 줄에 아홉 개씩 수가 주어진다. 주어지는 수는 100보다 작은 자연수 또는 0이다.

Output:
첫째 줄에 최댓값을 출력하고, 둘째 줄에 최댓값이 위치한 행 번호와 열 번호를 빈칸을 사이에 두고 차례로 출력한다.
최댓값이 두 개 이상인 경우 그 중 한 곳의 위치를 출력한다.


Limit: none
*/

int main()
{
    int Array[9][9];
    int max=0;
    int N, M;

    for (int n = 0; n < 9; n++)
    {
        for (int m = 0; m < 9; m++)
        {
            cin >> Array[n][m];

            if (max <= Array[n][m])
            {
                max = Array[n][m];
                N = n;
                M = m;
            }
        }
    }
    cout << max << endl;
    cout << N+1 << " " << M+1 << endl;
  


    return 0;
}