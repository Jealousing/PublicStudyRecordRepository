#include <iostream>
#include <string>
#include <algorithm>
using namespace std;

/*
Problem Number: 1541

Problem Description :
세준이는 양수와 +, -, 그리고 괄호를 가지고 식을 만들었다. 그리고 나서 세준이는 괄호를 모두 지웠다.
그리고 나서 세준이는 괄호를 적절히 쳐서 이 식의 값을 최소로 만들려고 한다.
괄호를 적절히 쳐서 이 식의 값을 최소로 만드는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1541

Input:
첫째 줄에 식이 주어진다. 식은 ‘0’~‘9’, ‘+’, 그리고 ‘-’만으로 이루어져 있고, 가장 처음과 마지막 문자는 숫자이다. 
그리고 연속해서 두 개 이상의 연산자가 나타나지 않고, 5자리보다 많이 연속되는 숫자는 없다. 수는 0으로 시작할 수 있다. 
입력으로 주어지는 식의 길이는 50보다 작거나 같다.

Output:
첫째 줄에 정답을 출력한다.

Limit: none
*/

string str, temp;
int minSum=0;
bool Flag = false;

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    cin >> str;
    // - 뒤가 괄호로 묵여있으면 제일 작은 값 
    for (int i = 0; i <= str.length(); i++) 
    {
        // 기호를 만났거나 식이 끝날때 계산
        if (str[i] == '-' || str[i] == '+' || i == str.length())
        {
            // 마이너스를 찾기전엔 다 +
            if (Flag)
            {
                minSum -= stoi(temp);
                temp.clear();
            }
            else 
            {
                minSum += stoi(temp);
                temp.clear();
            }
        }
        else 
        {
            // 기호가 아닌 경우 숫자저장
            temp += str[i];
        }

        // 마이너스일 경우
        if (str[i] == '-') 
        {
            Flag = true;
        }
    }

    cout << minSum << '\n';
    return 0;
}