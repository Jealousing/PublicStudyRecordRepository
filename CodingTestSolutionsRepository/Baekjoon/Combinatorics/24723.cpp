#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
#include <vector>
using namespace std;

/*
Problem Number: 24723

Problem Description :
Naver D2를 아시나요? D2는 For Developers, By Developers의 약자로, 개발자들을 위해 개발자들이 직접 만들어 가고 있는 네이버 개발자 지원 프로그램입니다. 
네이버가 축적한 기술과 지식을 공유하고, 외부 개발자들을 지원해 대한민국 개발자 역량 강화를 이끌고, 이를 통해 업계 전체와 네이버가 함께 성장하는 선순환 구조를 만들고자 합니다.
사실 네이버의 개발자 지원은 오랜 기간 꾸준히 이어져 왔습니다. 개발자 컨퍼런스 DEVIEW를 비롯, 오픈 소스와 개발 도구 공개, 학회 및 커뮤니티 지원 등 여러 지원 프로그램이 있었습니다. 
이런 다양한 프로그램을 하나로 통합한 것이 바로 Naver D2입니다.

2022년 봄 어느 날.
전 세계에 코딩괴물이 나타났다. 그리고 코딩괴물과 함께 갑작스레 등장한 '그것'... 바로 녹색거탑이다.
녹색거탑의 정상에서는 매년 NAVER가 개최하는 개발자 컨퍼런스 DEVIEW가 열린다. 이 DEVIEW에 참여하면, 코딩에 깊은 깨달음을 얻어 코딩괴물이 될 수 있다고 전해진다. 
그리고 코딩괴물은 녹색거탑의 정상에서 내려온다. 예전부터 전해 내려오는 D2 비전서에 의하면, 코딩괴물이 녹색거탑의 정상에서 내려오는 경우의 수를 파악한 사람은, 
개발자 컨퍼런스 DEVIEW에 참여할 수 있다 한다. 그리고 DEVIEW에 참여해 본인도 코딩괴물이 될 수 있다!
https://upload.acmicpc.net/db58c1ff-9dcd-4f53-8401-b66d74adcc66/-/preview/
녹색거탑은 위 그림과 같이 규칙적으로 쌓여있다.
그림의 시야에 보이지 않는 블록은 없다. 그림의 시야에 보이는 블록의 윗면만 이용해 녹색거탑을 내려올 수 있다.
녹색거탑이 N층이면, 총 N개의 블록을 이용한 최단 경로로만 내려온다. 녹색거탑을 내려올 때는 정상에서 시작해 노란색 바닥까지, 항상 인접한 아래층의 블록으로만 내려온다.
녹색거탑을 정복하고 DEVIEW에 참여하자.

Link: https://www.acmicpc.net/problem/24723

Input:
녹색거탑의 높이를 나타내는 정수 N이 주어진다. ( 1 <= N <= 5 )

Output:
녹색거탑의 정상에서 바닥으로 내려오는 경우의 수를 출력한다.

Limit: none
*/

void test(int n)
{
	
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	// 1	2	3	4	5	6
	// 2	4	8	16	32 64
	int n,sum=1;
	cin >> n;

	for (int i = 0; i < n; i++)
	{
		sum *= 2;
	}
	cout << sum << '\n';
	return 0;
}