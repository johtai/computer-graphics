#include "shapes.h"
#include <fstream>
#include <string>     // для std::getline

void ParseObjFromFile(std::string filename) 
{

	std::string line;

	std::ifstream in(filename);
	if (in.is_open()) 
	{
		while (std::getline(in, line)) 
		{

			std::cout << line << std::endl;
		}
		in.close();
	}


}