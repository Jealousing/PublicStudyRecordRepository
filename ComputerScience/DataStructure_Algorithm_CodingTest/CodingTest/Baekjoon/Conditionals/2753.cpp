#include <iostream>
using namespace std;

/*
Problem Number: 2753

Problem Description : 
������ �־����� ��, �����̸� 1, �ƴϸ� 0�� ����ϴ� ���α׷��� �ۼ��Ͻÿ�.
������ ������ 4�� ����̸鼭, 100�� ����� �ƴ� �� �Ǵ� 400�� ����� ���̴�.
���� ���, 2012���� 4�� ����̸鼭 100�� ����� �ƴ϶� �����̴�. 
1900���� 100�� ����̰� 400�� ����� �ƴϱ� ������ ������ �ƴϴ�. ������, 2000���� 400�� ����̱� ������ �����̴�.

Link: https://www.acmicpc.net/problem/2753

Input: ù° �ٿ� ������ �־�����. ������ 1���� ũ�ų� ����, 4000���� �۰ų� ���� �ڿ����̴�.

Output: ù° �ٿ� �����̸� 1, �ƴϸ� 0�� ����Ѵ�.

Limit: none
*/


bool IsinRange(int value)
{
    if (1 <= value <= 4000) return true;
    else return false;
}

int main()
{
    int year = 0;

    while (true)
    {
        cin >> year;

        if (IsinRange(year)) break;
    }
   
    if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0) cout << 1 << endl;
    else cout << 0 << endl;

	return 0;
}