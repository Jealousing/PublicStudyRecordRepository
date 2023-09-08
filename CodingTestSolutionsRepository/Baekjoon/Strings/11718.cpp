#include <iostream>
#include <algorithm>
#include <string>
using namespace std;
/*
Problem Number: 11718

Problem Description :
입력 받은 대로 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11718

Input:
입력이 주어진다. 입력은 최대 100줄로 이루어져 있고, 알파벳 소문자, 대문자, 공백, 숫자로만 이루어져 있다. 
각 줄은 100글자를 넘지 않으며, 빈 줄은 주어지지 않는다. 또, 각 줄은 공백으로 시작하지 않고, 공백으로 끝나지 않는다.

Output:
입력받은 그대로 출력한다.

Limit: none
*/

int main()
{
    int count = 0;
    string str;
    
    while (true)
    {
        getline(cin, str);
        if (str == "") break;
        else
            cout << str << endl;
    }
   

    return 0;
}