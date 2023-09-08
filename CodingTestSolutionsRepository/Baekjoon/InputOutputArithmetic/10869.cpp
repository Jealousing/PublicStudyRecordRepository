#include <iostream>
using namespace std;

/*
Problem Number: 10869
Problem Description : �� �ڿ��� A�� B�� �־�����. �̶�, A+B, A-B, A*B, A/B(��), A%B(������)�� ����ϴ� ���α׷��� �ۼ��Ͻÿ�. 
link: https://www.acmicpc.net/problem/10869
input: �� �ڿ��� A�� B�� �־�����. (1 �� A, B �� 10,000)
output: ù° �ٿ� A+B, ��° �ٿ� A-B, ��° �ٿ� A*B, ��° �ٿ� A/B, �ټ�° �ٿ� A%B�� ����Ѵ�.
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