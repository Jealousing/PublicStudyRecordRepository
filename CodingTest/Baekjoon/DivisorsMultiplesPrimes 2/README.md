단계명 : 약수, 배수와 소수 2    
설명 : 정수론의 세계로 조금 더 들어가 봅시다.    
Link : [백준.약수, 배수와 소수 2](https://www.acmicpc.net/step/18)  

### Key Takeaways  

1) 소수 검사할 때는 약수는 제곱근을 기준으로 대칭이므로 제곱근(sqrt())까지 검사(횟수 줄이기)  

2) 최대공약수(GCD), 최소공배수(LCM), 유클리드 호제법(gcdEuclidean), 에라토스테네스의 체(Sieve of Eratosthenes)  

```  

int gcd(int a, int b)  
{  
    if (b == 0) return a;  
    return gcd(b, a % b);  
}  

int lcm(int a, int b)   
{  
    return (a * b) / gcd(a, b);  
}  

int gcdEuclidean(int a, int b)   
{  
    while (b != 0)   
    {  
        int remainder = a % b;  
        a = b;  
        b = remainder;  
    }  
    return a;  
} 

vector<int> SieveOfEratosthenes(int n)  
{  
	vector<bool> primes(n + 1, true);  
	primes[0] = primes[1] = false;  

	int p = 2;  
	while (p * p <= n)   
	{  
		if (primes[p])   
		{  
			for (int i = p * p; i <= n; i += p)   
			{  
				primes[i] = false;  
			}  
		}  
		p++;  
	}  
  
	vector<int> result;  
	for (int i = 2; i <= n; i++)   
	{  
		if (primes[i])   
		{  
			result.push_back(i);  
		}   
	}   
	return result;   
}    
  
   
```   