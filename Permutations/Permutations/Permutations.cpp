// Permutations.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <string.h>

void swap(char *x, char *y)
{
	char temp = *x;
	*x = *y;
	*y = temp;
}

int g_count = 0;


//char* - string
// int start - start index
//int end  - end index
void Permute(char *s, int start, int end)
{
	if (start == end)
	{
		printf_s("%s\n", s);
		g_count++;
	}
	else
	{
		for (size_t i = start; i <= end; i++)
		{
			swap(s + start, s + i);
			Permute(s, start + 1, end);
			swap(s + start, s + i); //backtrack
		}			
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	char str[] = "ABC";
	int len = strlen(str);
	Permute(str, 0, len-1);

	printf("%d\n", g_count);
	return 0;
}

