#include <iostream>
#include <algorithm>
#include <cstring>
using namespace std;
/*
Problem Number: 2743

Problem Description :
알파벳으로만 이루어진 단어를 입력받아, 그 길이를 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2743

Input: 
첫째 줄에 영어 소문자와 대문자로만 이루어진 단어가 주어진다. 단어의 길이는 최대 100이다.

Output: 
첫째 줄에 입력으로 주어진 단어의 길이를 출력한다.

Limit: none
*/


int main()
{
	char string[101];

	cin >> string;
	cout << strlen(string) << endl;

	return 0;
}