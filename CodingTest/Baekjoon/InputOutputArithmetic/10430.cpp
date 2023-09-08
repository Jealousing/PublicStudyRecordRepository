#include <iostream>
using namespace std;

/*
Problem Number: 10430

Problem Description : 
(A+B)%C는 ((A%C) + (B%C))%C 와 같을까?
(A×B)%C는 ((A%C) × (B%C))%C 와 같을까?
세 수 A, B, C가 주어졌을 때, 위의 네 가지 값을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10430

Input: 첫째 줄에 A, B, C가 순서대로 주어진다. (2 ≤ A, B, C ≤ 10000)

Output: 첫째 줄에 (A+B)%C, 둘째 줄에 ((A%C) + (B%C))%C, 셋째 줄에 (A×B)%C, 넷째 줄에 ((A%C) × (B%C))%C를 출력한다.
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