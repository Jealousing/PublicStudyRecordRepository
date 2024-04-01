#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 5073

Problem Description :
삼각형의 세 변의 길이가 주어질 때 변의 길이에 따라 다음과 같이 정의한다.
Equilateral :  세 변의 길이가 모두 같은 경우
Isosceles : 두 변의 길이만 같은 경우
Scalene : 세 변의 길이가 모두 다른 경우

단 주어진 세 변의 길이가 삼각형의 조건을 만족하지 못하는 경우에는 "Invalid" 를 출력한다. 예를 들어 6, 3, 2가 이 경우에 해당한다. 
가장 긴 변의 길이보다 나머지 두 변의 길이의 합이 길지 않으면 삼각형의 조건을 만족하지 못한다.
세 변의 길이가 주어질 때 위 정의에 따른 결과를 출력하시오.

Link: https://www.acmicpc.net/problem/5073

Input:
각 줄에는 1,000을 넘지 않는 양의 정수 3개가 입력된다. 마지막 줄은 0 0 0이며 이 줄은 계산하지 않는다.

Output:
각 입력에 맞는 결과 (Equilateral, Isosceles, Scalene, Invalid) 를 출력하시오.

Limit: none
*/



int main()
{
	while (true)
	{
		int length[3];
		int maxLength = 0;
		cin >> length[0] >> length[1] >> length[2];

		if (length[0] == 0|| length[1] == 0|| length[2] == 0)break;

		bool isCheck = false;
		int checkCount = 0;
		for (int i = 0; i < 3; i++)
		{
			if (maxLength < length[i])
			{
				maxLength = length[i];
				checkCount = i;
			}
		}
		if (checkCount == 0 && maxLength >= length[1] + length[2]) isCheck = true;
		else if (checkCount == 1 && maxLength >= length[0] + length[2]) isCheck = true;
		else if (checkCount == 2 && maxLength >= length[0] + length[1]) isCheck = true;

		if (isCheck) cout << "Invalid" << endl;
		else
		{
			if (length[0] == length[1] && length[0] == length[2] && length[1] == length[2]) cout << "Equilateral" << endl;
			else if ((length[0] == length[1]) || (length[0] == length[2]) || (length[1] == length[2])) cout << "Isosceles" << endl;
			else cout << "Scalene" << endl;
		}

	
	}


	return 0;
}