#include <iostream>
#include <algorithm>
using namespace std;
/*
Problem Number: 10818

Problem Description :
N개의 정수가 주어진다. 이때, 최솟값과 최댓값을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10818

Input: 
첫째 줄에 정수의 개수 N (1 ≤ N ≤ 1,000,000)이 주어진다. 둘째 줄에는 N개의 정수를 공백으로 구분해서 주어진다.
모든 정수는 -1,000,000보다 크거나 같고, 1,000,000보다 작거나 같은 정수이다.

Output: 첫째 줄에 주어진 정수 N개의 최솟값과 최댓값을 공백으로 구분해 출력한다.

Limit: none
*/

int main()
{
	/* 생각해보니 이 방법은 배열을 사용을 하지않아서 수정
	int N, A, min = 1000000 , max = -1000001;

	while (true)
	{
		cin >> N;

		if (1 <= N <=1000000) break;
	}

	for (int i = 0; i < N; i++)
	{
		cin >> A;

		if (-1000000 <= A <= 1000000)
		{
			if (A < min) min = A;
			if (max < A) max = A;
		}
	}

	cout << min << " " << max << endl;
   */
	int N, A[1000001];

	while (true)
	{
		cin >> N;

		if (1 <= N <= 1000000) break;
	}

	for (int i = 0; i < N; i++)
	{
		cin >> A[i];
	}

	sort(A, A + N);

	cout << A[0] << " " << A[N - 1];

	return 0;
}