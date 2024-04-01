#include <iostream>
using namespace std;

/*
Problem Number: 18108

Problem Description : 
ICPC Bangkok Regional�� �����ϱ� ���� ���ϳ�ǰ �������׿� �� ������ �� �������Ʈ ������ ���� ���� �� ������. 
������ ���� ��ũ���� ���ذ� 2562���̶�� ���� �ִ� ���̾���.
�ұ� ������ �±��� �Ҹ���, �� ������ϰ� ������ �ظ� �������� ������ ���� �ұ⸦ ����Ѵ�. 
�ݸ�, �츮����� ���� ������ ����ϰ� �ִ�. �ұ� ������ �־��� �� �̸� ���� ������ �ٲ� �ִ� ���α׷��� �ۼ��Ͻÿ�.

link: https://www.acmicpc.net/problem/18108

input: ���� ������ �˾ƺ��� ���� �ұ� ���� y�� �־�����. (1000 �� y �� 3000)

output: �ұ� ������ ���� ������ ��ȯ�� ����� ����Ѵ�.
*/

int main()
{
	int year;
	while (true)
	{
		cin >> year;

		if (1000 <= year <= 3000) break;
	}

	cout << year - 543 << endl;

	return 0;
}