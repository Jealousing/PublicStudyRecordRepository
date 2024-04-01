#include <iostream>
#include <algorithm>
#include <cstring>
using namespace std;
/*
Problem Number: 11720

Problem Description :
N개의 숫자가 공백 없이 쓰여있다. 이 숫자를 모두 합해서 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11720

Input:
첫째 줄에 숫자의 개수 N (1 ≤ N ≤ 100)이 주어진다. 둘째 줄에 숫자 N개가 공백없이 주어진다.

Output:
입력으로 주어진 숫자 N개의 합을 출력한다.

Limit: none
*/


int main()
{
	int result = 0;
	int number;
	while (true)
	{
		cin >> number;
		if (1 <= number <= 100) break;
	}

	char string[101];
	cin >> string;

	for (int i = 0; i < number; i++)
	{
		result += string[i]-48;
	}
	cout << result << endl;


	return 0;
}