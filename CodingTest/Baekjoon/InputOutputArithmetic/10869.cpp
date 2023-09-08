#include <iostream>
using namespace std;

/*
Problem Number: 10869
Problem Description : 두 자연수 A와 B가 주어진다. 이때, A+B, A-B, A*B, A/B(몫), A%B(나머지)를 출력하는 프로그램을 작성하시오. 
link: https://www.acmicpc.net/problem/10869
input: 두 자연수 A와 B가 주어진다. (1 ≤ A, B ≤ 10,000)
output: 첫째 줄에 A+B, 둘째 줄에 A-B, 셋째 줄에 A*B, 넷째 줄에 A/B, 다섯째 줄에 A%B를 출력한다.
*/

int main()
{
	int A=0, B=0;
	while (true)
	{
		cin >> A>> B;
	
		if (1<= A && B <= 10000) break;
	}

	cout << A + B << endl;
	cout << A - B << endl;
	cout << A * B << endl;
	cout << A / B << endl;
	cout << A % B << endl;

	return 0;
}