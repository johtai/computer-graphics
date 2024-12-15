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
class Object3D
{
public:
    GLfloat x, y, z;
    GLfloat r, g, b;
    GLfloat s, t;
    Object3D(GLfloat x, GLfloat y, GLfloat z, GLfloat r, GLfloat g, GLfloat b, GLfloat s, GLfloat t) : x(x), y(y), z(z), r(r), g(g), b(b), s(s), t(t) {}
};
// extern указывает, что массивы будут определены в другом файле.
//extern  Vertex cubeVertices2[];
//extern std::vector<GLfloat> loadedVertices;
std::vector<GLfloat>  ParseObjFromFile(const std::string& filePath);
extern const std::vector<GLfloat> tetrafigure;
extern const std::vector<GLfloat> cubefigure;

#endif SHAPES_H