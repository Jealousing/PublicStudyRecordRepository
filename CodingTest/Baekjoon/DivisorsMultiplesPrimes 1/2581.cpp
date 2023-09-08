#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 2581

Problem Description :
자연수 M과 N이 주어질 때 M이상 N이하의 자연수 중 소수인 것을 모두 골라 이들 소수의 합과 최솟값을 찾는 프로그램을 작성하시오.
예를 들어 M=60, N=100인 경우 60이상 100이하의 자연수 중 소수는 61, 67, 71, 73, 79, 83, 89, 97 총 8개가 있으므로, 이들 소수의 합은 620이고, 최솟값은 61이 된다.

Link: https://www.acmicpc.net/problem/2581

Input:
입력의 첫째 줄에 M이, 둘째 줄에 N이 주어진다.
M과 N은 10,000이하의 자연수이며, M은 N보다 작거나 같다.

Output:
M이상 N이하의 자연수 중 소수인 것을 모두 찾아 첫째 줄에 그 합을, 둘째 줄에 그 중 최솟값을 출력한다.
단, M이상 N이하의 자연수 중 소수가 없을 경우는 첫째 줄에 -1을 출력한다.

Limit: none
*/

int main()
{
	int M, N;
	int sum=0, min=10001;
	
	cin >> M;
	cin >> N;

	for (int i = M; i <= N; i++)
	{
		bool check = false;

		if (i == 1) check = true;

		for (int j = 1; j <= i; j++)
		{
			if (j == i || j == 1) continue;

			if (i % j == 0) check = true;
		}

		if (!check)
		{
			sum += i;
			if (i <= min)
			{
				min = i;
			}
		}
	}
	if (min != 10001)
	{
		cout << sum << endl;
		cout << min << endl;
	}
	else
	{
		cout << -1 << endl;
	}

	return 0;
}