#include <iostream>
#include <algorithm>
#include <string>
using namespace std;
/*
Problem Number: 1152

Problem Description :
영어 대소문자와 공백으로 이루어진 문자열이 주어진다. 이 문자열에는 몇 개의 단어가 있을까?
이를 구하는 프로그램을 작성하시오. 단, 한 단어가 여러 번 등장하면 등장한 횟수만큼 모두 세어야 한다

Link: https://www.acmicpc.net/problem/1152

Input:
첫 줄에 영어 대소문자와 공백으로 이루어진 문자열이 주어진다. 이 문자열의 길이는 1,000,000을 넘지 않는다.
단어는 공백 한 개로 구분되며, 공백이 연속해서 나오는 경우는 없다. 또한 문자열은 공백으로 시작하거나 끝날 수 있다.

Output:
첫째 줄에 단어의 개수를 출력한다.

Limit: none
*/


int main()
{
    int count = 0;
    string str;
    getline(cin, str);

    if (str.empty())
    {
        cout << "0";
        return count;
    }

    for (int i = 0; i < str.length(); i++)
    {
        if (str[i] == ' ')
            count++;
    }
    if (str[0] == ' ')
        count--;
    if (str[str.length() - 1] == ' ')
        count--;

    cout << count + 1;

    return 0;
}