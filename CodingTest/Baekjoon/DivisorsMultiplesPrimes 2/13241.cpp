﻿#include <iostream>
#include <algorithm>
#include <string>
#include <set>
#include <vector>
using namespace std;

/*
Problem Number: 13241

Problem Description :
정수 B에 0보다 큰 정수인 N을 곱해 정수 A를 만들 수 있다면, A는 B의 배수이다.
예:
10은 5의 배수이다 (5*2 = 10)
10은 10의 배수이다(10*1 = 10)
6은 1의 배수이다(1*6 = 6)
20은 1, 2, 4,5,10,20의 배수이다.
다른 예:
2와 5의 최소공배수는 10이고, 그 이유는 2와 5보다 작은 공배수가 없기 때문이다.
10과 20의 최소공배수는 20이다.
5와 3의 최소공배수는 15이다.
당신은 두 수에 대하여 최소공배수를 구하는 프로그램을 작성 하는 것이 목표이다.

Link: https://www.acmicpc.net/problem/13241

Input:
한 줄에 두 정수 A와 B가 공백으로 분리되어 주어진다.
50%의 입력 중 A와 B는 1000(103)보다 작다. 다른 50%의 입력은 1000보다 크고 100000000(108)보다 작다.
추가: 큰 수 입력에 대하여 변수를 64비트 정수로 선언하시오. C/C++에서는 long long int를 사용하고, Java에서는 long을 사용하시오.

Output:
A와 B의 최소공배수를 한 줄에 출력한다.

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	long long int A, B;

	long long result=1;

	cin >> A >> B;

	long long high = A > B ? A : B;

	for (int j = 2; j <= high;)
	{
		if (A % j == 0 && B % j == 0)
		{
			result *= j;
			A /= j;
			B /= j;

		}
		else
		{
			j++;
		}
	}

	result *= A * B;

	cout << result << '\n';

	return 0;
}