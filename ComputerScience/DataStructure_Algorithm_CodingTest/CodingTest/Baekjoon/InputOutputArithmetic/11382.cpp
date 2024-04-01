#include <iostream>
using namespace std;

/*
Problem Number: 11382

Problem Description : 
꼬마 정민이는 이제 A + B 정도는 쉽게 계산할 수 있다. 이제 A + B + C를 계산할 차례이다!

Link: https://www.acmicpc.net/problem/11382

Input: 첫 번째 줄에 A, B, C (1 ≤ A, B, C ≤  pow(10,12))이 공백을 사이에 두고 주어진다.

Output: A+B+C의 값을 출력한다.
*/


int main()
{
	long long A = 0, B = 0, C = 0;

	cin >> A >> B >> C;

	cout << A + B + C << endl;

	return 0;
}