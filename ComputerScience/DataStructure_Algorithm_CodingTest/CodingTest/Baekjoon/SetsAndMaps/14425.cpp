#include <iostream>
#include <algorithm>
#include <vector>
#include <map>
using namespace std;

/*
Problem Number: 14425

Problem Description :
총 N개의 문자열로 이루어진 집합 S가 주어진다.
입력으로 주어지는 M개의 문자열 중에서 집합 S에 포함되어 있는 것이 총 몇 개인지 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/14425

Input:
첫째 줄에 문자열의 개수 N과 M (1 ≤ N ≤ 10,000, 1 ≤ M ≤ 10,000)이 주어진다.
다음 N개의 줄에는 집합 S에 포함되어 있는 문자열들이 주어진다.
다음 M개의 줄에는 검사해야 하는 문자열들이 주어진다.
입력으로 주어지는 문자열은 알파벳 소문자로만 이루어져 있으며, 길이는 500을 넘지 않는다. 집합 S에 같은 문자열이 여러 번 주어지는 경우는 없다.

Output:
첫째 줄에 M개의 문자열 중에 총 몇 개가 집합 S에 포함되어 있는지 출력한다.

Limit: none
*/


int main()
{
	int N, M, count = 0;
	cin >> N>>M;

	string tempValue;
	vector<string> data1, data2;

	for (int i = 0; i < N; i++)
	{
		cin >> tempValue;
		data1.push_back(tempValue);
	}
	// 정렬 
	sort(data1.begin(), data1.end());

	for (int i = 0; i < M; i++)
	{
		cin >> tempValue;
		data2.push_back(tempValue);
	}

	for (int i = 0; i < M; i++)
	{
		if (binary_search(data1.begin(), data1.end(), data2[i])) count++;
	}
	cout << count << endl;
	return 0;
}