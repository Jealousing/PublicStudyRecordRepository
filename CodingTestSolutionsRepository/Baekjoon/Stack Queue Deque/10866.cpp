#include <iostream>
#include <algorithm>

using namespace std;

/*
Problem Number: 10866

Problem Description :
정수를 저장하는 덱(Deque)를 구현한 다음, 입력으로 주어지는 명령을 처리하는 프로그램을 작성하시오.
명령은 총 여덟 가지이다.
push_front X: 정수 X를 덱의 앞에 넣는다.
push_back X: 정수 X를 덱의 뒤에 넣는다.
pop_front: 덱의 가장 앞에 있는 수를 빼고, 그 수를 출력한다. 만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
pop_back: 덱의 가장 뒤에 있는 수를 빼고, 그 수를 출력한다. 만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
size: 덱에 들어있는 정수의 개수를 출력한다.
empty: 덱이 비어있으면 1을, 아니면 0을 출력한다.
front: 덱의 가장 앞에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
back: 덱의 가장 뒤에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.

Link: https://www.acmicpc.net/problem/10866

Input:
첫째 줄에 주어지는 명령의 수 N (1 ≤ N ≤ 10,000)이 주어진다. 둘째 줄부터 N개의 줄에는 명령이 하나씩 주어진다.
주어지는 정수는 1보다 크거나 같고, 100,000보다 작거나 같다. 문제에 나와있지 않은 명령이 주어지는 경우는 없다.

Output:
출력해야하는 명령이 주어질 때마다, 한 줄에 하나씩 출력한다.

Limit: none
*/

#define MAX 20002
int deque[MAX];

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int t, head = MAX/2-1, tail = MAX/2, temp = 0;
	cin >> t;
	/*
	push_front X: 정수 X를 덱의 앞에 넣는다.
	push_back X: 정수 X를 덱의 뒤에 넣는다.
	pop_front: 덱의 가장 앞에 있는 수를 빼고, 그 수를 출력한다. 만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
	pop_back: 덱의 가장 뒤에 있는 수를 빼고, 그 수를 출력한다. 만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
	size: 덱에 들어있는 정수의 개수를 출력한다.
	empty: 덱이 비어있으면 1을, 아니면 0을 출력한다.
	front: 덱의 가장 앞에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
	back: 덱의 가장 뒤에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
	*/

	for (int i = 0; i < t; i++)
	{
		string str;
		cin >> str;

		if (str == "push_front")
		{
			cin >> temp;
			if (deque[head] == 0)
			{
				deque[head--] = temp;
			}
		}
		else if (str == "push_back")
		{
			cin >> temp;
			if (deque[tail] == 0)
			{
				deque[tail++] = temp;
			}
		}
		else if (str == "pop_front")
		{
			temp = deque[head+1];
			if (temp == 0) cout << -1 << '\n';
			else
			{
				cout << temp << '\n';
				deque[head+1] = 0;
				head++;
			}
		}
		else if (str == "pop_back")
		{
			temp = deque[tail-1];
			if (temp == 0) cout << -1 << '\n';
			else
			{
				cout << temp << '\n';
				deque[tail-1] = 0;
				tail--;
			}
		}
		else if (str == "size")
		{
			cout << tail - head-1 << '\n';
		}
		else if (str == "empty")
		{
			temp = tail - head-1;
			if (temp == 0) cout << 1 << '\n';
			else cout << 0 << '\n';
		}
		else if (str == "front")
		{
			temp = deque[head+1];
			if (temp == 0) cout << -1 << '\n';
			else cout << temp << '\n';
		}
		else if (str == "back")
		{
			temp = deque[tail-1];
			if (temp == 0) cout << -1 << '\n';
			else cout << temp << '\n';
		}
	}

	return 0;
}