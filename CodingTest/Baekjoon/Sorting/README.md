단계명 : 정렬  
설명 : 배열의 원소를 순서대로 나열하는 알고리즘을 배워 봅시다.   
Link : [백준.정렬](https://www.acmicpc.net/step/9)  

### Key Takeaways  
sort(data.begin(), data.end()); -> 오름차순정렬  
data.erase(unique(data.begin(), data.end()), data.end()); -> 중복값 제거하는 방법  
unique는 중복원소를 쓰레기값으로 변환 후 뒤로 넘김   
erase는 위치1부터 위치2까지 원소를 제거함  

sort함수는 지정 정렬을 아래와 같이 사용할 수 있다.  
``` c++  
  
// 정렬 알고리즘사용   
sort(profile, profile +N, compare);    
int compare(Data a, Data b)  
{  
	//길이가 짧은 것부터  
	//길이가 같으면 사전 순으로  
	if (a.age == b.age) return a.num < b.num;  
	else return a.age < b.age;  
}  
  
```  

qsort도 마찬가지이다.  
```  c++  
  
// 퀵정렬 알고리즘사용  
qsort(pos, N, sizeof(Pos), compare);  
int compare(const void* a, const void* b)  
{  
	Pos A = *(Pos*)a;  
	Pos B = *(Pos*)b;  
  
	if (A.y > B.y) return 1;  
	else if (A.y == B.y)  
	{  
		if (A.x > B.x)  return 1;  
		else return -1;  
	}  
	else return -1;  
}  

```
