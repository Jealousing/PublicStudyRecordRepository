#include <iostream>
using namespace std;

/*
Problem Number: 10998
Problem Description : 두 정수 A와 B를 입력받은 다음, A×B를 출력하는 프로그램을 작성하시오.
link: https://www.acmicpc.net/problem/10998
input: 첫째 줄에 A와 B가 주어진다. (0 < A, B < 10)
output: 첫째 줄에 A×B를 출력한다.
*/

int main()
{
	int A=0, B=0;
	while (true)
	{
		cin >> A>> B;
	
		if (0 < A && B < 10) break;
	}
	cout << A*B << endl;
	return 0;
}