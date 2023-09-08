#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 10101

Problem Description :
창영이는 삼각형의 종류를 잘 구분하지 못한다. 따라서 프로그램을 이용해 이를 외우려고 한다.
삼각형의 세 각을 입력받은 다음,
세 각의 크기가 모두 60이면, Equilateral
세 각의 합이 180이고, 두 각이 같은 경우에는 Isosceles
세 각의 합이 180이고, 같은 각이 없는 경우에는 Scalene
세 각의 합이 180이 아닌 경우에는 Error
를 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10101

Input:
총 3개의 줄에 걸쳐 삼각형의 각의 크기가 주어진다. 모든 정수는 0보다 크고, 180보다 작다.

Output:
문제의 설명에 따라 Equilateral, Isosceles, Scalene, Error 중 하나를 출력한다.

Limit: none
*/



int main()
{
	int angle[3];
	int count = 0;
	int sum = 0;

	for (int i = 0; i < 3; i++)
	{
		cin >> angle[i];
		sum += angle[i];
		if (angle[i] == 60) count++;
	}

	if (count == 3) cout << "Equilateral" << endl;
	else if (sum==180 && (angle[0]==angle[1] || angle[0]==angle[2] || angle[1]==angle[2]))  cout << "Isosceles" << endl;
	else if(sum == 180 &&  angle[0]!=angle[1]!=angle[2]) cout << "Scalene" << endl;
	else if (sum != 180) cout << "Error" << endl;

	return 0;
}