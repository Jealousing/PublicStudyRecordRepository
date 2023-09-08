#include <iostream>
#include <algorithm>
#include <string>
#include <set>
#include <vector>
using namespace std;

/*
Problem Number: 1735

Problem Description :
분수 A/B는 분자가 A, 분모가 B인 분수를 의미한다. A와 B는 모두 자연수라고 하자.
두 분수의 합 또한 분수로 표현할 수 있다. 두 분수가 주어졌을 때, 그 합을 기약분수의 형태로 구하는 프로그램을 작성하시오. 기약분수란 더 이상 약분되지 않는 분수를 의미한다.

Link: https://www.acmicpc.net/problem/1735

Input:
첫째 줄과 둘째 줄에, 각 분수의 분자와 분모를 뜻하는 두 개의 자연수가 순서대로 주어진다. 입력되는 네 자연수는 모두 30,000 이하이다.

Output:
첫째 줄에 구하고자 하는 기약분수의 분자와 분모를 뜻하는 두 개의 자연수를 빈 칸을 사이에 두고 순서대로 출력한다.

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int A1,A0, B1,B0;

	long long minCommonMultiple=1;

	cin >> A0 >> A1;
	cin >> B0 >> B1;

	long long high = A1 > B1 ? A1 : B1;

	int A = A1, B = B1;

	for (int j = 2; j <= high;)
	{
		if (A % j == 0 && B % j == 0)
		{
			minCommonMultiple *= j;
			A /= j;
			B /= j;
		}
		else
		{
			j++;
		}
	}

	high = minCommonMultiple *= A * B;
	int result = (A0 * minCommonMultiple / A1) + (B0 * minCommonMultiple / B1);

	for (int j = 2; j <= high;)
	{
		if (result % j == 0 && minCommonMultiple %j ==0)
		{
			result /= j;
			minCommonMultiple /= j;
		}
		else
		{
			j++;
		}
	}


	cout << result << ' ' << minCommonMultiple << '\n';

	return 0;
}