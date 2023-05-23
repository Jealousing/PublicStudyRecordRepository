#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 1181

Problem Description :
알파벳 소문자로 이루어진 N개의 단어가 들어오면 아래와 같은 조건에 따라 정렬하는 프로그램을 작성하시오.
길이가 짧은 것부터
길이가 같으면 사전 순으로
단, 중복된 단어는 하나만 남기고 제거해야 한다.

Link: https://www.acmicpc.net/problem/1181

Input:
첫째 줄에 단어의 개수 N이 주어진다. (1 ≤ N ≤ 20,000) 둘째 줄부터 N개의 줄에 걸쳐 알파벳 소문자로 이루어진 단어가 한 줄에 하나씩 주어진다. 
주어지는 문자열의 길이는 50을 넘지 않는다.

Output:
조건에 따라 정렬하여 단어들을 출력한다.

Limit: none
*/

const int MAX_SIZE = 20001;
string str[MAX_SIZE];
int compare(string a, string b)
{
	//길이가 짧은 것부터
	//길이가 같으면 사전 순으로
	if (a.size() == b.size()) return a < b;
	else return a.size() < b.size();
}

int main()
{
	int N;
	cin >> N;

	for (int i = 0; i < N; i++)
	{
		cin >> str[i];
	}

	// 정렬 알고리즘사용
	sort(str, str+N, compare);

	for (int i = 0; i < N; i++)
	{
		if (str[i] == str[i - 1]) {
			continue;
		}
		cout << str[i] << '\n';
	}

	return 0;
}