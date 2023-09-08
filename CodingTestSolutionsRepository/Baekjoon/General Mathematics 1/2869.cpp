#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 2869

Problem Description :
땅 위에 달팽이가 있다. 이 달팽이는 높이가 V미터인 나무 막대를 올라갈 것이다.
달팽이는 낮에 A미터 올라갈 수 있다. 하지만, 밤에 잠을 자는 동안 B미터 미끄러진다. 또, 정상에 올라간 후에는 미끄러지지 않는다.
달팽이가 나무 막대를 모두 올라가려면, 며칠이 걸리는지 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2869

Input:
첫째 줄에 세 정수 A, B, V가 공백으로 구분되어서 주어진다. (1 ≤ B < A ≤ V ≤ 1,000,000,000)

Output:
첫째 줄에 달팽이가 나무 막대를 모두 올라가는데 며칠이 걸리는지 출력한다.

Limit: none
*/

int main()
{
    int targetHeight, curHeight = 0, upDistance, downDistance;
    long long day = 0;
    bool IsNight = false;

    cin >> upDistance >> downDistance >> targetHeight;

    // 시간초과 발생
    /*
     while (true)
    {
        if (!IsNight)
        {
            curHeight += upDistance;
            IsNight = true;
        }
        else
        {
            curHeight -= downDistance;
            day++;
            IsNight = false;
        }

        if (curHeight >= targetHeight) break;
    }
    */

   // 하루에 증가하는량 
    int dayUpDistace = upDistance - downDistance;
    // 목표 거리까지 몇일걸리나
    day = (targetHeight- downDistance) / dayUpDistace;
    // 나머지 거리
    int remainDistance = (targetHeight- downDistance) % dayUpDistace;

    if (remainDistance > 0)
    {
        day++;
    }

    cout << day << endl;

    return 0;
}