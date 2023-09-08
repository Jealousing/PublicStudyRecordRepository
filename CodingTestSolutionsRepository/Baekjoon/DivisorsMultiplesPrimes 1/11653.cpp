#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 11653

Problem Description :
정수 N이 주어졌을 때, 소인수분해하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11653

Input:
첫째 줄에 정수 N (1 ≤ N ≤ 10,000,000)이 주어진다.

Output:
N의 소인수분해 결과를 한 줄에 하나씩 오름차순으로 출력한다. N이 1인 경우 아무것도 출력하지 않는다.

Limit: none
*/

int main()
{
	int N = 0;
	int min = 2;
	cin >> N;

	if (N == 1) return 0;

	while (true)
	{
		if (N % min == 0)
		{
			cout << min << endl;
			if (N == 0 || N == min) break;
			N /= min;
		}
		else
		{
			min++;
			if (N == min)
			{
				cout << N << endl;
				break;
			}
		}
	}

	return 0;
}