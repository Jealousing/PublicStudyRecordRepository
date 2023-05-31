#include <iostream>
#include <algorithm>
#include<deque>
#include<string>
using namespace std;

/*
Problem Number: 10870

Problem Description :
피보나치 수는 0과 1로 시작한다. 0번째 피보나치 수는 0이고, 1번째 피보나치 수는 1이다. 그 다음 2번째 부터는 바로 앞 두 피보나치 수의 합이 된다.
이를 식으로 써보면 Fn = Fn-1 + Fn-2 (n ≥ 2)가 된다.
n=17일때 까지 피보나치 수를 써보면 다음과 같다.
0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597
n이 주어졌을 때, n번째 피보나치 수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10870

Input:
첫째 줄에 n이 주어진다. n은 20보다 작거나 같은 자연수 또는 0이다.

Output:
첫째 줄에 n번째 피보나치 수를 출력한다.

Limit: none
*/

long long int Fibonacci(long long int n)
{
	if (n == 0)return 0;
	else if (n <= 2) return 1;
	return Fibonacci(n-2) + Fibonacci(n - 1);
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int  n;
	cin >> n;
	cout << Fibonacci(n)<<'\n';

	return 0;
}