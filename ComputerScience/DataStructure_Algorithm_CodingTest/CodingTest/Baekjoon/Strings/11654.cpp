#include <iostream>
#include <algorithm>
#include <cstring>
using namespace std;
/*
Problem Number: 11654

Problem Description :
알파벳 소문자, 대문자, 숫자 0-9중 하나가 주어졌을 때, 주어진 글자의 아스키 코드값을 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11654

Input:
알파벳 소문자, 대문자, 숫자 0-9 중 하나가 첫째 줄에 주어진다.

Output:
입력으로 주어진 글자의 아스키 코드 값을 출력한다.

Limit: none
*/


int main()
{
	char string;
	cin >> string;

	cout << (int)string << endl;


	return 0;
}