#include <iostream>
#include <algorithm>
#include<deque>
#include<string>
using namespace std;

/*
Problem Number: 27433

Problem Description :
0보다 크거나 같은 정수 N이 주어진다. 이때, N!을 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/27433

Input:
첫째 줄에 정수 N(0 ≤ N ≤ 20)이 주어진다.

Output:
첫째 줄에 N!을 출력한다.

Limit: none
*/

long long int factorial(long long int n)
{
	if (n == 0) return 1;
	return n * factorial(n - 1);
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int  n;
	cin >> n;
	cout << factorial(n)<<'\n';

	return 0;
}