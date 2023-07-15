단계명 : 우선순위 큐    
설명 : 가장 작은/큰 원소를 뽑는 자료구조를 배워 봅시다.     
Link : [백준.우선순위 큐](https://www.acmicpc.net/step/13)  

### Key Takeaways   
먼저 들어오는 데이터가 아니라, 우선순위가 높은 데이터가 먼저 나가는 형태의 자료구조    
priority_queue를 사용하면 쉽게 풀이가능하다.   
 
1) 가장 큰 값이 top   
priority_queue<int> queue -> priority_queue<int, vector<int>, less<int>> queue   
2) 가장 작은 값이 top , first로 정렬 및 second으로 출력하기위한 방법    
priority_queue<pair<int,int>, vector<pair<int,int>>,greater<>> queue    
