#include <iostream>

int addition(int a, int b)
{
	return a + b;
}

int main()
{
	int a, b;
	std::cin >> a >> b;
	std::cout << addition(a,b) << std::endl;
}