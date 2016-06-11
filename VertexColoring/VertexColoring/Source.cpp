#include "Source.h"
#include <stdio.h>
#include <conio.h>

Source::Source()
{
}


Source::~Source()
{
}


const int MAX_VERTEX = 5;
const int COLORS = 2;
int map[MAX_VERTEX][MAX_VERTEX];
int vertexes[MAX_VERTEX];

//vertex coloring problem.. Given a connected graph (V,E). and M colors. color the vertexes of the grapph in such a way that 
// no two adjacent vertexes have the same identical color.


//Check to see if it is safe to color the vertex with the color
bool IsSafe(int vertex, int color)
{
	bool isSameColor = false;
	for (size_t i = 0; i < MAX_VERTEX; i++) //columns
	{
		//check adjacency matrix. If not a neighbour then ignore and keep scanning.
		if (map[vertex][i] == 0)
		{
			continue;
		}
		
		if (vertexes[i] == color)// i is the neighbour of vertex. checking if neighbour has same color.
		{
			isSameColor = true;
			break;
		}
	}

	return !isSameColor;
}


//Recursion
bool Solve(int vertex)
{
	if (vertex >= MAX_VERTEX)//base case
	{
		return true;
	}

	for (size_t i = 1; i <= COLORS; i++) //try colors, 0 is no color (1----COLORS)
	{
		if (IsSafe(vertex, i))
		{
			vertexes[vertex] = i;//assign color as it is safe

			if (Solve(vertex + 1))
			{
				return true;
			}

			vertexes[vertex] = 0;//backtrack. reset color as it is not leading to a solution.
		}
	}

	return false; //no solution exists
}


int main(char args[])
{
	//vertexes array - Indices of this array identify vertex.index 0 = node 0, index 1 = node 1

	//adjacency matrix init
	for (size_t i = 0; i < MAX_VERTEX; i++)
	{
		for (size_t j = 0; j < MAX_VERTEX; j++)
		{
			map[i][j] = 0;
		}
	}

	map[0][1] = 1; // (0->1)
	map[1][0] = 1; //reverse

	map[0][2] = 1; // (0 ->2)
	map[2][0] = 1; //reverse

	map[1][3] = 1; //(1 ->3)
	map[3][1] = 1; //reverse 

	map[1][4] = 1;// (1 -> 4)
	map[4][1] = 1;//reverse

	printf_s("\nProvided Colors...");
	for (size_t i = 1; i <= COLORS; i++)
	{
		printf_s("%d ", i);
	}

	printf_s("\nAdjacency matrix...");
	for (size_t i = 0; i < MAX_VERTEX; i++)
	{
		printf_s("\n");
		for (size_t j = 0; j < MAX_VERTEX; j++)
		{
			printf_s("%d\t", map[i][j]);
		}
	}

	
	if (Solve(0))
	{
		// solution exists
		printf_s("\n\nsolution exists \n");

		for (size_t i = 0; i < MAX_VERTEX; i++)
		{
			printf_s("Vertex(%d) has Color = %d.\n ",i, vertexes[i]);
		}
	}
	else
	{
		printf_s("no solution exists");
	}

	return 0;
}


