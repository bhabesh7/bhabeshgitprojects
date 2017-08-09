// PuzzleGap_August9_Exam.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include <limits>
#include <iostream>
using namespace std;
//Question - Given a puzzle of 5x5, which are filled with numbers from 1-9(24 numbers) and 1 number filled with 0.
//With Input K provided, move the gap to 4 directions of neighbours -- 
//and then find sum of diagonals (left top to right bottom) and compare with K. When these are equal,
// print minimum number of moves needed for that. If number if number of moves > 8 then -1.
const int MAX = 5;
int rowAd[] = { 0, -1, 0,1 };
int colAd[] = { -1, 0, 1, 0 };
int minimum = INT_MAX;

bool exchange(int srow, int scol, int nrow, int ncol, int puzzle[MAX][MAX])
{
	int temp = puzzle[srow][scol];
	puzzle[srow][scol] = puzzle[nrow][ncol];
	puzzle[nrow][ncol] = temp;
	return true;
}

bool issafe(int row, int col)
{
	if (row >= 0 && row <= MAX && col >= 0 && col <= MAX)
	{
		return true;
	}
	return false;
}

int sumdiag(int puzzle[MAX][MAX])
{
	int sum = 0;
	for (int i = 0; i < MAX; i++)
	{
		sum += puzzle[i][i];
	}
	return sum;
}

int solve(int srow, int scol, int moves, int K, int puzzle[MAX][MAX])
{
	if (moves > 8)
	{
		return -1;
	}

	if (sumdiag(puzzle) == K)
	{
		return moves;
	}

	//for 4 directions
	for (int i = 0; i < 4; i++)
	{
		int nrow = srow + rowAd[i];
		int ncol = scol + colAd[i];

		if (issafe(nrow, ncol))
		{
			int puzz[MAX][MAX];

			exchange(srow, scol, nrow, ncol, puzzle);

			//create a puzzle
			for (int i = 0; i < MAX; i++)
			{
				for (int j = 0; j < MAX; j++)
				{
					puzz[i][j] = puzzle[i][j];
				}
			}

			int res = solve(nrow, ncol, moves + 1, K, puzz);
			//cout << res<<":";
			//reset config
			exchange(srow, scol, nrow, ncol, puzzle);

			if (res > 0)
			{
				if (minimum == -1)
				{
					minimum = res;
				}
				else if (res < minimum)
				{
					minimum = res;
				}
			}
			else if (res == -1)
			{
				if (minimum == INT_MAX)
				{
					minimum = -1;
				}				
			}

		}
	}
}


int main()
{	
	minimum = INT_MAX;
	int puzzle[MAX][MAX] = {
		{1,2,4,5,6},
		{3,5,6,8,2},
		{7,9,2,5,1},
		{6,5,3,2,8},
		{2,4,4,0,1} };

	int K = 9;

	int res = solve(4, 3, 0, 9, puzzle);
	cout << res << " " << minimum;
	
	return 0;
}

