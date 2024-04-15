# 해시 탐색(Hash Search)

## 해시 탐색이란?
  해시 탐색(Hash Search)은 **[해시 함수를][Hashlink]** 사용하여 데이터를 저장하고 검색하는 알고리즘으로, 해시 함수를 통해 데이터가 저장될 위치를 계산하고, 해당 위치에 데이터를 저장하거나 검색합니다.

### 해시 탐색의 동작 원리
1. 해시 함수는 입력 데이터를 받아 해시 테이블의 인덱스로 변환합니다.
2. 변환된 인덱스에 해당하는 위치에 데이터를 저장하거나 검색합니다.

## 해시 탐색의 특징
 ### 장점
* 해시 탐색은 평균적으로 매우 빠른 검색 속도를 제공합니다.
* 데이터의 키(Key)를 해시 함수를 통해 변환하기 때문에, 키가 바로 저장 위치를 결정하여 검색 시간을 줄일 수 있습니다.

 ### 단점
* 해시 충돌이 발생할 수 있으며, 충돌을 처리하는 방법에 따라 성능이 달라질 수 있습니다.
* 해시 함수의 선택이 중요하며, 부적절한 해시 함수는 해시 충돌을 증가시킬 수 있습니다.
 
## 시간복잡도
 해시 탐색의 시간 복잡도는 일반적으로 O(1)입니다. 하지만 해시 충돌이 발생할 경우 충돌을 해결하는 방법에 따라 시간 복잡도가 증가할 수 있습니다.

## 예시코드 c++
```cpp
#include <iostream>
#include <vector>
using namespace std;

class HashTable 
{
private:
    static const int tableSize = 10;
    vector<pair<int, string>> table[tableSize]; // 해시 테이블

    // 해시 함수: 간단하게 키를 나눈 나머지를 반환
    int hashFunction(int key) 
    {
        return key % tableSize;
    }

public:
    // 데이터 삽입
    void Insert(int key, string value) 
    {
        int index = hashFunction(key);
        table[index].push_back({key, value});
    }

    // 데이터 검색
    string Search(int key) 
    {
        int index = hashFunction(key);
        for (int i = 0; i < table[index].size(); ++i) 
        {
            if (table[index][i].first == key)
                return table[index][i].second;
        }
        return "Not found";
    }
};

int main() 
{
    // 해시 테이블 생성
    HashTable hashtable;

    // 데이터 삽입
    hashtable.Insert(1, "apple");
    hashtable.Insert(11, "banana");

    // 데이터 검색
    cout << hashtable.Search(1) << endl;   // 출력: apple
    cout << hashtable.Search(11) << endl;  // 출력: banana

    return 0;
}
``` 

## 사용예시 & 코딩테스트 유형
* 해시 탐색은 데이터베이스, 캐시 시스템, 검색 엔진 등 다양한 분야에서 사용됩니다.
* 코딩 테스트에서는 해시를 사용한 데이터 처리 및 검색 문제가 자주 출제됩니다.

[Hashlink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/DataStructures/HashTable