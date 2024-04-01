#include <iostream>
using namespace std;

const int MAX_SIZE = 100;
/*
Problem Number: 2563

Problem Description :
가로, 세로의 크기가 각각 100인 정사각형 모양의 흰색 도화지가 있다. 
이 도화지 위에 가로, 세로의 크기가 각각 10인 정사각형 모양의 검은색 색종이를 색종이의 변과 도화지의 변이 평행하도록 붙인다.
이러한 방식으로 색종이를 한 장 또는 여러 장 붙인 후 색종이가 붙은 검은 영역의 넓이를 구하는 프로그램을 작성하시오.
https://upload.acmicpc.net/6000c956-1b07-4913-83c3-72eda18fa1d1/-/preview/
예를 들어 흰색 도화지 위에 세 장의 검은색 색종이를 그림과 같은 모양으로 붙였다면 검은색 영역의 넓이는 260이 된다.

Link: https://www.acmicpc.net/problem/2563

Input:
첫째 줄에 색종이의 수가 주어진다. 이어 둘째 줄부터 한 줄에 하나씩 색종이를 붙인 위치가 주어진다. 
색종이를 붙인 위치는 두 개의 자연수로 주어지는데 첫 번째 자연수는 색종이의 왼쪽 변과 도화지의 왼쪽 변 사이의 거리이고,
두 번째 자연수는 색종이의 아래쪽 변과 도화지의 아래쪽 변 사이의 거리이다. 색종이의 수는 100 이하이며, 색종이가 도화지 밖으로 나가는 경우는 없다

Output:
첫째 줄에 색종이가 붙은 검은 영역의 넓이를 출력한다.

Limit: none
*/

int main()
{
    int number;
    bool checkPoint[MAX_SIZE][MAX_SIZE] = { false, };
    int count = 0;
    cin >> number;


    for (int i = 0; i < number; i++)
    {
        int x, y;
        cin >> x >> y;
        

        for (int X = x; X < x + 10; X++)
        {
            for (int Y = y; Y < y + 10; Y++)
            {
                checkPoint[X][Y] = true;
            }
        }
    }

    for (int i = 0; i < MAX_SIZE; i++)
    {
        for (int j = 0; j < MAX_SIZE; j++)
        {
            if (checkPoint[i][j])
            {
                count++;
            }
        }
    }

    cout << count << endl;

    return 0;
}