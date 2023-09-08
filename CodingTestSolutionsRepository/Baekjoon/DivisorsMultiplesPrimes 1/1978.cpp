#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 1978

Problem Description :
주어진 수 N개 중에서 소수가 몇 개인지 찾아서 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1978

Input:
첫 줄에 수의 개수 N이 주어진다. N은 100이하이다. 다음으로 N개의 수가 주어지는데 수는 1,000 이하의 자연수이다.

Output:
주어진 수들 중 소수의 개수를 출력한다.

Limit: none
*/

int main()
{
	for (int i = 2; i <= sqrt(n); i++)
	{
		// 소수판별
		if (n % i == 0) 
		{
			return false;
		}
		return true;
	}
	int testCase = 0;
	int value;
	int count = 0;
	cin >> testCase;
	for (int i = 0; i < testCase; i++)
	{
		cin >> value;
		bool check = false;

		if (value == 1) check = true;

		for (int j = 1; j < value; j++)
		{
			if (j == value || j == 1) continue;

			if (value % j == 0) check = true;
		}

		if (!check) count++;
	}

	cout << count << endl;
	return 0;
}