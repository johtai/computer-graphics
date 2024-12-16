#ifndef SHAPES_H
#define SHAPES_H
#include <GL/glew.h>
#include <SFML/Graphics.hpp>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <SOIL/SOIL.h>
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

extern const std::vector<GLfloat> tetrafigure;
extern const std::vector<GLfloat> cubefigure;
extern std::vector<GLfloat> circle;

void initCircle();

#endif SHAPES_H