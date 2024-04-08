# 해시 테이블 (Hash Table)

## 해시 테이블이란
해시 테이블은 키-값(key-value) 쌍을 저장하고 검색하는 자료구조입니다. 각 키는 해시 함수를 사용하여 고유한 해시 값으로 변환되며, 이 해시 값은 배열의 인덱스로 사용됩니다. 이를 통해 키를 기반으로 한 빠른 데이터 접근이 가능합니다.

### 핵심 기능
* 삽입(insert): 주어진 키와 값을 해시 테이블에 추가합니다.
* 검색(search): 주어진 키에 해당하는 값을 반환합니다.
* 삭제(delete): 주어진 키에 해당하는 항목을 해시 테이블에서 제거합니다.

### 해시 함수 (Hash Function)
해시 함수는 임의의 크기를 가진 데이터를 고정된 크기의 값으로 변환하는 함수입니다. 좋은 해시 함수는 가능한 모든 입력 값을 고르게 분포시켜야 하며, 이는 해시 충돌을 최소화하고 효율적인 검색을 가능하게 합니다.

#### 해시 함수 예시
해시 함수는 입력 데이터를 고유한 해시 값으로 변환하는 역할을 합니다.
다음은 문자열 키를 해싱하는 간단한 예시코드입니다.

```cpp
#include <iostream>
#include <string>

using namespace std;

// 간단한 문자열 해시 함수
int simpleHash(const string& key, int tableSize) 
{
    int hashValue = 0;
    for (char ch : key) 
    {
        hashValue = (hashValue * 31 + ch) % tableSize; // 31은 일반적으로 사용되는 소수입니다.
    }
    return hashValue;
}

int main() 
{
    string key = "apple";
    int tableSize = 10;
    int hashValue = simpleHash(key, tableSize);
    cout << "Hash value for key 'apple': " << hashValue << endl;

    return 0;
}
```
위의 예시 코드에서는 simpleHash함수를 사용하여 문자열의 해시 값을 계산합니다.

### 해시 충돌 (Hash Collision)
해시 충돌은 서로 다른 두 개의 키가 같은 해시 값으로 매핑되는 상황을 말합니다.

주로 해시 충돌은 해시 함수의 특성과 데이터의 특성에 따라 발생합니다.
#### 해시 함수의 특성
* 해시 함수는 입력 데이터를 고유한 해시 값으로 변환하는 역할을 합니다. 그러나 때로는 서로 다른 입력에 대해 동일한 해시 값이 생성될 수 있습니다. 이러한 상황에서 충돌이 발생합니다.
* 해시 함수의 특성에 따라 충돌이 발생할 확률이 달라집니다. 만일 해시 함수가 입력 데이터를 골고루 분산시키지 못하는 경우 특정 영역에 데이터가 집중되어 충돌이 자주 발생할 수 있습니다.

#### 데이터의 특성
* 입력 데이터의 특성에 따라 충돌이 발생할 수 있습니다. 예를 들어, 입력 데이터의 범위가 제한되어 있는 경우 충돌이 더 자주 발생할 수 있습니다. 또한, 데이터가 균일하게 분포되지 않는 경우 특정 영역에 데이터가 밀집되어 충돌이 발생할 수 있습니다.

이러한 충돌을 최소화하기 위해서는 적절한 해시 함수를 선택하고, 데이터의 특성을 고려하여 충돌을 줄이는 방법을 적용해야 합니다.

### 해시 충돌 완화 방법
해시 충돌을 완화하는 방법에는 다음과 같은 세 가지 주요 방법이 있습니다.

* 체이닝(Chaining): 체이닝은 각 해시 테이블 슬롯에 연결 리스트를 사용하여 충돌을 해결하는 방식입니다. 충돌이 발생하면 해당 슬롯에 연결 리스트에 노드를 추가합니다. 이 방식은 메모리를 더 사용하지만, 충돌이 발생할 때마다 추가적인 탐사가 필요하지 않기 때문에 성능이 좋습니다.
* 재해싱(Rehashing) 재해싱은 해시 테이블의 크기를 변경하고 기존 항목을 새로운 해시 테이블에 다시 삽입하는 과정입니다. 해시 테이블이 채워지면 충돌이 더 자주 발생하므로, 일정한 기준에 따라 해시 테이블의 크기를 증가시키고 재해시를 수행하여 충돌을 최소화할 수 있습니다.

#### 개방 주소법(Open Addressing)
개방 주소법은 충돌이 발생했을 때 다른 빈 슬롯을 찾기 위해 해시 테이블 내에서 추가적인 탐사를 수행하는 기법입니다. 다음은 개방 주소법의 세 가지 주요 방법입니다.

* 선형 탐사(Linear Probing): 충돌이 발생하면 다음 슬롯을 순차적으로 검사하여 빈 슬롯을 찾습니다. 즉, 충돌이 발생한 슬롯의 다음 위치부터 순차적으로 빈 슬롯을 탐색합니다. 이는 충돌이 발생했을 때 항목을 저장할 수 있는 가장 간단한 방법 중 하나입니다.
* 제곱 탐사(Quadratic Probing): 충돌이 발생한 슬롯으로부터 일정한 간격의 제곱 수만큼 떨어진 위치를 순차적으로 검사합니다. 즉, 충돌이 발생한 위치로부터 1, 4, 9, 16, ... 등의 간격으로 떨어진 위치를 순차적으로 검사하여 빈 슬롯을 찾습니다. 이는 선형 탐사보다 더 넓은 범위를 탐색하므로 더 효율적입니다.
* 이중 해싱(Double Hashing): 이중 해싱은 두 개의 해시 함수를 사용하여 충돌을 해결하는 방법입니다. 충돌이 발생하면 다른 해시 함수를 사용하여 새로운 주소를 찾습니다. 이를 통해 충돌이 발생할 때마다 선형 탐사나 다른 방법을 사용하는 것보다 더 효율적으로 충돌을 해결할 수 있습니다.

