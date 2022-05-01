#ifdef _WINDOWS
#include <windows.h>
#else
#include <unistd.h>
#define Sleep(x) usleep((x)*1000)
#endif

#include <iostream>

int main()
{
	Sleep(3000);
	std::cout << "This message should appear after 3 seconds" << std::endl;
}