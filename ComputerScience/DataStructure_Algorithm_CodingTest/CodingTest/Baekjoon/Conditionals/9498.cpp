#include <iostream>
using namespace std;

/*
Problem Number: 9498

Problem Description : 
���� ������ �Է¹޾� 90 ~ 100���� A, 80 ~ 89���� B, 70 ~ 79���� C, 60 ~ 69���� D, ������ ������ F�� ����ϴ� ���α׷��� �ۼ��Ͻÿ�.

Link: https://www.acmicpc.net/problem/9498

Input: ù° �ٿ� ���� ������ �־�����. ���� ������ 0���� ũ�ų� ����, 100���� �۰ų� ���� �����̴�.

Output: ���� ������ ����Ѵ�.

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