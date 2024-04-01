#include <iostream>
using namespace std;

/*
Problem Number: 2438

Problem Description :
첫째 줄에는 별 1개, 둘째 줄에는 별 2개, N번째 줄에는 별 N개를 찍는 문제

Link: https://www.acmicpc.net/problem/2438

Input: 
첫째 줄에 N(1 ≤ N ≤ 100)이 주어진다.

Output: 첫째 줄부터 N번째 줄까지 차례대로 별을 출력한다.

Limit: none
*/

int main()
{
    int N;
    
    while (true)
    {
        cin >> N;

        if (1 <= N<=100) break;
    }

    for (int i = 0; i < N; i++)
    {
        for (int j = 0; j <= i; j++)
        {
            cout << "*";
        }
        cout << endl;
    }
   
	return 0;
}