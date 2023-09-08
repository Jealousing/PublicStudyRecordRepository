#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 2750

Problem Description :
N개의 수가 주어졌을 때, 이를 오름차순으로 정렬하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2750

Input:
첫째 줄에 수의 개수 N(1 ≤ N ≤ 1,000)이 주어진다. 둘째 줄부터 N개의 줄에는 수가 주어진다. 이 수는 절댓값이 1,000보다 작거나 같은 정수이다. 수는 중복되지 않는다.

Output:
첫째 줄부터 N개의 줄에 오름차순으로 정렬한 결과를 한 줄에 하나씩 출력한다.

Limit: none
*/



int main()
{
	int N;
	int value[1000];
	cin >> N;

	for (int i = 0; i < N; i++)
	{
		cin >> value[i];
	}

	for (int i = 0; i < N-1; i++)
	{
		for (int j = i+1; j < N; j++)
		{
			if (value[i] > value[j])
			{
				int temp = value[i];
				value[i] = value[j];
				value[j] = temp;
			}
		}
	}

	for (int i = 0; i < N; i++)
	{
		cout << value[i] << endl;
	}

	return 0;
}