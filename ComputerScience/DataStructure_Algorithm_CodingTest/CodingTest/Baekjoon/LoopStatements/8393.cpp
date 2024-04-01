#include <iostream>
using namespace std;

/*
Problem Number: 8393

Problem Description : n이 주어졌을 때, 1부터 n까지 합을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/8393

Input: 첫째 줄에 n (1 ≤ n ≤ 10,000)이 주어진다.

Output: 1부터 n까지 합을 출력한다.

Limit: none
*/

int main()
{
    int N;

    while (true)
    {
        cin >> N;

        if (1 <= N<=10000) break;
    }
    int result = 0;
    for (int i = 1; i <= N; i++)
    {
        result += i;
    }
    cout << result << endl;

	return 0;
}