#include <iostream>
#include <algorithm>
#include <cstring>
using namespace std;
/*
Problem Number: 27866

Problem Description :
단어 S와 정수 i가 주어졌을 때, S의 i번째 글자를 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/27866

Input: 
첫째 줄에 영어 소문자와 대문자로만 이루어진 단어 S가 주어진다. 단어의 길이는 최대 1,000이다.
둘째 줄에 정수 i가 주어진다. (1 <= i <=|S|)

Output: 
첫째 줄에 새로운 평균을 출력한다. 실제 정답과 출력값의 절대오차 또는 상대오차가 10^-2 이하이면 정답이다.

Limit: none
*/


int main()
{
	char string[1001];
	int num;

	cin >> string;
	while (true)
	{
		cin >> num;
		if (1 <= num && num <= strlen(string)) break;
	}
	
	cout << string[num-1] << endl;

	return 0;
}