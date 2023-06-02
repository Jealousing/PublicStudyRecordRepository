단계명 : 입출력과 사칙연산
설명 : 입력, 출력과 사칙연산을 연습해 봅시다. Hello World!
Link : [백준.입출력과 사칙연산](https://www.acmicpc.net/step/1)

### Key Takeaways

기본 입력 출력으로 문제를 푸는 단계이며, 특수문자 출력을 되돌아보게 만들어주는 문제가 몇개가 있다.

소수점 설정
cout << fixed; -> 소수점 이하의 자릿수만 출력범위 설정할수 있도록 변경.
cout.unsetf(ios::fixed); -> 해제
cout.precision(n); -> 가장 큰 자리수(정수+소수 출력범위)부터 n자리를 출력