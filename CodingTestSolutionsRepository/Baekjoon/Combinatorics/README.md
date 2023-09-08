단계명 : 조합론    
설명 : 경우의 수를 세어 봅시다.    
Link : [백준.조합론](https://www.acmicpc.net/step/61)  

### Key Takeaways  

1) 조합식 : n개의 원소 중에서 k개의 원소를 선택하는 경우의 수  
``` c++  

// 이항계수, Factorial(n) / (Factorial(k) * Factorial(n - k)) 보다 시간복잡도가 낮음.  
long long Combination(int n, int k)  
{  
	long long result = 1;  
	for (int i = 1; i <= k; i++)  
	{  
		result *= (n - i + 1);  
		result /= i;  
	}  
	return result;  
}  
  
```  