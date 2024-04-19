# 최단경로(Shortest Path) 알고리즘

## 최단 경로란?
최단 경로란 그래프 상에서 두 정점 사이를 이동할 때, 가장 적은 비용을 갖는 경로를 의미합니다. 여기서 "비용"이란 간선의 가중치 합을 의미하며, 가중치는 간선의 길이, 비용, 시간 등 다양한 형태로 표현될 수 있습니다.

### 최단 경로의 종류
* 단일 출발 최단 경로 문제: 하나의 정점에서 다른 모든 정점까지의 최단 경로를 찾는 문제입니다. 이 문제는 주로 다익스트라 알고리즘 또는 벨만-포드 알고리즘으로 해결합니다.
* 단일 도착 최단 경로 문제: 모든 정점에서 하나의 정점까지의 최단 경로를 찾는 문제입니다. 이 문제는 단일 출발 최단 경로 문제를 그래프의 모든 정점에 대해 적용하여 해결할 수 있습니다.
* 모든 쌍 최단 경로 문제: 그래프 상의 모든 정점 쌍 사이의 최단 경로를 찾는 문제입니다. 이 문제는 주로 플로이드-워셜 알고리즘으로 해결합니다.

## 최단경로 알고리즘별 사용할 수 있는 유형 및 결과
* **[너비 우선 탐색 알고리즘(BFS):][BreadthFirstSearchlink]** 가중치가 없는 그래프나 가중치가 모두 같은 그래프에서 사용됩니다. 시작 정점에서 각 정점까지의 최단 경로를 찾습니다.
* **[다익스트라 알고리즘:][Dijkstralink]** 음수 간선이 존재하지 않을 때, 한 노드에서 다른 모든 노드까지의 최단 경로/거리를 찾습니다.
* **[벨만-포드 알고리즘:][BellmanFordlink]** 음수 사이클이 존재하지 않을 때, 한 노드에서 다른 모든 노드까지의 최단 경로/거리를 찾습니다.
* **[플로이드-워셜 알고리즘:][FloydWarshalllink]** 음수 사이클이 존재하지 않을 때, 모든 노드에서 모든 노드까지의 최단 경로/거리를 찾습니다.
* **[A* 알고리즘:][AStarlink]** 음수 간선이 없는 그래프에서 사용됩니다. 시작 노드에서 목표 노드까지의 최단 경로를 찾습니다. 
 
## 최단 경로 알고리즘별 시간 복잡도
* 너비 우선 탐색 알고리즘: O(V + E)
* 다익스트라 알고리즘: O((V + E) log V) (우선순위 큐를 사용한 경우)
* 벨만-포드 알고리즘: O(V * E)
* 플로이드-워셜 알고리즘: O(V^3)
* A* 알고리즘: A* 알고리즘의 시간 복잡도는 휴리스틱 함수의 품질과 그래프의 구조에 따라 달라집니다.

		여기서 V는 정점의 수, E는 간선의 수입니다.

## 사용예시 & 코딩테스트 유형
너비 우선 탐색은 그래프 탐색 문제와 관련하여 코딩 테스트에서 자주 출제됩니다. 특히, 최단 경로를 찾거나 그래프의 구조를 분석하는 문제에 적합합니다. 또한, 큐를 사용하는 방식과 최단 경로를 보장한다는 특성으로 인해 BFS 관련 문제는 코딩 테스트에서도 자주 등장합니다.     
A* 알고리즘은 경로 찾기 문제에 주로 사용됩니다. 특히, 게임 개발에서 많이 사용합니다.    


[BreadthFirstSearchlink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/Algorithms/GraphAlgorithms/BreadthFirstSearch
[Dijkstralink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/Algorithms/GraphAlgorithms/ShortestPath/Dijkstra
[BellmanFordlink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/Algorithms/GraphAlgorithms/ShortestPath/BellmanFord
[FloydWarshalllink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/Algorithms/GraphAlgorithms/ShortestPath/FloydWarshall
[AStarlink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/Algorithms/GraphAlgorithms/ShortestPath/AStar