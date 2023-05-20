#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

const int MAX_SIZE = 1000;
/*
Problem Number: 2941

Problem Description :
예전에는 운영체제에서 크로아티아 알파벳을 입력할 수가 없었다. 따라서, 다음과 같이 크로아티아 알파벳을 변경해서 입력했다.
예를 들어, ljes=njak은 크로아티아 알파벳 6개(lj, e, š, nj, a, k)로 이루어져 있다. 단어가 주어졌을 때, 몇 개의 크로아티아 알파벳으로 이루어져 있는지 출력한다.
dž는 무조건 하나의 알파벳으로 쓰이고, d와 ž가 분리된 것으로 보지 않는다. lj와 nj도 마찬가지이다. 위 목록에 없는 알파벳은 한 글자씩 센다.

Link: https://www.acmicpc.net/problem/2941

Input:
첫째 줄에 최대 100글자의 단어가 주어진다. 알파벳 소문자와 '-', '='로만 이루어져 있다.
단어는 크로아티아 알파벳으로 이루어져 있다. 문제 설명의 표에 나와있는 알파벳은 변경된 형태로 입력된다.

Output:
입력으로 주어진 단어가 몇 개의 크로아티아 알파벳으로 이루어져 있는지 출력한다.

Limit: none
*/

int main()
{
    string str;
    int count = 0;
    getline(cin, str);

    for (int i = 0; i < str.length(); i++)
    {
        if (i + 1 == str.length())
        {
            if (str[i] != ' ')
                count++;
        }
        else
        {
            if (str[i] == 'c')
            {
                if (str[i + 1] == '=' || str[i + 1] == '-')
                {
                    count++;
                    i++;
                }
                else
                {
                    count++;
                }
            }
            else if (str[i] == 'd')
            {
                if (str[i + 1] == 'z' && str[i + 2] == '=')
                {
                    count++;
                    i += 2;
                }
                else if (str[i + 1] == '-')
                {
                    count++;
                    i++;
                }
                else
                {
                    count++;
                }
            }
            else if (str[i] == 'l' && str[i + 1] == 'j')
            {
                count++;
                i++;
            }
            else if (str[i] == 'n' && str[i + 1] == 'j')
            {
                count++;
                i++;
            }
            else if ((str[i] == 's' || str[i] == 'z') && str[i + 1] == '=')
            {
                count++;
                i++;
            }
            else if (str[i] != ' ')
            {
                count++;
            }
        }
    }
    cout << count;
    

    return 0;
}