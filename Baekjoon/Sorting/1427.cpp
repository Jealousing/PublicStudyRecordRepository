#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 1427

Problem Description :
배열을 정렬하는 것은 쉽다. 수가 주어지면, 그 수의 각 자리수를 내림차순으로 정렬해보자.

Link: https://www.acmicpc.net/problem/1427

Input:
첫째 줄에 정렬하려고 하는 수 N이 주어진다. N은 1,000,000,000보다 작거나 같은 자연수이다.

Output:
첫째 줄에 자리수를 내림차순으로 정렬한 수를 출력한다.

Limit: none
*/



int main()
{
	
	string N;
	cin >> N;

	for (int i = 0; i < N.size() - 1; i++)
	{
		for (int j = i + 1; j < N.size(); j++)
		{
			if (N[i] < N[j])
			{
				int temp = N[i];
				N[i] = N[j];
				N[j] = temp;
			}
		}
	}

	cout << N << endl;

	return 0;
}