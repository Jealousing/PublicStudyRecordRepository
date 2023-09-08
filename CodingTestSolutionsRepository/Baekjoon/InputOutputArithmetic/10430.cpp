#include <iostream>
using namespace std;

/*
Problem Number: 10430

Problem Description : 
(A+B)%C�� ((A%C) + (B%C))%C �� ������?
(A��B)%C�� ((A%C) �� (B%C))%C �� ������?
�� �� A, B, C�� �־����� ��, ���� �� ���� ���� ���ϴ� ���α׷��� �ۼ��Ͻÿ�.

Link: https://www.acmicpc.net/problem/10430

Input: ù° �ٿ� A, B, C�� ������� �־�����. (2 �� A, B, C �� 10000)

Output: ù° �ٿ� (A+B)%C, ��° �ٿ� ((A%C) + (B%C))%C, ��° �ٿ� (A��B)%C, ��° �ٿ� ((A%C) �� (B%C))%C�� ����Ѵ�.
*/

bool isInRange(int value)
{
	if (2 <= value <= 10000) return true;
	else return false;
}

int main()
{
	int A=0,B=0,C=0;
	while (true)
	{
		cin >> A>>B>>C;

		if (isInRange(A)&& isInRange(B)&& isInRange(C)) break;
	}

	
	cout << (A + B) % C << endl;
	cout << ((A % C) + (B % C)) % C << endl;
	cout << (A*B) % C << endl;
	cout << ((A % C) *(B % C)) % C << endl;

	return 0;
}