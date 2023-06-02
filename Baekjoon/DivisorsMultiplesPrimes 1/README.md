단계명 : 약수, 배수와 소수  
설명 : 약수와 배수는 정수론의 시작점이라고 할 수 있습니다.  
Link : [백준.약수, 배수와 소수](https://www.acmicpc.net/step/10)  

### Key Takeaways  

``` c++

//n의 제곱근까지
for (int i = 2; i <= sqrt(n); i++)  
{  
	// 소수판별  
	if (n % i == 0)  
	{  
		return false;  
	}  
	return true;  
}  

```