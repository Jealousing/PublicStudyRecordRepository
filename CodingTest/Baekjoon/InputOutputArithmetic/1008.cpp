#include <iostream>
using namespace std;

/*
Problem Number: 1008
Problem Description : �� ���� A�� B�� �Է¹��� ����, A/B�� ����ϴ� ���α׷��� �ۼ��Ͻÿ�.
link: https://www.acmicpc.net/problem/1008
input: ù° �ٿ� A�� B�� �־�����. (0 < A, B < 10)
output: ù ��° �ٿ� A/B�� ����մϴ�. ���� ����� ��°��� ������� �Ǵ� �������� sqrt(10E-9)�����̸� �����̴�.
*/

int main()
{
	double A=0, B=0;
	while (true)
	{
		cin >> A>> B;
	
		if (0 < A && B < 10) break;
	}
	// sqrt(10E-9)���ϸ� ��Ű�� ���� �Ҽ����Ʒ� 10�ڸ����� ǥ���ϵ��� ����
	cout.precision(10);
	cout << A/B << endl;
	return 0;
}