#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
#include <vector>
using namespace std;

/*
Problem Number: 10872

Problem Description :
0보다 크거나 같은 정수 N이 주어진다. 이때, N!을 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10872

Input:
첫째 줄에 정수 N(0 ≤ N ≤ 12)이 주어진다.

Output:
첫째 줄에 N!을 출력한다.

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int n,result=1;
	cin >> n;

	for (int i = n; i > 0; i--)
	{
		result *= i;
	}
	cout << result << '\n';
	return 0;
}