## 해시 테이블 특징
 
 ### 장점
* 해시 테이블을 사용하면 키를 이용하여 매우 빠르게 데이터를 검색할 수 있습니다.
* 삽입, 검색, 삭제 연산의 시간복잡도가 평균적으로 O(1)에 가깝습니다.

 ### 단점
* 해시 충돌(Collision)이 발생할 수 있으며, 이를 처리하기 위한 추가적인 처리가 필요합니다.
* 해시 테이블의 크기를 너무 작게 설정하면 충돌이 자주 발생할 수 있고, 너무 크게 설정하면 메모리를 낭비할 수 있습니다.
 
## 해시 테이블을 사용하면 좋은 경우
* 데이터의 삽입, 검색, 삭제가 빈번하게 발생하는 경우에 유용합니다.
* 캐싱(Caching), 데이터베이스 인덱싱(Database Indexing), 중복 검사(Duplicate Detection) 등의 문제를 해결할 때 사용됩니다.
 
## 예시코드 c++
 
### STL을 사용한 해시 테이블 (unordered_map)
* STL의 unordered_map은 해시 테이블을 기반으로 한 키-값 쌍을 저장하는 자료구조입니다.
* 해시 충돌이 발생하면 STL의 unordered_map은 자체적으로 충돌을 처리하며, 사용자는 이를 직접 다룰 필요가 없습니다.
* unordered_map의 시간 복잡도는 삽입, 검색, 삭제 모두 평균적으로 O(1)에 가깝습니다.
```cpp
#include <iostream>
#include <unordered_map>

using namespace std;

int main() 
{
    // 해시 테이블 선언
    unordered_map<string, int> hash_table;

    // 요소 삽입
    hash_table["apple"] = 5;
    hash_table["banana"] = 10;

    // 요소 검색
    cout << "Value of apple: " << hash_table["apple"] << endl;

    // 요소 삭제
    hash_table.erase("banana");

    return 0;
}
```

### 해시 테이블 직접 구현하기 ( 선형 탐사, Open Addressing )

```cpp
#include <iostream>

using namespace std;

class HashTable 
{
private:
    static const int tableSize = 10;
    pair<int, int> table[tableSize]; // 키-값 쌍을 저장하는 배열

    // 해시 함수: 간단하게 키를 tableSize로 나눈 나머지를 사용
    int hashFunction(int key) 
    {
        return key % tableSize;
    }

    // 충돌 처리를 위한 선형 탐사
    int linearProbe(int index) 
    {
        int i = 1;
        // 비어있는 슬롯을 찾을 때까지 탐색
        while (table[(index + i) % tableSize].first != -1) 
        { 
            i++;
        }
        return (index + i) % tableSize; // 비어있는 슬롯의 인덱스 반환
    }

public:
    HashTable() 
    {
        for (int i = 0; i < tableSize; i++) 
        {
            table[i].first = -1; // 키 값으로 -1을 사용하여 슬롯이 비어있음을 표시
        }
    }

    // 삽입 메서드
    void insert(int key, int value) 
    {
        int index = hashFunction(key);
        if (table[index].first == -1) 
        {
            table[index] = make_pair(key, value); // 해당 슬롯이 비어있는 경우 바로 삽입
        } 
        else 
        {
            index = linearProbe(index); // 선형 탐사를 통해 다른 빈 슬롯을 찾음
            table[index] = make_pair(key, value);
        }
    }

    // 검색 메서드
    int search(int key) 
    {
        int index = hashFunction(key);
        int i = 0;
        while (table[(index + i) % tableSize].first != key) 
        { 
            // 해당 키를 가진 슬롯을 찾을 때까지 탐색
            if (table[(index + i) % tableSize].first == -1) 
            {
                return -1; // 키를 찾지 못한 경우
            }
            i++;
        }
        return table[(index + i) % tableSize].second; // 해당 키의 값 반환
    }

    // 삭제 메서드
    void remove(int key) 
    {
        int index = hashFunction(key);
        int i = 0;
        while (table[(index + i) % tableSize].first != key) 
        { 
            // 해당 키를 가진 슬롯을 찾을 때까지 탐색
            if (table[(index + i) % tableSize].first == -1) 
            {
                cout << "Key not found!" << endl;
                return;
            }
            i++;
        }
        table[(index + i) % tableSize].first = -1; // 해당 키를 가진 슬롯을 비움
    }
};

int main() 
{
    HashTable ht;

    ht.insert(10, 100);
    ht.insert(20, 200);

    cout << "Value of key 10: " << ht.search(10) << endl;
    cout << "Value of key 20: " << ht.search(20) << endl;

    ht.remove(10);
    cout << "Value of key 10 after removal: " << ht.search(10) << endl;

    return 0;
}
```
