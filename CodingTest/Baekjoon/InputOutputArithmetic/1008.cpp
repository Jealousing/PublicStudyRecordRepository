#include <iostream>
using namespace std;

/*
Problem Number: 1008
Problem Description : 두 정수 A와 B를 입력받은 다음, A/B를 출력하는 프로그램을 작성하시오.
link: https://www.acmicpc.net/problem/1008
input: 첫째 줄에 A와 B가 주어진다. (0 < A, B < 10)
output: 첫 번째 줄에 A/B를 출력합니다. 실제 정답과 출력값의 절대오차 또는 상대오차가 sqrt(10E-9)이하이면 정답이다.
*/

int main()
{
	double A=0, B=0;
	while (true)
	{
		cin >> A>> B;
	
		if (0 < A && B < 10) break;
	}
	// sqrt(10E-9)이하를 지키기 위해 소수점아래 10자리까지 표현하도록 설정
	cout.precision(10);
	cout << A/B << endl;
	return 0;
}