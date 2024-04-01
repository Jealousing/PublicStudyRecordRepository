#include <iostream>
#include <algorithm>
#include<vector>
#include<string>
using namespace std;

/*
Problem Number: 24060

Problem Description :
오늘도 서준이는 병합 정렬 수업 조교를 하고 있다. 아빠가 수업한 내용을 학생들이 잘 이해했는지 문제를 통해서 확인해보자.
N개의 서로 다른 양의 정수가 저장된 배열 A가 있다. 
병합 정렬로 배열 A를 오름차순 정렬할 경우 배열 A에 K 번째 저장되는 수를 구해서 우리 서준이를 도와주자.
크기가 N인 배열에 대한 병합 정렬 의사 코드는 다음과 같다.

Link: https://www.acmicpc.net/problem/24060

Input:
첫째 줄에 배열 A의 크기 N(5 ≤ N ≤ 500,000), 저장 횟수 K(1 ≤ K ≤ 108)가 주어진다.
다음 줄에 서로 다른 배열 A의 원소 A1, A2, ..., AN이 주어진다. (1 ≤ Ai ≤ 109)

Output:
배열 A에 K 번째 저장 되는 수를 출력한다. 저장 횟수가 K 보다 작으면 -1을 출력한다.

Limit: none
*/

#define MAXSIZE 500001
int value[MAXSIZE];
int temp[MAXSIZE];

int cnt = 0,k;
int result=0;

//p start, q mind , r end
void merge(int ary[], int start, int mid, int end, int tmp[])
{
	int i = start;
	int j = mid + 1;
	int t = 0;

	while (i <= mid && j <= end)
	{
		if (ary[i] <= ary[j]) tmp[t++] = ary[i++];
		else tmp[t++] = ary[j++];
	}
	while (i <= mid) 	tmp[t++] = ary[i++];
	while (j <= end) tmp[t++] = ary[j++];

	i = start;
	t = 0;

	while (i <= end)
	{
		cnt++;
		ary[i++] = tmp[t++];
		if (cnt == k) 
		{
			result = ary[i - 1];
		}
	}
}

void merge_sort(int ary[], int start, int end, int tmp[])
{
		if (start < end)
		{
			int mid  = (start + end) / 2;
			merge_sort(ary, start, mid, tmp);
			merge_sort(ary, mid + 1, end, tmp);
			merge(ary, start, mid, end,tmp);
		}
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int n;
	cin >> n >> k;

	for (int i = 0; i < n; i++)
	{
		int temp;
		cin >> temp;
		value[i] = temp;
	}
	merge_sort(value, 0, n - 1, temp);

	if (cnt < k) cout << -1 << '\n';
	else 	cout <<result<< endl;

	return 0;
}