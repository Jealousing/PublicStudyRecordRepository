#include <iostream>
using namespace std;

/*
Problem Number: 10998
Problem Description : �� ���� A�� B�� �Է¹��� ����, A��B�� ����ϴ� ���α׷��� �ۼ��Ͻÿ�.
link: https://www.acmicpc.net/problem/10998
input: ù° �ٿ� A�� B�� �־�����. (0 < A, B < 10)
output: ù° �ٿ� A��B�� ����Ѵ�.
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