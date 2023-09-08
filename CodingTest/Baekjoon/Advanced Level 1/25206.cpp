#include <iostream>
#include <algorithm>
#include <string>
using namespace std;
/*
Problem Number: 25206

Problem Description :
인하대학교 컴퓨터공학과를 졸업하기 위해서는, 전공평점이 3.3 이상이거나 졸업고사를 통과해야 한다. 
그런데 아뿔싸, 치훈이는 깜빡하고 졸업고사를 응시하지 않았다는 사실을 깨달았다!
치훈이의 전공평점을 계산해주는 프로그램을 작성해보자.
전공평점은 전공과목별 (학점 × 과목평점)의 합을 학점의 총합으로 나눈 값이다.
인하대학교 컴퓨터공학과의 등급에 따른 과목평점은 다음 표와 같다.

P/F 과목의 경우 등급이 P또는 F로 표시되는데, 등급이 P인 과목은 계산에서 제외해야 한다.
과연 치훈이는 무사히 졸업할 수 있을까?

Link: https://www.acmicpc.net/problem/25206

Input:
20줄에 걸쳐 치훈이가 수강한 전공과목의 과목명, 학점, 등급이 공백으로 구분되어 주어진다.

Output:
치훈이의 전공평점을 출력한다.
정답과의 절대오차 또는 상대오차가
10^-4 이하이면 정답으로 인정한다.


Limit: 
1 ≤ 과목명의 길이 ≤ 50
과목명은 알파벳 대소문자 또는 숫자로만 이루어져 있으며, 띄어쓰기 없이 주어진다. 입력으로 주어지는 모든 과목명은 서로 다르다.
학점은 1.0,2.0,3.0,4.0중 하나이다.
등급은 A+,A0,B+,B0,C+,C0,D+,D0,F,P중 하나이다.
적어도 한 과목은 등급이 P가 아님이 보장된다.

*/

int main()
{
    cout << fixed;
    cout.precision(6);

    string subjectName;
    float grade;
    float sumGrades=0;
    float majorScore=0;
    string rating;

    for (int i = 0; i < 20; i++)
    {
        cin >> subjectName >> grade >> rating;

        bool isPass = false;
        float score = 0;
        if (rating == "A+")
        {
            score = 4.5;
        }
        else if (rating == "A0")
        {
            score = 4.0;
        }
        else if (rating == "B+")
        {
            score = 3.5;
        }
        else if (rating == "B0")
        {
            score = 3.0;
        }
        else if (rating == "C+")
        {
            score = 2.5;
        }
        else if (rating == "C0")
        {
            score = 2.0;
        }
        else if (rating == "D+")
        {
            score = 1.5;
        }
        else if (rating == "D0")
        {
            score = 1.0;
        }
        else if (rating == "F")
        {
            score = 0;
        }
        else
        {
            isPass = true;
        }

        if (!isPass)
        {
            sumGrades += grade;
            majorScore += grade * score;
        }
    }

    cout << majorScore / sumGrades << endl;

    return 0;
}