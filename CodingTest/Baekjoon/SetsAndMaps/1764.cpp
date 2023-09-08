#include <iostream>
#include <algorithm>
#include <string>
#include <set>
#include <vector>
using namespace std;

/*
Problem Number: 1764

Problem Description :
김진영이 듣도 못한 사람의 명단과, 보도 못한 사람의 명단이 주어질 때, 듣도 보도 못한 사람의 명단을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1764

Input:
첫째 줄에 듣도 못한 사람의 수 N, 보도 못한 사람의 수 M이 주어진다. 이어서 둘째 줄부터 N개의 줄에 걸쳐 듣도 못한 사람의 이름과,
N+2째 줄부터 보도 못한 사람의 이름이 순서대로 주어진다. 이름은 띄어쓰기 없이 알파벳 소문자로만 이루어지며, 그 길이는 20 이하이다. N, M은 500,000 이하의 자연수이다.
듣도 못한 사람의 명단에는 중복되는 이름이 없으며, 보도 못한 사람의 명단도 마찬가지이다.

Output:
듣보잡의 수와 그 명단을 사전순으로 출력한다.

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);
    int N, M;
	cin >> N >> M;

    set<string> data1;
    set<string> data2;

    for (int i = 0; i < N; ++i)
    {
        string tempStr;
        cin >> tempStr;
        data1.insert(tempStr);
    }

    for (int i = 0; i < M; ++i)
    {
        string tempStr;
        cin >> tempStr;
        if (data1.find(tempStr) != data1.end())
        {
            data2.insert(tempStr);
        }
    }

    cout << data2.size() << '\n';
    for (const string& element : data2) {
        std::cout << element << '\n';
    }

	return 0;
}