#include <iostream>
using namespace std;

/*
Problem Number: 2588

Problem Description : 
(�� �ڸ� ��) �� (�� �ڸ� ��)�� ������ ���� ������ ���Ͽ� �̷������.
https://www.acmicpc.net/upload/images/f5NhGHVLM4Ix74DtJrwfC97KepPl27s%20(1).png
(1)�� (2)��ġ�� �� �� �ڸ� �ڿ����� �־��� �� (3), (4), (5), (6)��ġ�� �� ���� ���ϴ� ���α׷��� �ۼ��Ͻÿ�.

Link: https://www.acmicpc.net/problem/2588

Input: ù° �ٿ� (1)�� ��ġ�� �� �� �ڸ� �ڿ�����, ��° �ٿ� (2)�� ��ġ�� �� ���ڸ� �ڿ����� �־�����.)

Output: ù° �ٺ��� ��° �ٱ��� ���ʴ�� (3), (4), (5), (6)�� �� ���� ����Ѵ�
*/

int main()
{
	int A = 0, B = 0;
	cin >> A;
	cin>> B;

	cout << A * (B % 10) << endl;
	cout << A * (B % 100/10) << endl; 
	cout << A * (B / 100) << endl; 
	cout << A *B << endl;

	return 0;
}