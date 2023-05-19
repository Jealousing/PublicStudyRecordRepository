#include <iostream>
using namespace std;
const int MAX_SIZE = 10000;
/*
Problem Number: 10871

Problem Description :
정수 N개로 이루어진 수열 A와 정수 X가 주어진다. 이때, A에서 X보다 작은 수를 모두 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10871

Input: 
첫째 줄에 N과 X가 주어진다. (1 ≤ N, X ≤ 10,000)
둘째 줄에 수열 A를 이루는 정수 N개가 주어진다. 주어지는 정수는 모두 1보다 크거나 같고, 10,000보다 작거나 같은 정수이다.

Output: X보다 작은 수를 입력받은 순서대로 공백으로 구분해 출력한다. X보다 작은 수는 적어도 하나 존재한다.

Limit: none
*/

int main()
{
    int N, A[MAX_SIZE], X;

	while (true)
	{
		cin >> N >> X;

		if (1 <= N && X<=10000) break;
	}

	for (int i = 0; i < N; i++)
	{
		cin >> A[i];
	}

	for (int i = 0; i < N; i++)
	{
		if (A[i] < X) cout << A[i] << " ";
	}
	cout << endl;
   
	return 0;
}