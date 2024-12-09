#ifndef SHAPES_H
#define SHAPES_H
#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <iostream>
#include <vector>
#include <fstream>
#include <sstream>
#include <vector>
class Vertex
{
public:
    GLfloat x, y, z;
    GLfloat r, g, b;
    Vertex(GLfloat x, GLfloat y, GLfloat z, GLfloat r, GLfloat g, GLfloat b) : x(x), y(y), z(z), r(r), g(g), b(b) {}
};
// extern указывает, что массивы будут определены в другом файле.
//extern  Vertex cubeVertices2[];
extern const std::vector<GLfloat> cubeVertices;
extern const std::vector<GLfloat> tetrahedronVertices;



#endif SHAPES_H