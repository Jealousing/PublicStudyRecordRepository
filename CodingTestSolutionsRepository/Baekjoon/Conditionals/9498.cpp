#include <iostream>
using namespace std;

/*
Problem Number: 9498

Problem Description : 
시험 점수를 입력받아 90 ~ 100점은 A, 80 ~ 89점은 B, 70 ~ 79점은 C, 60 ~ 69점은 D, 나머지 점수는 F를 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/9498

Input: 첫째 줄에 시험 점수가 주어진다. 시험 점수는 0보다 크거나 같고, 100보다 작거나 같은 정수이다.

Output: 시험 성적을 출력한다.

Limit: none
*/


bool IsinRange(int value)
{
    if (0 <= value <= 100) return true;
    else return false;
}

int main()
{
    int score = 0;

    while (true)
    {
        cin >> score;

        if (IsinRange(score)) break;
    }
   

    if (90 <= score) cout << "A" << endl;
    else if (80 <= score) cout << "B" << endl;
    else if (70 <= score) cout << "C" << endl;
    else if (60 <= score) cout << "D" << endl;
    else cout << "F" << endl;

	return 0;
}