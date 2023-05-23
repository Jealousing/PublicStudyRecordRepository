#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 10989

Problem Description :
N개의 수가 주어졌을 때, 이를 오름차순으로 정렬하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10989

Input:
첫째 줄에 수의 개수 N(1 ≤ N ≤ 10,000,000)이 주어진다. 둘째 줄부터 N개의 줄에는 수가 주어진다. 이 수는 10,000보다 작거나 같은 자연수이다.

Output:
첫째 줄부터 N개의 줄에 오름차순으로 정렬한 결과를 한 줄에 하나씩 출력한다.

Limit: none
*/



int main()
{
	ios::sync_with_stdio(0); cin.tie(0);
	int N;
	int temp = 0;
	int value[10001]{0,};
	cin >> N;

	for (int i = 0; i < N; i++)
	{
		cin >> temp;
		value[temp] ++;
	}

	for (int i = 0; i < 10001; i++)
	{
		for (int j = 0; j < value[i]; j++)
		{
			cout << i << '\n';
		}
	}

	return 0;
}