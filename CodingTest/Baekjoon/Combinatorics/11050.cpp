#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
#include <vector>
using namespace std;

/*
Problem Number: 11050

Problem Description :
자연수 N과 정수 K가 주어졌을 때 이항 계수(NK)를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11050

Input:
첫째 줄에 N과 K가 주어진다. (1 ≤ N ≤ 10, 0 ≤ K ≤ N)

Output:
(NK)를 출력한다.

Limit: none
*/

int Factorial(int n)
{
	int result = 1;
	for (int i = n; i > 0; i--)
	{
		result *= i;
	}
	return result;
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int n,k,count=0;
	cin >> n>>k;

	

	cout << Factorial(n) / (Factorial(k) * Factorial(n - k)) << '\n';

	return 0;
}