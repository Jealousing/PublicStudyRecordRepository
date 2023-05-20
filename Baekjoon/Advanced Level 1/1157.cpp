#include <iostream>
#include <algorithm>
#include <string>
using namespace std;
/*
Problem Number: 1157

Problem Description :
알파벳 대소문자로 된 단어가 주어지면, 이 단어에서 가장 많이 사용된 알파벳이 무엇인지 알아내는 프로그램을 작성하시오. 
단, 대문자와 소문자를 구분하지 않는다.

Link: https://www.acmicpc.net/problem/1157

Input:
첫째 줄에 알파벳 대소문자로 이루어진 단어가 주어진다. 주어지는 단어의 길이는 1,000,000을 넘지 않는다.

Output:
첫째 줄에 이 단어에서 가장 많이 사용된 알파벳을 대문자로 출력한다. 단, 가장 많이 사용된 알파벳이 여러 개 존재하는 경우에는 ?를 출력한다.

Limit: none
*/

int main()
{
    string str1;
    int check[26] {0};

    cin >> str1;

    // 소문자화
    transform(str1.begin(), str1.end(), str1.begin(),::toupper);

    for (int i = 0; i < str1.size(); i++)
    {
        check[str1[i] - 65]  += 1;
    }

    int maxCount=0;
    int checkPoint = 0;
    bool doubleMaxCheck = false;

    for (int i = 0; i < 26; i++)
    {
        if (maxCount < check[i])
        {
            doubleMaxCheck = false;
            checkPoint = i;
            maxCount = check[i];
        }
        else if (maxCount == check[i])
        {
            doubleMaxCheck = true;
            checkPoint = i;
            maxCount = check[i];
        }
    }

    if (doubleMaxCheck)
    {
        cout << "?" ;
    }
    else
    {
        cout << (char)(checkPoint + 65) ;
    }

    return 0;
}