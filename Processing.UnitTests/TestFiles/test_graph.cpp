// Test is to determine the shortest path between two given vertices (-1 if it can't be reached)
// Data will be input through the stdin in this order:
// Number of vertices, Number of edges, Starting vertex index, Finish vertex index
// Following Number of edges lines will contain:
// From vertex, To vertex, Cost

#include <iostream>
#include <vector>
#include <queue>
#define INF 0x3f3f3f3f

using namespace std;

typedef pair<int, int> pii;

struct Vertex
{
	std::vector<pii> adjacent; // To, Cost
};

// Returns a vector with distances to all vertices from starting vertex
int dijkstra(vector<Vertex> vertices, int starting, int finish)
{
	int dist[vertices.size()];
	for (int i = 0; i < vertices.size(); i++)
		dist[i] = INF;

	priority_queue<pii, vector<pii>, greater<pii> > pq;
	pq.push(pii(starting, 0));
	dist[starting] = 0;

	while(!pq.empty())
	{
		pii top = pq.top();
		pq.pop();

		for(int i = 0; i < vertices[top.first].adjacent.size(); i++)
		{
			pii adj = vertices[top.first].adjacent[i];

			int newCost = dist[top.first] + adj.second;
			if (newCost < dist[adj.first])
			{
				dist[adj.first] = newCost;
				pq.push(pii(adj.first, newCost));
			}
		}
	}

	if (dist[finish] != INF)
		return dist[finish];
	else
		return -1;
}

int main()
{
	int vertexCount, edgeCount, starting, finish;
	cin >> vertexCount >> edgeCount >> starting >> finish;

	vector<Vertex> vertices(vertexCount);
	for(int i = 0; i < edgeCount; i++)
	{
		int a, b, c;
		cin >> a >> b >> c;

		vertices[a].adjacent.push_back(pii(b, c));
		vertices[b].adjacent.push_back(pii(a, c)); // Bidirectional
	}
	
	cout << dijkstra(vertices, starting, finish) << endl;
}